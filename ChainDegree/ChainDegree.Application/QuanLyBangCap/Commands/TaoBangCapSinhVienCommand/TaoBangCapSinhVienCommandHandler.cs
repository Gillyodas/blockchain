using ChainDegree.Application.Common.Persistence;
using ChainDegree.Application.QuanLyBangCap.Interfaces.Repositories;
using ChainDegree.Domain.QuanLyBangCap.Aggregates;
using ChainDegree.Domain.QuanLyBangCap.Entities;
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
    private readonly IBangCapRepository _bangCapRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TaoBangCapSinhVienCommandHandler(IBangCapRepository bangCapRepository, ISinhVienRepository sinhVienRepository, ICoSoDaoTaoRepository coSoDaoTaoRepository, IUnitOfWork unitOfWork, ILogger<TaoBangCapSinhVienCommandHandler> logger)
    {
        _logger = logger;
        _coSoDaoTaoRepository = coSoDaoTaoRepository;
        _sinhVienRepository = sinhVienRepository;
        _bangCapRepository = bangCapRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<TaoBangCapSinhVienCommandResult>> Handle(TaoBangCapSinhVienCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Bắt đầu xử lý tạo bằng cấp cho sinh viên với ID: {SinhVienId}", request.SinhVienId);

        _logger.LogInformation("Tìm kiếm cơ sở đào tạo với ID: {CoSoDaoTaoId}", request.CoSoDaoTaoId);
        CoSoDaoTao? csdt =  await _coSoDaoTaoRepository.GetByIdAsync(request.CoSoDaoTaoId, cancellationToken);
        if(csdt == null)
        {
            _logger.LogWarning("Không tìm thấy cơ sở đào tạo khi tạo bằng cấp cho sinh viên với CSDT ID: {CoSoDaoTaoId}", request.CoSoDaoTaoId);
            return Result<TaoBangCapSinhVienCommandResult>.Failure(CoSoDaoTaoError.KhongTimThayCoSoDaoTao);
        }

        _logger.LogInformation("Tìm kiếm sinh viên với ID: {SinhVienId} hoặc CCCD: {CCCD}", request.SinhVienId, request.CCCD);
        SinhVien? sinhVien;
        if (request.SinhVienId == Guid.Empty)
        {
            sinhVien = await _sinhVienRepository.GetByCCCDAsync(request.CCCD!, cancellationToken);
        }
        else
        {
            sinhVien = await _sinhVienRepository.GetByIdAsync(request.SinhVienId!.Value, cancellationToken);
        }
        if(sinhVien == null)
        {
            _logger.LogWarning("Không tìm thấy sinh viên khi tạo bằng cấp cho sinh viên với ID hoặc CCCD: {SinhVienId}, {CCCD}", request.SinhVienId, request.CCCD);
            return Result<TaoBangCapSinhVienCommandResult>.Failure(SinhVienError.KhongTimThaySinhVien);
        }

        _logger.LogInformation("Kiểm tra trùng lặp khi cấp bằng cho sinh viên với ID: {SinhVienId}, Loại bằng cấp: {LoaiBangCap}, Lĩnh vực ID: {LinhVucId}", sinhVien.Id, request.LoaiBangCap, request.LinhVucId);
        bool trungLapKhiCapBangChoSinhVien = await _coSoDaoTaoRepository.TrungLapKhiCapBangChoSinhVien(csdt.Id, sinhVien.Id, request.LoaiBangCap, request.LinhVucId, cancellationToken);
        if(trungLapKhiCapBangChoSinhVien)
        {
            _logger.LogWarning("Trùng lặp khi cấp bằng cho cho sinh viên với ID: {SinhVienId}. Lỗi: {Error}", sinhVien.Id, CoSoDaoTaoError.SinhVienDaCoBangCapCungLoaiVaLinhVuc);
            return Result<TaoBangCapSinhVienCommandResult>.Failure(CoSoDaoTaoError.SinhVienDaCoBangCapCungLoaiVaLinhVuc);
        }

        _logger.LogInformation("Tạo bằng cấp mới cho sinh viên với ID: {SinhVienId}", sinhVien.Id);
        Result<BangCap> bangCapMoi = csdt.TaoBangCapChoSinhVien(request.TenBangCap, request.Diem, request.LoaiBangCap, request.LinhVucId, request.FileBangCap, request.LinkBangCap, request.NgayCap, request.NgayHetHan, sinhVien.Id);
        if(bangCapMoi.IsFailure)
        {
            _logger.LogWarning("Thất bại khi tạo bằng cấp cho sinh viên với ID: {SinhVienId}. Lỗi: {Error}", sinhVien.Id, bangCapMoi.Error);
            return Result<TaoBangCapSinhVienCommandResult>.Failure(bangCapMoi.Error);
        }

        _logger.LogInformation("Lưu bằng cấp mới vào repository cho sinh viên với ID: {SinhVienId}. Bằng cấp ID: {BangCapId}", sinhVien.Id, bangCapMoi.Value.Id);
        await _bangCapRepository.AddAsync(bangCapMoi.Value, cancellationToken);

        _logger.LogInformation("Cam kết giao dịch để tạo bằng cấp cho sinh viên với ID: {SinhVienId}", sinhVien.Id);
        await _unitOfWork.CommitAsync(cancellationToken);

        TaoBangCapSinhVienCommandResult commandResult = new TaoBangCapSinhVienCommandResult(
            bangCapMoi.Value.Id,
            bangCapMoi.Value.TrangThaiBangCapHienTai,
            bangCapMoi.Value.TrangThaiBlockchainHienTai,
            bangCapMoi.Value.MaBamXacThuc ?? string.Empty
        );

        _logger.LogInformation("Tạo bằng cấp thành công cho sinh viên với ID: {SinhVienId}", sinhVien.Id);

        return Result<TaoBangCapSinhVienCommandResult>.Success(commandResult);
    }
}
