using System;
using System.Collections.Generic;
using System.Text;
using ChainDegree.Application.Common.Persistence;
using ChainDegree.Application.QuanLyToChuc.Interfaces.Repositories;
using ChainDegree.Domain.QuanLyToChuc.Enums;
using ChainDegree.Domain.TuyenDung.Aggregates;
using ChainDegree.SharedKernel.Common.Errors;
using ChainDegree.SharedKernel.QuanLyToChuc;
using ControlHub.SharedKernel.Common.Exceptions;
using ControlHub.SharedKernel.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChainDegree.Application.QuanLyToChuc.Commands.DuyetDangKyNTD;

public class DuyetDangKyNTDCommandHandler : IRequestHandler<DuyetDangKyNTDCommand, Result>
{
    private readonly ILogger<DuyetDangKyNTDCommandHandler> _logger;
    private readonly IYeuCauDangKyRepository _yeuCauDangKyRepository;
    private readonly INhaTuyenDungRepository _nhaTuyenDungRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DuyetDangKyNTDCommandHandler(
        ILogger<DuyetDangKyNTDCommandHandler> logger,
        IYeuCauDangKyRepository yeuCauDangKyRepository,
        INhaTuyenDungRepository nhaTuyenDungRepository,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _yeuCauDangKyRepository = yeuCauDangKyRepository;
        _nhaTuyenDungRepository = nhaTuyenDungRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DuyetDangKyNTDCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Bắt đầu duyệt đăng ký NTD ID: {Id}", request.YeuCauDangKyId);

            _logger.LogDebug("Lấy yêu cầu đăng ký từ repository với ID: {Id}", request.YeuCauDangKyId);
            var yeuCauDangKy = await _yeuCauDangKyRepository.GetByIdAsync(request.YeuCauDangKyId, cancellationToken);
            if (yeuCauDangKy == null || yeuCauDangKy.Loai != LoaiToChuc.Verifier)
            {
                _logger.LogWarning("Không tìm thấy yêu cầu đăng ký NTD hợp lệ với ID: {Id}", request.YeuCauDangKyId);
                return Result.Failure(QuanLyToChucError.KhongTimThayYeuCauDangKy);
            }

            _logger.LogDebug("Duyệt yêu cầu đăng ký NTD ID: {Id} với ghi chú: {GhiChu}", request.YeuCauDangKyId, request.GhiChu);
            var approveResult = yeuCauDangKy.AdminDuyet(request.GhiChu);
            if (approveResult.IsFailure)
            {
                _logger.LogWarning("Duyệt yêu cầu đăng ký NTD ID: {Id} thất bại với lỗi: {Error}", request.YeuCauDangKyId, approveResult.Error);
                return Result.Failure(approveResult.Error);
            }

            _logger.LogDebug("Cập nhật trạng thái yêu cầu đăng ký NTD ID: {Id} trong repository", request.YeuCauDangKyId);
            var ntdResult = NhaTuyenDung.Create(
                yeuCauDangKy.TenToChuc,
                yeuCauDangKy.DiaChiVi,
                yeuCauDangKy.TaiKhoanId,
                yeuCauDangKy.Id,
                yeuCauDangKy.GiayPhepNTDs.ToList());

            if (ntdResult.IsFailure)
            {
                _logger.LogWarning("Tạo NTD từ yêu cầu đăng ký ID: {Id} thất bại với lỗi: {Error}", request.YeuCauDangKyId, ntdResult.Error);
                return Result.Failure(ntdResult.Error);
            }
            
            _logger.LogDebug("Lưu NTD mới vào repository");
            await _nhaTuyenDungRepository.AddAsync(ntdResult.Value, cancellationToken);

            _logger.LogDebug("Lưu thay đổi vào database");
            await _unitOfWork.CommitAsync(cancellationToken);

            return Result.Success();
        }
        catch (RepositoryConcurrencyException ex)
        {
            _logger.LogError(ex, "Lỗi xung đột dữ liệu khi duyệt đăng ký NTD. Lỗi: {ErrorMessage}", ex.Message);
            return Result<Guid>.Failure(ApplicationError.ConcurrencyError);
        }
        catch (RepositoryException ex)
        {
            _logger.LogError(ex, "Lỗi truy cập dữ liệu khi duyệt đăng ký NTD. Lỗi: {ErrorMessage}", ex.Message);
            return Result<Guid>.Failure(ApplicationError.RepositoryError);
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError(ex, "Lỗi hủy bỏ khi duyệt đăng ký NTD. Lỗi: {ErrorMessage}", ex.Message);
            return Result<Guid>.Failure(ApplicationError.Cancelled);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi duyệt đăng ký NTD. Lỗi: {ErrorMessage}", ex.Message);
            return Result<Guid>.Failure(ApplicationError.UnknownError);
        }
    }
}
