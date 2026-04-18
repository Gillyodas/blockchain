using ChainDegree.Application.Common.Persistence;
using ChainDegree.Application.QuanLyToChuc.Interfaces.Repositories;
using ChainDegree.Domain.QuanLyToChuc.Aggregates;
using ChainDegree.SharedKernel.Common.Errors;
using ChainDegree.SharedKernel.QuanLyToChuc;
using ControlHub.SharedKernel.Common.Exceptions;
using ControlHub.SharedKernel.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChainDegree.Application.QuanLyToChuc.Commands.DangKyToChuc;
public class DangKyToChucCommandHandler : IRequestHandler<DangKyToChucCommand, Result<Guid>>
{
    private readonly ILogger<DangKyToChucCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYeuCauDangKyRepository _yeuCauDangKyRepository;

    public DangKyToChucCommandHandler(ILogger<DangKyToChucCommandHandler> logger, IUnitOfWork unitOfWork, IYeuCauDangKyRepository yeuCauDangKyRepository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _yeuCauDangKyRepository = yeuCauDangKyRepository;
    }

    public async Task<Result<Guid>> Handle(DangKyToChucCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Bắt đầu xử lý đăng ký làm tổ chức tạo với tên tổ chức: {TenToChuc}", request.TenToChuc);

            _logger.LogDebug("Kiểm tra địa chỉ ví đã được sử dụng cho yêu cầu đăng ký làm tổ chức với tên tổ chức {TenToChuc} và địa chỉ ví {DiaChiVi}", request.TenToChuc, request.DiaChiVi);
            bool diaChiViDaDuocSuDung = await _yeuCauDangKyRepository.ExistsByDiaChiViAsync(request.DiaChiVi, cancellationToken);
            if(diaChiViDaDuocSuDung)
            {
                _logger.LogWarning("Địa chỉ ví {DiaChiVi} đã được sử dụng cho một yêu cầu đăng ký làm tổ chức khác. Không thể tiếp tục đăng ký làm tổ chức với tên tổ chức {TenToChuc}", request.DiaChiVi, request.TenToChuc);
                return Result<Guid>.Failure(QuanLyToChucError.DiaChiViDaDuocSuDung);
            }

            _logger.LogInformation("Tạo yêu cầu đăng ký làm tổ chức mới với tên tổ chức {TenToChuc}", request.TenToChuc);
            Result<YeuCauDangKy> yeuCauDangKyResult = YeuCauDangKy.Create(request.TenToChuc, request.LoaiToChucDangKy, request.TkId, request.DiaChiVi);
            if (yeuCauDangKyResult.IsFailure)
            {
                _logger.LogWarning("Đăng ký làm tổ chức thất bại do lỗi dữ liệu. Lỗi: {ErrorMessage}", yeuCauDangKyResult.Error);
                return Result<Guid>.Failure(yeuCauDangKyResult.Error);
            }

            _logger.LogInformation("Lưu yêu cầu đăng ký làm tổ chức mới vào cơ sở dữ liệu với tên tổ chức {TenToChuc}", request.TenToChuc);
            await _yeuCauDangKyRepository.AddAsync(yeuCauDangKyResult.Value, cancellationToken);

            _logger.LogInformation("Cam kết giao dịch để hoàn tất đăng ký làm tổ chức với tên tổ chức {TenToChuc}", request.TenToChuc);
            await _unitOfWork.CommitAsync(cancellationToken);

            _logger.LogInformation("Đăng ký làm tổ chức thành công với tên tổ chức {TenToChuc} và ID yêu cầu đăng ký {YeuCauDangKyId}", request.TenToChuc, yeuCauDangKyResult.Value.Id);
            return Result<Guid>.Success(yeuCauDangKyResult.Value.Id);
        }
        catch (RepositoryConcurrencyException ex)
        {
            _logger.LogError(ex, "Lỗi xung đột dữ liệu khi đăng ký làm tổ chức. Lỗi: {ErrorMessage}", ex.Message);
            return Result<Guid>.Failure(ApplicationError.ConcurrencyError);
        }
        catch (RepositoryException ex)
        {
            _logger.LogError(ex, "Lỗi truy cập dữ liệu khi đăng ký làm tổ chức. Lỗi: {ErrorMessage}", ex.Message);
            return Result<Guid>.Failure(ApplicationError.RepositoryError);
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError(ex, "Lỗi hủy bỏ khi đăng ký làm tổ chức. Lỗi: {ErrorMessage}", ex.Message);
            return Result<Guid>.Failure(ApplicationError.Cancelled);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi đăng ký làm tổ chức. Lỗi: {ErrorMessage}", ex.Message);
            return Result<Guid>.Failure(ApplicationError.UnknownError);
        }
    }
}
