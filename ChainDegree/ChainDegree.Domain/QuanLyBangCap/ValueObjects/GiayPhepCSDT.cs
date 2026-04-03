using System;
using System.Collections.Generic;
using System.Text;
using ChainDegree.Domain.Common.ValueObjects;
using ControlHub.Domain.SharedKernel;

namespace ChainDegree.Domain.QuanLyBangCap.ValueObjects;

public class GiayPhepCSDT : ValueObject
{
    public string DuongDanLuuTru { get; private set; }
    public DateTime ThoiGianTaiLen { get; private set; }
    public DateTime? ThoiGianDuocXacMinh { get; private set; }
    public DateTime? ThoiGianHetHan { get; private set; }
    public Guid? DuocXacMinhBoiAdminId { get; private set; }
    public ThongTinChuKySo? ThongTinChuKySoGiayPhepCSDT { get; private set; }

    private GiayPhepCSDT(string duongDanLuuTru, DateTime thoiGianTaiLen, DateTime thoiGianHetHan)
    {
        DuongDanLuuTru = duongDanLuuTru;
        ThoiGianTaiLen = thoiGianTaiLen;
        ThoiGianHetHan = thoiGianHetHan;
    }

    public static GiayPhepCSDT Create(string duongDanLuuTru, DateTime thoiGianTaiLen, DateTime thoiGianHetHan)
    {
        return new GiayPhepCSDT(duongDanLuuTru, thoiGianTaiLen, thoiGianHetHan);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }

    public override bool Equals(GiayPhepCSDT khac)
    {
        if (khac == null) throw new ArgumentNullException();

        if(this == khac) return true;
    }
}
