using System;
using System.Collections.Generic;
using ChainDegree.Domain.TuyenDung.Enums;
using ChainDegree.SharedKernel.TuyenDung;
using ControlHub.Domain.SharedKernel;
using ControlHub.SharedKernel.Results;

namespace ChainDegree.Domain.TuyenDung.ValueObjects;

public class GiayPhepNhaTuyenDung : ValueObject
{
    public string DuongDanLuuTru { get; private set; }
    public KieuGiayPhepNTD KieuGiayPhep { get; private set; }
    public DateTime ThoiGianTaiLen { get; private set; }
    public DateTime? ThoiGianDuocXacMinh { get; private set; }
    public Guid? XacMinhBoiAdminId { get; private set; }

    private GiayPhepNhaTuyenDung() { }

    private GiayPhepNhaTuyenDung(string duongDanLuuTru, KieuGiayPhepNTD kieuGiayPhep, DateTime thoiGianTaiLen, DateTime? thoiGianDuocXacMinh, Guid? xacMinhBoiAdminId)
    {
        DuongDanLuuTru = duongDanLuuTru;
        KieuGiayPhep = kieuGiayPhep;
        ThoiGianTaiLen = thoiGianTaiLen;
        ThoiGianDuocXacMinh = thoiGianDuocXacMinh;
        XacMinhBoiAdminId = xacMinhBoiAdminId;
    }

    public static Result<GiayPhepNhaTuyenDung> Create(string duongDanLuuTru, KieuGiayPhepNTD kieuGiayPhep)
    {
        if (string.IsNullOrWhiteSpace(duongDanLuuTru))
            return Result<GiayPhepNhaTuyenDung>.Failure(TuyenDungError.DuongDanGiayPhepTrong);

        return Result<GiayPhepNhaTuyenDung>.Success(
            new GiayPhepNhaTuyenDung(duongDanLuuTru, kieuGiayPhep, DateTime.UtcNow, null, null));
    }

    public GiayPhepNhaTuyenDung DanhDauDaXacMinh(Guid adminId)
    {
        return new GiayPhepNhaTuyenDung(DuongDanLuuTru, KieuGiayPhep, ThoiGianTaiLen, DateTime.UtcNow, adminId);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return DuongDanLuuTru;
        yield return KieuGiayPhep;
        yield return ThoiGianTaiLen;
    }
}
