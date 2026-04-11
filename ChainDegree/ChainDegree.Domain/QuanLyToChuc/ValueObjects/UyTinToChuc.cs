using System;
using System.Collections.Generic;
using ControlHub.Domain.SharedKernel;
using ChainDegree.Domain.QuanLyToChuc.Enums;
using ControlHub.SharedKernel.Results;

namespace ChainDegree.Domain.QuanLyToChuc.ValueObjects;

public class UyTinToChuc : ValueObject
{
    public int DiemUyTin { get; private set; }
    public int SoLuongGiayPhep { get; private set; } = 0;
    public int SoLuongXacMinhHopLe { get; private set; } = 0;
    public int SoLuongBangCapBiBaoCaoGianLan { get; private set; } = 0;
    public int SoLuongBangCapThuHoi { get; private set; } = 0;
    public int SoLuongBangCapPhatHanh { get; private set; } = 0;
    public HangUyTin Hang { get; private set; }

    private const int DIEM_SO_VOI_MOI_GIAY_PHEP = 50;

    private UyTinToChuc() { }

    private UyTinToChuc(
    int diemUyTin,
    int soLuongGiayPhep,
    int soLuongXacMinhHopLe,
    int soLuongBangCapBiBaoCaoGianLan,
    int soLuongBangCapThuHoi,
    int soLuongBangCapPhatHanh)
    {
        DiemUyTin = diemUyTin;
        SoLuongGiayPhep = soLuongGiayPhep;
        SoLuongXacMinhHopLe = soLuongXacMinhHopLe;
        SoLuongBangCapBiBaoCaoGianLan = soLuongBangCapBiBaoCaoGianLan;
        SoLuongBangCapThuHoi = soLuongBangCapThuHoi;
        SoLuongBangCapPhatHanh = soLuongBangCapPhatHanh;
        CapNhatHangUyTin();
    }

    public static UyTinToChuc KhoiTao(int soLuongGiayPhep)
    {
        return new UyTinToChuc(soLuongGiayPhep * DIEM_SO_VOI_MOI_GIAY_PHEP, soLuongGiayPhep, 0, 0, 0, 0);
    }

    public UyTinToChuc ThemGiayPhep()
    {
        return new UyTinToChuc(
            DiemUyTin + DIEM_SO_VOI_MOI_GIAY_PHEP,
            SoLuongGiayPhep + 1,
            SoLuongXacMinhHopLe,
            SoLuongBangCapBiBaoCaoGianLan,
            SoLuongBangCapThuHoi,
            SoLuongBangCapPhatHanh);
    }

    public UyTinToChuc CongDiemXacMinhHopLe()
    {
        return new UyTinToChuc(
            DiemUyTin + 1,
            SoLuongGiayPhep,
            SoLuongXacMinhHopLe + 1,
            SoLuongBangCapBiBaoCaoGianLan,
            SoLuongBangCapThuHoi,
            SoLuongBangCapPhatHanh);
    }

    public UyTinToChuc TruDiemBangCapGianLan()
    {
        return new UyTinToChuc(
            DiemUyTin - 200,
            SoLuongGiayPhep,
            SoLuongXacMinhHopLe,
            SoLuongBangCapBiBaoCaoGianLan + 1,
            SoLuongBangCapThuHoi,
            SoLuongBangCapPhatHanh);
    }

    public UyTinToChuc CongDiemCapBangThanhCong()
    {
        return new UyTinToChuc(
            DiemUyTin + 2,
            SoLuongGiayPhep,
            SoLuongXacMinhHopLe,
            SoLuongBangCapBiBaoCaoGianLan,
            SoLuongBangCapThuHoi,
            SoLuongBangCapPhatHanh + 1);
    }

    public UyTinToChuc TruDiemHuyBangLoiNhapLieu()
    {
        return new UyTinToChuc(
            DiemUyTin - 5,
            SoLuongGiayPhep,
            SoLuongXacMinhHopLe,
            SoLuongBangCapBiBaoCaoGianLan,
            SoLuongBangCapThuHoi,
            SoLuongBangCapPhatHanh);
    }

    public UyTinToChuc TruDiemThuHoiBang()
    {
        return new UyTinToChuc(
            DiemUyTin - 5,
            SoLuongGiayPhep,
            SoLuongXacMinhHopLe,
            SoLuongBangCapBiBaoCaoGianLan,
            SoLuongBangCapThuHoi + 1,
            SoLuongBangCapPhatHanh);
    }

    public UyTinToChuc TruDiemThuHoiGianLan()
    {
        return new UyTinToChuc(
            DiemUyTin - 200,
            SoLuongGiayPhep,
            SoLuongXacMinhHopLe,
            SoLuongBangCapBiBaoCaoGianLan,
            SoLuongBangCapThuHoi + 1,
            SoLuongBangCapPhatHanh);
    }

    private void CapNhatHangUyTin()
    {
        if (SoLuongGiayPhep == 0) Hang = HangUyTin.ChuaCoGiayPhep;
        else if (DiemUyTin < 100) Hang = HangUyTin.Dong;
        else if (DiemUyTin < 300) Hang = HangUyTin.Bac;
        else if (DiemUyTin < 500) Hang = HangUyTin.Vang;
        else Hang = HangUyTin.DaCoGiayPhep;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return DiemUyTin;
        yield return SoLuongGiayPhep;
        yield return SoLuongXacMinhHopLe;
        yield return SoLuongBangCapBiBaoCaoGianLan;
        yield return SoLuongBangCapThuHoi;
        yield return SoLuongBangCapPhatHanh;
        yield return Hang;
    }
}
