using System;
using ChainDegree.Domain.TuyenDung.Enums;
using ChainDegree.Domain.TuyenDung.Errors;
using ControlHub.SharedKernel.Results;

namespace ChainDegree.Domain.TuyenDung.Entities;

public class ThongTinTuyenDung
{
    public Guid Id { get; private set; }
    public string Ten { get; private set; }
    public string MoTa { get; private set; }
    public Guid LinhVucId { get; private set; }
    public DateTime ThoiHanUngTuyen { get; private set; }
    public Guid NhaTuyenDungId { get; private set; }
    public TrangThaiThongTinTuyenDung TrangThai { get; private set; }
    public DateTime ThoiGianTao { get; private set; }
    public DateTime ThoiGianCapNhat { get; private set; } = DateTime.MinValue;
    public DateTime ThoiGianXoa { get; private set; } = DateTime.MinValue;

    private ThongTinTuyenDung(
        Guid id, 
        string ten, 
        string moTa, 
        Guid linhVucId, 
        DateTime thoiHanUngTuyen, 
        Guid nhaTuyenDungId,
        TrangThaiThongTinTuyenDung trangThai,
        DateTime thoiGianTao)
    {
        Id = id;
        Ten = ten;
        MoTa = moTa;
        LinhVucId = linhVucId;
        ThoiHanUngTuyen = thoiHanUngTuyen;
        NhaTuyenDungId = nhaTuyenDungId;
        TrangThai = trangThai;
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
            return Result<ThongTinTuyenDung>.Failure(TuyenDungError.ViTriKhongDuocTrong);

        if (string.IsNullOrWhiteSpace(moTa))
            return Result<ThongTinTuyenDung>.Failure(TuyenDungError.MoTaKhongDuocTrong);

        if (thoiHanUngTuyen <= DateTime.UtcNow)
            return Result<ThongTinTuyenDung>.Failure(TuyenDungError.HanUngTuyenKhongHopLe);

        return Result<ThongTinTuyenDung>.Success(new ThongTinTuyenDung(
            Guid.NewGuid(), 
            ten, 
            moTa, 
            linhVucId, 
            thoiHanUngTuyen, 
            nhaTuyenDungId, 
            TrangThaiThongTinTuyenDung.DangTuyen,
            DateTime.UtcNow));
    }

    public Result CapNhatTTTD(string ten, string moTa, Guid linhVucId, DateTime thoiHanUngTuyen, TrangThaiThongTinTuyenDung trangThai)
    {
        if (string.IsNullOrWhiteSpace(ten))
            return Result.Failure(TuyenDungError.ViTriKhongDuocTrong);

        if (string.IsNullOrWhiteSpace(moTa))
            return Result.Failure(TuyenDungError.MoTaKhongDuocTrong);

        if (thoiHanUngTuyen <= DateTime.UtcNow)
            return Result.Failure(TuyenDungError.HanUngTuyenKhongHopLe);

        Ten = ten;
        MoTa = moTa;
        LinhVucId = linhVucId;
        ThoiHanUngTuyen = thoiHanUngTuyen;
        TrangThai = trangThai;
        ThoiGianCapNhat = DateTime.UtcNow;

        return Result.Success();
    }

    public void XoaTTTD()
    {
        TrangThai = TrangThaiThongTinTuyenDung.DaDong;
        ThoiGianXoa = DateTime.UtcNow;
        ThoiGianCapNhat = DateTime.UtcNow;
    }
}
