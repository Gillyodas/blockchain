using System;
using ChainDegree.Domain.TuyenDung.Aggregates;
using ControlHub.SharedKernel.Common.Errors;
using ControlHub.SharedKernel.Results;

namespace ChainDegree.Domain.TuyenDung.ValueObjects;

public class GiayPhepError
{
    public static readonly Error DuongDanTrong = Error.Validation("GiayPhep.DuongDanTrong", "Đường dẫn lưu trữ giấy phép không được để trống.");
}

public class GiayPhepNhaTuyenDung
{
    public string DuongDanLuuTru { get; private set; }
    public KieuGiayPhepNTD KieuGiayPhep { get; private set; }
    public DateTime ThoiGianTaiLen { get; private set; }
    public DateTime? ThoiGianDuocXacMinh { get; private set; }
    public Guid? XacMinhBoiAdminId { get; private set; }

    private GiayPhepNhaTuyenDung(string duongDanLuuTru, KieuGiayPhepNTD kieuGiayPhep, DateTime thoiGianTaiLen)
    {
        DuongDanLuuTru = duongDanLuuTru;
        KieuGiayPhep = kieuGiayPhep;
        ThoiGianTaiLen = thoiGianTaiLen;
    }

    public static Result<GiayPhepNhaTuyenDung> Create(string duongDanLuuTru, KieuGiayPhepNTD kieuGiayPhep)
    {
        if (string.IsNullOrWhiteSpace(duongDanLuuTru))
            return Result<GiayPhepNhaTuyenDung>.Failure(GiayPhepError.DuongDanTrong);

        return Result<GiayPhepNhaTuyenDung>.Success(new GiayPhepNhaTuyenDung(duongDanLuuTru, kieuGiayPhep, DateTime.UtcNow));
    }
}
