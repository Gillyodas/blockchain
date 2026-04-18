using ChainDegree.Application.Common.Persistence;
using ChainDegree.Application.QuanLyBangCap.Interfaces.Repositories;
using ChainDegree.Application.QuanLyToChuc.Interfaces.Repositories;
using ChainDegree.Domain.QuanLyBangCap.Aggregates;
using ChainDegree.Domain.QuanLyToChuc.Enums;
using ChainDegree.Domain.QuanLyToChuc.Events;
using ChainDegree.SharedKernel.Common.Errors;
using ChainDegree.SharedKernel.QuanLyToChuc;
using ControlHub.SharedKernel.Common.Exceptions;
using ControlHub.SharedKernel.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChainDegree.Application.QuanLyToChuc.Commands.DuyetDangKyCSDT;

public class DuyetDangKyCSDTCommandHandler : IRequestHandler<DuyetDangKyCSDTCommand, Result>
{
    private readonly ILogger<DuyetDangKyCSDTCommandHandler> _logger;
    private readonly IYeuCauDangKyRepository _yeuCauDangKyRepository;
    private readonly ICoSoDaoTaoRepository _coSoDaoTaoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DuyetDangKyCSDTCommandHandler(
        ILogger<DuyetDangKyCSDTCommandHandler> logger,
        IYeuCauDangKyRepository yeuCauDangKyRepository,
        ICoSoDaoTaoRepository coSoDaoTaoRepository,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _yeuCauDangKyRepository = yeuCauDangKyRepository;
        _coSoDaoTaoRepository = coSoDaoTaoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DuyetDangKyCSDTCommand request, CancellationToken cancellationToken)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            _logger.LogInformation("Bắt đầu duyệt đăng ký CSDT ID: {Id}", request.YeuCauDangKyId);

            _logger.LogDebug("Lấy thông tin yêu cầu đăng ký CSDT từ repository");
            var yeuCauDangKy = await _yeuCauDangKyRepository.GetByIdAsync(request.YeuCauDangKyId, cancellationToken);
            if (yeuCauDangKy == null || yeuCauDangKy.Loai != LoaiToChuc.Issuer)
            {
                _logger.LogWarning("Không tìm thấy yêu cầu đăng ký CSDT hợp lệ với ID: {Id}", request.YeuCauDangKyId);
                return Result.Failure(QuanLyToChucError.KhongTimThayYeuCauDangKy);
            }

            _logger.LogDebug("Thực hiện duyệt yêu cầu đăng ký CSDT");
            var approveResult = yeuCauDangKy.AdminDuyet(request.GhiChu);
            if (approveResult.IsFailure) return Result.Failure(approveResult.Error);

            _logger.LogDebug("Tạo cơ sở đào tạo mới từ yêu cầu đăng ký");
            var csdtResult = CoSoDaoTao.Create(
                yeuCauDangKy.TenToChuc,
                yeuCauDangKy.DiaChiVi,
                yeuCauDangKy.GiayPhepCSDTs.ToList());

            if (csdtResult.IsFailure)
            {
                _logger.LogWarning("Lỗi khi tạo cơ sở đào tạo từ yêu cầu đăng ký CSDT ID: {Id}, lỗi: {Error}", request.YeuCauDangKyId, csdtResult.Error);
                return Result.Failure(csdtResult.Error);
            }

            _logger.LogDebug("Lưu cơ sở đào tạo mới vào repository");
            await _coSoDaoTaoRepository.AddAsync(csdtResult.Value, cancellationToken);

            _logger.LogDebug("Lấy danh sách tất cả CSDT đã được duyệt");
            var csdtDaDuyet = await _coSoDaoTaoRepository.GetAllAsync(cancellationToken);

            _logger.LogDebug("Danh sách địa chỉ ví của các CSDT đã được duyệt");
            var danhSachDiaChiViDaDuyet = csdtDaDuyet
                .Select(c => c.DiaChiViCSDT)
                .OrderBy(a => a)
                .ToList();

            _logger.LogDebug(
                "Tạo CoSoDaoTaoApprovedEvent để ghi vào Outbox. Số cơ sở đào tạo: {Count}",
                danhSachDiaChiViDaDuyet.Count);
            var csdtEvent = CoSoDaoTaoApprovedEvent.Create(
                yeuCauDangKyId: yeuCauDangKy.Id,
                diaChiVi: yeuCauDangKy.DiaChiVi,
                tenToChuc: yeuCauDangKy.TenToChuc,
                danhSachDiaChiViDaDuyet: danhSachDiaChiViDaDuyet);

            _logger.LogDebug("Cập nhật trạng thái yêu cầu đăng ký CSDT đã được duyệt");
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            _logger.LogInformation("Duyệt đăng ký CSDT thành công cho yêu cầu ID: {Id}", request.YeuCauDangKyId);
            return Result.Success();
        }
        catch (RepositoryConcurrencyException ex)
        {
            _logger.LogError(ex, "Lỗi xung đột dữ liệu khi duyệt đăng ký CSDT. Lỗi: {ErrorMessage}", ex.Message);
            return Result<Guid>.Failure(ApplicationError.ConcurrencyError);
        }
        catch (RepositoryException ex)
        {
            _logger.LogError(ex, "Lỗi truy cập dữ liệu khi duyệt đăng ký CSDT. Lỗi: {ErrorMessage}", ex.Message);
            return Result<Guid>.Failure(ApplicationError.RepositoryError);
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError(ex, "Lỗi hủy bỏ khi duyệt đăng ký CSDT. Lỗi: {ErrorMessage}", ex.Message);
            return Result<Guid>.Failure(ApplicationError.Cancelled);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi duyệt đăng ký CSDT. Lỗi: {ErrorMessage}", ex.Message);
            return Result<Guid>.Failure(ApplicationError.UnknownError);
        }
    }
}
