using System;
using System.Collections.Generic;
using ControlHub.SharedKernel.Results;
using ChainDegree.Domain.QuanLyToChuc.Enums;
using ChainDegree.SharedKernel.QuanLyToChuc;
using ControlHub.Domain.SharedKernel;

namespace ChainDegree.Domain.QuanLyToChuc.ValueObjects;

public class GiayPhepCSDT : ValueObject
{
    public string DuongDanLuuTru { get; private set; } = null!;
    public LoaiGiayPhepCSDT LoaiGiayPhep { get; private set; }
    public TrangThaiXacMinh TrangThai { get; private set; } = TrangThaiXacMinh.ChoXacMinh;
    public DateTime ThoiGianTaiLen { get; private set; } = DateTime.UtcNow;
    public DateTime? ThoiGianDuocXacMinh { get; private set; }
    public DateTime? ThoiGianHetHan { get; private set; }
    public Guid? DuocXacMinhBoiAdminId { get; private set; }
    public ThongTinChuKySo? ThongTinChuKySoGiayPhepCSDT { get; private set; }

    private GiayPhepCSDT() { }
    private GiayPhepCSDT(string duongDanLuuTru, LoaiGiayPhepCSDT loai, DateTime thoiGianHetHan)
    {
        DuongDanLuuTru = duongDanLuuTru;
        LoaiGiayPhep = loai;
        ThoiGianHetHan = thoiGianHetHan;
    }

    private GiayPhepCSDT(string duongDanLuuTru, LoaiGiayPhepCSDT loai, TrangThaiXacMinh trangThai, DateTime? thoiGianHetHan)
    {
        DuongDanLuuTru = duongDanLuuTru;
        LoaiGiayPhep = loai;
        TrangThai = trangThai;
        ThoiGianHetHan = thoiGianHetHan;
    }

    internal static Result<GiayPhepCSDT> Create(string duongDanLuuTru, LoaiGiayPhepCSDT loai, DateTime thoiGianHetHan)
    {
        if (string.IsNullOrWhiteSpace(duongDanLuuTru))
            return Result<GiayPhepCSDT>.Failure(QuanLyToChucError.DuongDanFileTrong);

        if (thoiGianHetHan <= DateTime.UtcNow)
            return Result<GiayPhepCSDT>.Failure(GiayPhepCSDTError.GiayPhepCSDTHetHan);

        var giayPhep = new GiayPhepCSDT(duongDanLuuTru, loai, thoiGianHetHan);
        return Result<GiayPhepCSDT>.Success(giayPhep);
    }

    internal Result<GiayPhepCSDT> TaiLenLai(string duongDanMoi)
    {
        if (TrangThai != TrangThaiXacMinh.TuChoi)
            return Result<GiayPhepCSDT>.Failure(GiayPhepCSDTError.ChiDuocTaiLenLaiKhiBiTuChoi);

        if (string.IsNullOrWhiteSpace(duongDanMoi))
            return Result<GiayPhepCSDT>.Failure(QuanLyToChucError.DuongDanFileTrong);

        var giayPhepMoi = new GiayPhepCSDT(duongDanMoi, LoaiGiayPhep, TrangThaiXacMinh.TaiLenLai, ThoiGianHetHan);
        return Result<GiayPhepCSDT>.Success(giayPhepMoi);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return DuongDanLuuTru;
        yield return LoaiGiayPhep;
        yield return TrangThai;
    }
}
