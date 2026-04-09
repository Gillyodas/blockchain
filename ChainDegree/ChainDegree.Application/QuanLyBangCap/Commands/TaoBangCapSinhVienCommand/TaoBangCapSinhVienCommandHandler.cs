using ChainDegree.Application.Common.Persistence;
using ChainDegree.Application.QuanLyBangCap.Interfaces.Repositories;
using ChainDegree.Domain.QuanLyBangCap.Aggregates;
using ChainDegree.Domain.QuanLyBangCap.Entities;
using ChainDegree.SharedKernel.QuanLyBangCap.BangCap;
using ChainDegree.SharedKernel.QuanLyBangCap.CoSoDaoTao;
using ChainDegree.SharedKernel.QuanLyBangCap.SinhVien;
using ControlHub.SharedKernel.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChainDegree.Application.QuanLyBangCap.Commands.TaoBangCapChoSinhVienCommand;

public class TaoBangCapSinhVienCommandHandler : IRequestHandler<TaoBangCapSinhVienCommand, Result<TaoBangCapSinhVienCommandResult>>
{
    private readonly ILogger<TaoBangCapSinhVienCommandHandler> _logger;
    private readonly ICoSoDaoTaoRepository _coSoDaoTaoRepository;
    private readonly ISinhVienRepository _sinhVienRepository;

    public TaoBangCapSinhVienCommandHandler(IBangCapRepository bangCapRepository, ISinhVienRepository sinhVienRepository, ICoSoDaoTaoRepository coSoDaoTaoRepository, IUnitOfWork unitOfWork, ILogger<TaoBangCapSinhVienCommandHandler> logger)
    {
        _logger = logger;
        _coSoDaoTaoRepository = coSoDaoTaoRepository;
        _sinhVienRepository = sinhVienRepository;
    }

    public async Task<Result<TaoBangCapSinhVienCommandResult>> Handle(TaoBangCapSinhVienCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Bắt đầu xử lý tạo bằng cấp cho sinh viên với ID: {SinhVienId}", request.SinhVienId);

        Result<CoSoDaoTao> getCSDTResult =  await _coSoDaoTaoRepository.GetByIdAsync(request.CoSoDaoTaoId, cancellationToken);

        if(getCSDTResult.IsFailure)
        {
            _logger.LogWarning("Không tìm thấy cơ sở đào tạo khi tạo bằng cấp cho sinh viên với Cơ sở đào tạo ID: {CoSoDaoTaoId}", request.CoSoDaoTaoId);
            return Result<TaoBangCapSinhVienCommandResult>.Failure(CoSoDaoTaoError.KhongTimThayCoSoDaoTao);
        }

        Result<SinhVien> getSinhVienResult;
        if (request.SinhVienId == Guid.Empty)
        {
            getSinhVienResult = await _sinhVienRepository.GetByCCCDAsync(request.CCCD!, cancellationToken);
        }
        else
        {
            getSinhVienResult = await _sinhVienRepository.GetByIdAsync(request.SinhVienId!.Value, cancellationToken);
        }

        if (getSinhVienResult.IsFailure)
        {
            _logger.LogWarning("Không tìm thấy sinh viên khi tạo bằng cấp cho sinh viên với ID hoặc CCCD: {SinhVienId}, {CCCD}", request.SinhVienId, request.CCCD);
            return Result<TaoBangCapSinhVienCommandResult>.Failure(SinhVienError.KhongTimThaySinhVien);
        }

        Result<bool> 

        return Result<TaoBangCapSinhVienCommandResult>.Success();
    }
}
