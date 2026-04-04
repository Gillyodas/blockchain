using System;
using System.Collections.Generic;
using System.Text;
using ControlHub.SharedKernel.Results;
using ChainDegree.Domain.QuanLyToChuc.Enums;
using ChainDegree.SharedKernel.QuanLyToChuc;
using ControlHub.Domain.SharedKernel;
using ChainDegree.Domain.TuyenDung.ValueObjects;

namespace ChainDegree.Domain.QuanLyToChuc.ValueObjects;

public class GiayPhepCSDT : ValueObject
{
    public string DuongDanLuuTru { get; private set; }
    public LoaiGiayPhepCSDT LoaiGiayPhep { get; private set; }
    public TrangThaiXacMinh TrangThai { get; private set; } = TrangThaiXacMinh.ChoXacMinh;
    public DateTime ThoiGianTaiLen { get; private set; } = DateTime.Now;
    public DateTime? ThoiGianDuocXacMinh { get; private set; }
    public DateTime? ThoiGianHetHan { get; private set; }
    public Guid? DuocXacMinhBoiAdminId { get; private set; }
    public ThongTinChuKySo ThongTinChuKySoGiayPhepCSDT { get; private set; }
    private GiayPhepCSDT(string duongDanLuuTru, LoaiGiayPhepCSDT loai, DateTime thoiGianHetHan)
    {
        DuongDanLuuTru = duongDanLuuTru;
        LoaiGiayPhep = loai;
        ThoiGianHetHan = thoiGianHetHan;
    }
    internal static Result<GiayPhepCSDT> Create(string duongDanLuuTru, LoaiGiayPhepCSDT loai, DateTime thoiGianHetHan)
    {
        if(thoiGianHetHan <= DateTime.Now)
        {
            return Result<GiayPhepCSDT>.Failure(GiayPhepCSDTError.GiayPhepCSDTHetHan);
        }
        var giayPhep = new GiayPhepCSDT(duongDanLuuTru, loai, TrangThaiXacMinh.ChoXacMinh, thoiGianHetHan);
        return Result<GiayPhepCSDT>.Success(giayPhep);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return DuongDanLuuTru;
        yield return LoaiGiayPhep;
        yield return TrangThai;
    }
}
