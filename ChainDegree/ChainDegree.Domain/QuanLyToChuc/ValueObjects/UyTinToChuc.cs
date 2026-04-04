using System;
using System.Collections.Generic;
using ControlHub.Domain.SharedKernel;
using ChainDegree.Domain.QuanLyToChuc.Enums;
using ControlHub.SharedKernel.Results;

namespace ChainDegree.Domain.QuanLyToChuc.ValueObjects;

public class UyTinToChuc : ValueObject
{
    public int DiemUyTin { get; private set; }
    public int SoLuongXacMinhHopLe { get; private set; } = 0;
    public int SoLuongBangCapBiBaoCaoGianLan { get; private set; } = 0;
    public int SoLuongBangCapThuHoi { get; private set; } = 0;
    public int SoLuongBangCapPhatHanh { get; private set; } = 0;
    public HangUyTin Hang { get; private set; }

    private const int DIEM_SO_VOI_MOI_GIAY_PHEP = 50;

    private UyTinToChuc(
    int diemUyTin,
    int soLuongXacMinhHopLe,
    int soLuongBangCapBiBaoCaoGianLan,
    int soLuongBangCapThuHoi,
    int soLuongBangCapPhatHanh)
    {
        DiemUyTin = diemUyTin;
        SoLuongXacMinhHopLe = soLuongXacMinhHopLe;
        SoLuongBangCapBiBaoCaoGianLan = soLuongBangCapBiBaoCaoGianLan;
        SoLuongBangCapThuHoi = soLuongBangCapThuHoi;
        SoLuongBangCapPhatHanh = soLuongBangCapPhatHanh;
        CapNhatHangUyTin();
    }

    public static UyTinToChuc KhoiTao(int soLuongGiayPhep)
    {
        return new UyTinToChuc(soLuongGiayPhep * DIEM_SO_VOI_MOI_GIAY_PHEP, 0, 0, 0, 0);
    }

    public UyTinToChuc ThemGiayPhep()
    {
        return new UyTinToChuc(
            DiemUyTin + DIEM_SO_VOI_MOI_GIAY_PHEP,
            SoLuongXacMinhHopLe,
            SoLuongBangCapBiBaoCaoGianLan,
            SoLuongBangCapThuHoi,
            SoLuongBangCapPhatHanh);
    }

    public UyTinToChuc CongDiemXacMinhHopLe()
    {
        return new UyTinToChuc(
            DiemUyTin + 1,
            SoLuongXacMinhHopLe + 1,
            SoLuongBangCapBiBaoCaoGianLan,
            SoLuongBangCapThuHoi,
            SoLuongBangCapPhatHanh);
    }

    public UyTinToChuc TruDiemBangCapGianLan()
    {
        return new UyTinToChuc(
            DiemUyTin - 200,
            SoLuongXacMinhHopLe,
            SoLuongBangCapBiBaoCaoGianLan + 1,
            SoLuongBangCapThuHoi,
            SoLuongBangCapPhatHanh);
    }

    public UyTinToChuc CongDiemCapBangThanhCong()
    {
        return new UyTinToChuc(
            DiemUyTin + 2,
            SoLuongXacMinhHopLe,
            SoLuongBangCapBiBaoCaoGianLan,
            SoLuongBangCapThuHoi,
            SoLuongBangCapPhatHanh + 1);
    }

    public UyTinToChuc TruDiemHuyBangLoiNhapLieu()
    {
        return new UyTinToChuc(
            DiemUyTin - 5,
            SoLuongXacMinhHopLe,
            SoLuongBangCapBiBaoCaoGianLan,
            SoLuongBangCapThuHoi,
            SoLuongBangCapPhatHanh);
    }

    public UyTinToChuc TruDiemThuHoiBang()
    {
        return new UyTinToChuc(
            DiemUyTin - 5,
            SoLuongXacMinhHopLe,
            SoLuongBangCapBiBaoCaoGianLan,
            SoLuongBangCapThuHoi + 1,
            SoLuongBangCapPhatHanh);
    }

    public UyTinToChuc TruDiemThuHoiGianLan()
    {
        return new UyTinToChuc(
            DiemUyTin - 200,
            SoLuongXacMinhHopLe,
            SoLuongBangCapBiBaoCaoGianLan,
            SoLuongBangCapThuHoi + 1,
            SoLuongBangCapPhatHanh);
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
