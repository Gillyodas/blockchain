using System;
using System.Collections.Generic;
using System.Text;
using ChainDegree.Domain.QuanLyToChuc.Enums;
using ChainDegree.Domain.TuyenDung.Enums;
using ChainDegree.SharedKernel.QuanLyToChuc;
using ChainDegree.SharedKernel.TuyenDung;
using ControlHub.Domain.SharedKernel;
using ControlHub.SharedKernel.Results;

namespace ChainDegree.Domain.QuanLyToChuc.ValueObjects;

public class GiayPhepNhaTuyenDung : ValueObject
{
    public string DuongDanLuuTru { get; private set; } = null!;
    public LoaiGiayPhepNTD LoaiGiayPhep { get; private set; }
    public DateTime ThoiGianTaiLen { get; private set; }
    public DateTime? ThoiGianDuocXacMinh { get; private set; }
    public Guid? XacMinhBoiAdminId { get; private set; }
    public TrangThaiXacMinh TrangThai { get; private set; }

    private GiayPhepNhaTuyenDung() { }

    private GiayPhepNhaTuyenDung(string duongDanLuuTru, LoaiGiayPhepNTD loaiGiayPhep, DateTime thoiGianTaiLen, DateTime? thoiGianDuocXacMinh, Guid? xacMinhBoiAdminId, TrangThaiXacMinh trangThaiXacMinh)
    {
        DuongDanLuuTru = duongDanLuuTru;
        LoaiGiayPhep = loaiGiayPhep;
        ThoiGianTaiLen = thoiGianTaiLen;
        ThoiGianDuocXacMinh = thoiGianDuocXacMinh;
        TrangThai = trangThaiXacMinh;
        XacMinhBoiAdminId = xacMinhBoiAdminId;
    }

    public static Result<GiayPhepNhaTuyenDung> Create(string duongDanLuuTru, LoaiGiayPhepNTD loaiGiayPhep)
    {
        if (string.IsNullOrWhiteSpace(duongDanLuuTru))
            return Result<GiayPhepNhaTuyenDung>.Failure(TuyenDungError.DuongDanGiayPhepTrong);

        return Result<GiayPhepNhaTuyenDung>.Success(
            new GiayPhepNhaTuyenDung(duongDanLuuTru, loaiGiayPhep, DateTime.UtcNow, null, null, TrangThaiXacMinh.ChoXacMinh));
    }

    public GiayPhepNhaTuyenDung DanhDauDaXacMinh(Guid adminId)
    {
        return new GiayPhepNhaTuyenDung(DuongDanLuuTru, LoaiGiayPhep, ThoiGianTaiLen, DateTime.UtcNow, adminId, TrangThaiXacMinh.DaXacMinh);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return DuongDanLuuTru;
        yield return LoaiGiayPhep;
        yield return ThoiGianTaiLen;
        yield return ThoiGianDuocXacMinh!;
        yield return XacMinhBoiAdminId!;
        yield return TrangThai;
    }
}
