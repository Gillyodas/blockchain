using ChainDegree.Domain.BaoCaoGianLan.Enums;
using ChainDegree.Domain.BaoCaoGianLan.Events;
using ChainDegree.SharedKernel.BaoCaoGianLan;
using ControlHub.Domain.SharedKernel;
using ControlHub.SharedKernel.Results;

namespace ChainDegree.Domain.BaoCaoGianLan.Aggregates;

public class BaoCaoGianLan : AggregateRoot
{
    public Guid Id { get; private set; }
    public Guid BangCapId { get; private set; }
    public Guid NguoiBaoCaoId { get; private set; }
    public LoaiNguoiBaoCao LoaiNguoiBaoCao { get; private set; }
    public LyDoBaoCaoGianLan LyDo { get; private set; }
    public string? GhiChu { get; private set; }
    public TrangThaiBaoCao TrangThai { get; private set; }
    public DateTime ThoiGianBaoCao { get; private set; }
    public DateTime? ThoiGianCapNhat { get; private set; }

    private BaoCaoGianLan(Guid id, Guid bangCapId, Guid nguoiBaoCaoId,
        LoaiNguoiBaoCao loaiNguoiBaoCao, LyDoBaoCaoGianLan lyDo, string? ghiChu)
    {
        Id = id;
        BangCapId = bangCapId;
        NguoiBaoCaoId = nguoiBaoCaoId;
        LoaiNguoiBaoCao = loaiNguoiBaoCao;
        LyDo = lyDo;
        GhiChu = ghiChu;
        TrangThai = TrangThaiBaoCao.ChoXuLy;
        ThoiGianBaoCao = DateTime.UtcNow;
    }

    public static Result<BaoCaoGianLan> Create(
        Guid bangCapId,
        Guid nguoiBaoCaoId,
        LoaiNguoiBaoCao loaiNguoiBaoCao,
        LyDoBaoCaoGianLan lyDo,
        string? ghiChu)
    {
        if (bangCapId == Guid.Empty)
            return Result<BaoCaoGianLan>.Failure(BaoCaoGianLanError.BangCapIdTrong);

        if (nguoiBaoCaoId == Guid.Empty)
            return Result<BaoCaoGianLan>.Failure(BaoCaoGianLanError.NguoiBaoCaoTrong);

        if (lyDo == LyDoBaoCaoGianLan.Khac && string.IsNullOrWhiteSpace(ghiChu))
            return Result<BaoCaoGianLan>.Failure(BaoCaoGianLanError.LyDoKhacCanGhiChu);

        return Result<BaoCaoGianLan>.Success(
            new BaoCaoGianLan(Guid.NewGuid(), bangCapId, nguoiBaoCaoId, loaiNguoiBaoCao, lyDo, ghiChu));
    }

    public Result TiepNhan()
    {
        if (TrangThai != TrangThaiBaoCao.ChoXuLy)
            return Result.Failure(BaoCaoGianLanError.TrangThaiKhongHopLe);

        TrangThai = TrangThaiBaoCao.DangXuLy;
        ThoiGianCapNhat = DateTime.UtcNow;
        return Result.Success();
    }

    public Result XacNhanGianLan()
    {
        if (TrangThai != TrangThaiBaoCao.DangXuLy)
            return Result.Failure(BaoCaoGianLanError.TrangThaiKhongHopLe);

        TrangThai = TrangThaiBaoCao.DaXuLy;
        ThoiGianCapNhat = DateTime.UtcNow;
        RaiseDomainEvent(new GianLanXacNhanDomainEvent(Id, BangCapId));
        return Result.Success();
    }

    public Result TuChoiBaoCao()
    {
        if (TrangThai != TrangThaiBaoCao.DangXuLy)
            return Result.Failure(BaoCaoGianLanError.TrangThaiKhongHopLe);

        TrangThai = TrangThaiBaoCao.TuChoi;
        ThoiGianCapNhat = DateTime.UtcNow;
        return Result.Success();
    }
}
