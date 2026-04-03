using System;
using ChainDegree.Domain.TuyenDung.Aggregates;
using ControlHub.SharedKernel.Common.Errors;
using ControlHub.SharedKernel.Results;

namespace ChainDegree.Domain.TuyenDung.Entities;

public class ThongTinTuyenDungError
{
    public static readonly Error ViTriKhongDuocTrong = Error.Validation("TuyenDung.ViTriKhongDuocTrong", "Vị trí tuyển dụng không được để trống.");
    public static readonly Error MoTaKhongDuocTrong = Error.Validation("TuyenDung.MoTaKhongDuocTrong", "Mô tả không được để trống.");
    public static readonly Error HanUngTuyenKhongHopLe = Error.Validation("TuyenDung.HanUngTuyenKhongHopLe", "Hạn ứng tuyển không hợp lệ.");
}

public class ThongTinTuyenDung
{
    public Guid Id { get; private set; }
    public string Ten { get; private set; }
    public string MoTa { get; private set; }
    public Guid LinhVucId { get; private set; }
    public DateTime ThoiHanUngTuyen { get; private set; }
    public Guid NhaTuyenDungId { get; private set; }
    public DateTime ThoiGianTao { get; private set; }
    public DateTime ThoiGianCapNhat { get; private set; } = DateTime.MinValue;
    public DateTime ThoiGianXoa { get; private set; } = DateTime.MinValue;

    private ThongTinTuyenDung(Guid id, string ten, string moTa, Guid linhVucId, DateTime thoiHanUngTuyen, Guid nhaTuyenDungId,DateTime thoiGianTao)
    {
        Id = id;
        Ten = ten;
        MoTa = moTa;
        LinhVucId = linhVucId;
        ThoiHanUngTuyen = thoiHanUngTuyen;
        NhaTuyenDungId = nhaTuyenDungId;
        ThoiGianTao = thoiGianTao;
    }
    internal static Result<ThongTinTuyenDung> Create(
        string ten,
        string moTa,
        Guid linhVucId,
        DateTime thoiHanUngTuyen,
        Guid nhaTuyenDungId)
    {
        if (string.IsNullOrWhiteSpace(ten))
            return Result<ThongTinTuyenDung>.Failure(ThongTinTuyenDungError.ViTriKhongDuocTrong);

        if (string.IsNullOrWhiteSpace(moTa))
            return Result<ThongTinTuyenDung>.Failure(ThongTinTuyenDungError.MoTaKhongDuocTrong);

        if (thoiHanUngTuyen <= DateTime.UtcNow)
            return Result<ThongTinTuyenDung>.Failure(ThongTinTuyenDungError.HanUngTuyenKhongHopLe);

        var thongTinTuyenDung = new ThongTinTuyenDung(Guid.NewGuid(), ten, moTa, linhVucId, thoiHanUngTuyen, nhaTuyenDungId, DateTime.UtcNow);

        return Result<ThongTinTuyenDung>.Success(thongTinTuyenDung);
    }

    public Result CapNhatTTTD(string ten, string moTa, Guid linhVucId, DateTime thoiHanUngTuyen)
    {
        if (string.IsNullOrWhiteSpace(ten))
            return Result.Failure(ThongTinTuyenDungError.ViTriKhongDuocTrong);

        if (string.IsNullOrWhiteSpace(moTa))
            return Result.Failure(ThongTinTuyenDungError.MoTaKhongDuocTrong);

        if (thoiHanUngTuyen <= DateTime.UtcNow)
            return Result.Failure(ThongTinTuyenDungError.HanUngTuyenKhongHopLe);

        Ten = ten;
        MoTa = moTa;
        LinhVucId = linhVucId;
        ThoiHanUngTuyen = thoiHanUngTuyen;
        ThoiGianCapNhat = DateTime.UtcNow;

        return Result.Success();
    }

    public void XoaTTTD()
    {
        ThoiGianXoa = DateTime.UtcNow;
        ThoiGianCapNhat = DateTime.UtcNow;
    }
}
