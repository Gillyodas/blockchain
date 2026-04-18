using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ChainDegree.Application.Common.Persistence;
using ChainDegree.Application.Common.Services;
using ChainDegree.Application.QuanLyToChuc.Interfaces.Repositories;
using ChainDegree.Domain.QuanLyToChuc.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ChainDegree.Infrastructure.QuanLyToChuc.BackgroundServices;

public class CoSoDaoTaoApprovedEventProcessor : BackgroundService
{
    private readonly ILogger<CoSoDaoTaoApprovedEventProcessor> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;

    // Thời gian poll: Mỗi 5 giây kiểm tra có event mới không
    private const int POLL_INTERVAL_MS = 5000;

    // Nếu xử lý fail, chờ bao lâu rồi retry
    private const int INITIAL_BACKOFF_MS = 2000;

    // Sau khi restart Besu, chờ bao lâu để Besu startup xong
    private const int BESU_STARTUP_DELAY_MS = 5000;

    public CoSoDaoTaoApprovedEventProcessor(
        ILogger<CoSoDaoTaoApprovedEventProcessor> logger,
        IServiceProvider serviceProvider,
        IConfiguration configuration)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("CoSoDaoTaoApprovedEventProcessor bắt đầu.");

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                // Sử dụng scope để làm mới DbContext mỗi lần xử lý
                using var scope = _serviceProvider.CreateScope();
                var unitOfWork = scope.ServiceProvider
                    .GetRequiredService<IUnitOfWork>();
                var csdtEventRepo = scope.ServiceProvider
                    .GetRequiredService<ICoSoDaoTaoApprovedEventRepository>();
                var besuService = scope.ServiceProvider
                    .GetRequiredService<IBesuService>();
                var fileService = scope.ServiceProvider
                    .GetRequiredService<IFileService>();

                // Lấy event chưa xử lý
                var unprocessedEvents = await csdtEventRepo
                    .GetUnprocessedEventsAsync(cancellationToken);
                if (unprocessedEvents.Any())
                {
                    _logger.LogInformation("Tìm thấy {Count} event chưa xử lý.", unprocessedEvents.Count);
                }

                foreach (var @event in unprocessedEvents)
                {
                    await ProcessEventAsync(
                        @event,
                        unitOfWork,
                        csdtEventRepo,
                        besuService,
                        fileService,
                        cancellationToken);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xử lý CoSoDaoTaoApprovedEvent. Sẽ retry sau {Delay} ms.", INITIAL_BACKOFF_MS);
            }
            await Task.Delay(POLL_INTERVAL_MS, cancellationToken);
        }
        _logger.LogInformation("CoSoDaoTaoApprovedEventProcessor kết thúc.");
    }

    private async Task ProcessEventAsync(
        CoSoDaoTaoApprovedEvent @event,
        IUnitOfWork unitOfWork,
        ICoSoDaoTaoApprovedEventRepository csdtEventRepo,
        IBesuService besuService,
        IFileService fileService,
        CancellationToken cancellationToken)
    {
        var stopWatch = Stopwatch.StartNew();
        var backoffMs = INITIAL_BACKOFF_MS * (int)Math.Pow(2, @event.RetryCount);

        _logger.LogInformation("Bắt đầu xử lý event Id: {EventId}, RetryCount: {RetryCount}", @event.Id, @event.RetryCount);

        try
        {
            var addresses = @event.GetPayload<List<string>>()
                ?? new List<string>();

            if (addresses.Count == 0)
            {
                throw new InvalidOperationException(
                    "Event payload không chứa địa chỉ ví");
            }

            _logger.LogDebug("Danh sách CSDT: {Count} địa chỉ ví", addresses.Count);

            // Ghi vào toEncode.json
            _logger.LogDebug("Ghi toEncode.json...");
            var toEncodePath = GetToEncodePath();
            await fileService.WriteJsonAsync(toEncodePath, addresses, cancellationToken);
            _logger.LogInformation("✅ toEncode.json written");

            // Encode sang RLP
            _logger.LogDebug("2Encoding RLP CSDT...");
            var extraData = await besuService
                .EncodeValidatorsToExtraData(toEncodePath, cancellationToken);
            _logger.LogInformation("RLP encoded: {Data}...", extraData[..50]);

            // Update genesis.json
            _logger.LogDebug("Cập nhật genesis.json...");
            await fileService.UpdateGenesisExtraData(extraData, cancellationToken);
            _logger.LogInformation("genesis.json updated");

            // Restart Besu để áp dụng thay đổi
            _logger.LogDebug("Restart Besu container...");
            await besuService.RestartBesuContainer(cancellationToken);
            _logger.LogInformation("Besu restarted");

            // Chờ Besu startup xong
            _logger.LogDebug("Chờ Besu startup ({DelayMs}ms)...", BESU_STARTUP_DELAY_MS);
            await Task.Delay(BESU_STARTUP_DELAY_MS, cancellationToken);

            // Verify on-chain
            _logger.LogDebug("Xác minh CSDT trên blockchain...");
            var verified = await besuService.VerifyValidators(addresses, cancellationToken);

            if (!verified)
            {
                throw new InvalidOperationException(
                    "CSDT không được tìm thấy trên blockchain sau restart");
            }
            _logger.LogInformation("CSDT verified on-chain");

            // Mark as processed
            @event.IsProcessed = true;
            @event.ProcessedAt = DateTime.UtcNow;
            @event.ErrorMessage = null;
            csdtEventRepo.Update(@event);
            await unitOfWork.CommitAsync(cancellationToken);

            stopWatch.Stop();
            _logger.LogInformation(
                "Event {EventId} xử lý thành công trong {ElapsedMs}ms",
                @event.Id,
                stopWatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            stopWatch.Stop();

            @event.RetryCount++;
            @event.ErrorMessage = ex.Message;

            // Update event (lưu lỗi + retry count)
            csdtEventRepo.Update(@event);
            await unitOfWork.CommitAsync(cancellationToken);

            if (@event.CanRetry)
            {
                _logger.LogWarning(
                    "Event {EventId} xử lý fail. Sẽ retry trong {BackoffMs}ms. " +
                    "Retry count: {RetryCount}/{MaxRetries}. Lỗi: {Error}",
                    @event.Id,
                    backoffMs,
                    @event.RetryCount,
                    5,
                    ex.Message);

                // Exponential backoff
                await Task.Delay(backoffMs, cancellationToken);
            }
            else
            {
                _logger.LogError(
                    "Event {EventId} xử lý fail sau {MaxRetries} lần retry. " +
                    "Cần manual intervention. Lỗi: {Error}",
                    @event.Id,
                    5,
                    ex.Message);

                //TODO: Alert admin hoặc ghi log để monitor sau. Có thể thêm method khác để admin trigger manual retry
            }
        }
    }

    private string GetToEncodePath()
    {
        var baseDir = _configuration["BESU_CONFIG_PATH"]
            ?? "./besu/config";
        return Path.Combine(baseDir, "toEncode.json");
    }
}
