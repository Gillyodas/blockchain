using System;
using System.Collections.Generic;
using ControlHub.Domain.SharedKernel;
using ChainDegree.Domain.QuanLyToChuc.Enums;

namespace ChainDegree.Domain.QuanLyToChuc.ValueObjects;

public class UyTinToChuc : ValueObject
{
    public int DiemUyTin { get; private set; }
    public int SoLuongXacMinhHopLe { get; private set; }
    public int SoLuongBangCapBiBaoCaoGianLan { get; private set; }
    public HangUyTin Hang { get; private set; }

    private UyTinToChuc(int diemUyTin)
    {
        DiemUyTin = diemUyTin;
        SoLuongXacMinhHopLe = 0;
        SoLuongBangCapBiBaoCaoGianLan = 0;
        CapNhatHangUyTin();
    }

    public static UyTinToChuc KhoiTaoBanDau()
    {
        return new UyTinToChuc(100);
    }

    public void CongDiemXacMinhHopLe()
    {
        SoLuongXacMinhHopLe++;
        DiemUyTin += 1;
        CapNhatHangUyTin();
    }

    public void TruDiemBangCapGianLan()
    {
        SoLuongBangCapBiBaoCaoGianLan++;
        DiemUyTin -= 200;
        CapNhatHangUyTin();
    }

    private void CapNhatHangUyTin()
    {
        if (DiemUyTin < 100) Hang = HangUyTin.Dong;
        else if (DiemUyTin < 300) Hang = HangUyTin.Bac;
        else if (DiemUyTin < 500) Hang = HangUyTin.Vang;
        else Hang = HangUyTin.DaCoGiayPhep;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return DiemUyTin;
        yield return SoLuongXacMinhHopLe;
        yield return SoLuongBangCapBiBaoCaoGianLan;
        yield return Hang;
    }
}
