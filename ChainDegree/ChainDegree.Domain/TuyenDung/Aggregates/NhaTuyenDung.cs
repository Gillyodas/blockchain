using System;
using System.Collections.Generic;
using System.Linq;
using ChainDegree.Domain.QuanLyToChuc.Enums;
using ChainDegree.Domain.TuyenDung.Entities;
using ChainDegree.Domain.TuyenDung.ValueObjects;
using ControlHub.SharedKernel.Common.Errors;
using ControlHub.SharedKernel.Results;

namespace ChainDegree.Domain.TuyenDung.Aggregates;

public class NhaTuyenDung
{
    private readonly List<ThongTinTuyenDung> _thongTinTuyenDungs = new();
    private readonly List<GiayPhepNhaTuyenDung> _giayPheps = new();

    public Guid Id { get; private set; }
    public string Ten { get; private set; }
    public string DiaChi { get; private set; }
    public string DiaChiViNhaTuyenDung { get; private set; }
    public Guid TaiKhoanId { get; private set; }
    public Guid YeuCauDangKyId { get; private set; }
    public DateTime ThoiGianTao { get; private set; }
    public DateTime ThoiGianCapNhat { get; private set; } = DateTime.MinValue;
    public DateTime ThoiGianXoa { get; private set; } = DateTime.MinValue;
    public HangUyTin HangUyTin { get; private set; }

    private NhaTuyenDung(Guid id, string ten, string diaChi, string diaChiViNhaTuyenDung, Guid taiKhoanId, Guid yeuCauDangKyId DateTime thoiGianTao,
    {
        Id = id; 
        Ten = ten; 
        DiaChi = diaChi; 
        DiaChiViNhaTuyenDung = diaChiViNhaTuyenDung; 
        TaiKhoanId = taiKhoanId; 
        YeuCauDangKyId = yeuCauDangKyId;
        ThoiGianTao = thoiGianTao;
    }

    internal static Result<NhaTuyenDung> Create(Guid id, string ten, string diaChi, string diaChiViNhaTuyenDung, Guid taiKhoanId, Guid yeuCauDangKyId)
    {
        if (string.IsNullOrWhiteSpace(ten))
            return Result<NhaTuyenDung>.Failure(TuyenDungError.TenNhaTuyenDungTrong);

        return Result<NhaTuyenDung>.Success(new NhaTuyenDung(id, ten, diaChi, diaChiViNhaTuyenDung, taiKhoanId, yeuCauDangKyId,DateTime.UtcNow,                 ));
    }

    public Result CapNhatThongTinNhaTuyenDung(string ten, string diaChi)
    {
        if (string.IsNullOrWhiteSpace(ten))
            return Result.Failure(TuyenDungError.TenNhaTuyenDungTrong);

        Ten = ten;
        DiaChi = diaChi;
        ThoiGianCapNhat = DateTime.UtcNow;
        return Result.Success();
    }

    public Result<ThongTinTuyenDung> TaoTTTD(string ten, string moTa, Guid linhVucId, DateTime thoiHanUngTuyen)
    {
        var result = ThongTinTuyenDung.Create(ten, moTa, linhVucId, thoiHanUngTuyen, Id);
        if (result.IsSuccess)
        {
            _thongTinTuyenDungs.Add(result.Value);
            ThoiGianCapNhat = DateTime.UtcNow;
        }
        return result;
    }

    public Result CapNhatTTTD(Guid tttdId, string ten, string moTa, Guid linhVucId, DateTime thoiHanUngTuyen)
    {
        var tttd = _thongTinTuyenDungs.FirstOrDefault(x => x.Id == tttdId);
        if (tttd == null)
            return Result.Failure(TuyenDungError.TinTuyenDungKhongTonTai);

        var result = tttd.CapNhatTTTD(ten, moTa, linhVucId, thoiHanUngTuyen);
        if (result.IsSuccess)
            ThoiGianCapNhat = DateTime.UtcNow;

        return result;
    }

    public Result XoaTTTD(Guid tttdId)
    {
        var tttd = _thongTinTuyenDungs.FirstOrDefault(x => x.Id == tttdId);
        if (tttd == null)
            return Result.Failure(TuyenDungError.TinTuyenDungKhongTonTai);

        tttd.XoaTTTD();
        ThoiGianCapNhat = DateTime.UtcNow;
        return Result.Success();
    }

    public Result ThemGiayPhep(string duongDan, KieuGiayPhepNTD kieu)
    {
        var result = GiayPhepNhaTuyenDung.Create(duongDan, kieu);
        if (result.IsSuccess)
        {
            _giayPheps.Add(result.Value);
            ThoiGianCapNhat = DateTime.UtcNow;
        }
        return result;
    }
    public Result CapNhatGiayPhep(string oldPath, string newPath, KieuGiayPhepNTD kieu)
    {
        var gp = _giayPheps.FirstOrDefault(x => x.DuongDanLuuTru == oldPath);
        if (gp == null)
            return Result.Failure(TuyenDungError.GiayPhepKhongTonTai);

        _giayPheps.Remove(gp);
        var result = GiayPhepNhaTuyenDung.Create(newPath, kieu);
        if (result.IsSuccess)
        {
            _giayPheps.Add(result.Value);
            ThoiGianCapNhat = DateTime.UtcNow;
        }
        return result;
    }
    public Result XoaGiayPhep(string path)
    {
        var gp = _giayPheps.FirstOrDefault(x => x.DuongDanLuuTru == path);
        if (gp == null)
            return Result.Failure(TuyenDungError.GiayPhepKhongTonTai);

        _giayPheps.Remove(gp);
        ThoiGianCapNhat = DateTime.UtcNow;
        return Result.Success();
    }

    public Result DuyetHoSoUngTuyen(HoSoUngTuyen hoSo, TrangThaiUngTuyen trangThai)
    {
        return hoSo.CapNhatTrangThai(trangThai);
    }

    public Result BaoCaoGianLanBangCap(Guid bangCapId, string lyDo, string ghiChu)
    {
        ThoiGianCapNhat = DateTime.UtcNow;
        return Result.Success();
    }
}
