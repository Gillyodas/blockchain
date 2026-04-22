using System;
using System.Collections.Generic;
using System.Linq;
using ChainDegree.Domain.QuanLyToChuc.Enums;
using ChainDegree.Domain.QuanLyToChuc.ValueObjects;
using ChainDegree.SharedKernel.QuanLyToChuc;
using ControlHub.Domain.SharedKernel;
using ControlHub.SharedKernel.Results;

namespace ChainDegree.Domain.QuanLyToChuc.Aggregates;

public class YeuCauDangKy : AggregateRoot
{
    public Guid Id { get; private set; }
    public string TenToChuc { get; private set; } = null!;
    public Guid TaiKhoanId { get; private set; }
    public LoaiToChuc Loai { get; private set; }
    public TrangThaiYeuCauDangKy TrangThai { get; private set; }
    public DateTime ThoiGianTao { get; private set; }
    public DateTime? ThoiGianNop { get; private set; }
    public DateTime? ThoiGianXetDuyet { get; private set; }
    public string DiaChiVi { get; private set; } = null!;

    public LyDoTuChoi? LyDo { get; private set; }
    public string? GhiChuTuChoi { get; private set; }
    public string? GhiChuDuyet { get; private set; }

    private readonly List<GiayPhepCSDT> _giayPhepCSDTs = new();
    public IReadOnlyCollection<GiayPhepCSDT> GiayPhepCSDTs => _giayPhepCSDTs.AsReadOnly();
    private readonly List<GiayPhepNhaTuyenDung> _giayPhepNTDs = new();
    public IReadOnlyCollection<GiayPhepNhaTuyenDung> GiayPhepNTDs => _giayPhepNTDs.AsReadOnly();

    private YeuCauDangKy() { }

    private YeuCauDangKy(Guid id, string tenToChuc, LoaiToChuc loai, Guid tkId, TrangThaiYeuCauDangKy trangThai, string diaChiVi)
    {
        Id = id;
        TenToChuc = tenToChuc;
        Loai = loai;
        TaiKhoanId = tkId;
        TrangThai = trangThai;
        ThoiGianTao = DateTime.UtcNow;
        DiaChiVi = diaChiVi;
    }

    public static Result<YeuCauDangKy> Create(string tenToChuc, LoaiToChuc loai, Guid taiKhoanId, string diaChiVi)
    {
        if (string.IsNullOrWhiteSpace(tenToChuc))
            return Result<YeuCauDangKy>.Failure(QuanLyToChucError.TenToChucTrong);
        var request = new YeuCauDangKy(Guid.NewGuid(),
            tenToChuc,
            loai,
            taiKhoanId,
            TrangThaiYeuCauDangKy.DaGui,
            diaChiVi);
        return Result<YeuCauDangKy>.Success(request);
    }

    public Result ThemDiaChiVi(string diaChiVi)
    {
        this.DiaChiVi = diaChiVi;
        return Result.Success();
    }

    public Result ThemGiayPhep(string duongDanLuuTru, LoaiGiayPhepCSDT loai, DateTime thoiGianHetHan)
    {
        if (TrangThai != TrangThaiYeuCauDangKy.Nhap)
            return Result.Failure(QuanLyToChucError.HoSoDaGuiKhongDuocSua);

        if (Loai != LoaiToChuc.Issuer)
            return Result.Failure(QuanLyToChucError.SaiLoaiGiayPhep);

        var result = GiayPhepCSDT.Create(duongDanLuuTru, loai, thoiGianHetHan);
        if (result.IsFailure) return Result.Failure(result.Error);
        _giayPhepCSDTs.Add(result.Value);
        return Result.Success();
    }

    public Result ThemGiayPhep(string duongDanLuuTru, LoaiGiayPhepNTD loai)
    {
        if (TrangThai != TrangThaiYeuCauDangKy.Nhap)
            return Result.Failure(QuanLyToChucError.HoSoDaGuiKhongDuocSua);
        if (Loai != LoaiToChuc.Verifier)
            return Result.Failure(QuanLyToChucError.SaiLoaiGiayPhep);
        var result = GiayPhepNhaTuyenDung.Create(duongDanLuuTru, loai);
        if (result.IsFailure) return Result.Failure(result.Error);
        _giayPhepNTDs.Add(result.Value);
        return Result.Success();
    }

    public Result NopHoSo()
    {
        if (TrangThai != TrangThaiYeuCauDangKy.Nhap)
            return Result.Failure(QuanLyToChucError.HoSoKhongPhaiBanNhap);
        if (Loai == LoaiToChuc.Issuer)
        {
            bool coGiayPhepHoatDong = _giayPhepCSDTs.Any(x => x.LoaiGiayPhep == LoaiGiayPhepCSDT.GiayPhepHoatDongGiaoDuc);
            bool coQuyetDinhThanhLap = _giayPhepCSDTs.Any(x => x.LoaiGiayPhep == LoaiGiayPhepCSDT.QuyetDinhThanhLapTruong);

            if (!coGiayPhepHoatDong || !coQuyetDinhThanhLap)
                return Result.Failure(QuanLyToChucError.ThieuGiayPhepBatBuocCSDT);
        }
        else if (Loai == LoaiToChuc.Verifier)
        {
            bool coGiayPhepDKKD = _giayPhepNTDs.Any(x => x.LoaiGiayPhep == LoaiGiayPhepNTD.GiayPhepDangKyKinhDoanh);

            if (!coGiayPhepDKKD)
                return Result.Failure(QuanLyToChucError.ThieuGiayPhepBatBuocNTD);
        }
        TrangThai = TrangThaiYeuCauDangKy.DaGui;
        ThoiGianNop = DateTime.UtcNow;

        return Result.Success();
    }

    public Result TaiLenLaiGiayPhep(LoaiGiayPhepCSDT loai, string duongDanMoi)
    {
        if (TrangThai != TrangThaiYeuCauDangKy.Nhap)
            return Result.Failure(QuanLyToChucError.HoSoKhongTheXetDuyet);

        var giayPhep = _giayPhepCSDTs.FirstOrDefault(x => x.LoaiGiayPhep == loai && x.TrangThai == TrangThaiXacMinh.TuChoi);
        if (giayPhep == null)
            return Result.Failure(GiayPhepCSDTError.GiayPhepCSDTKhongHopLe);

        var result = giayPhep.TaiLenLai(duongDanMoi);
        if (result.IsFailure) return Result.Failure(result.Error);

        _giayPhepCSDTs.Remove(giayPhep);
        _giayPhepCSDTs.Add(result.Value);
        return Result.Success();
    }

    public Result AdminTuChoi(LyDoTuChoi lyDo, string ghiChu)
    {
        if (TrangThai != TrangThaiYeuCauDangKy.DaGui)
            return Result.Failure(QuanLyToChucError.HoSoKhongTheXetDuyet);

        if (lyDo == Enums.LyDoTuChoi.Khac && string.IsNullOrWhiteSpace(ghiChu))
            return Result.Failure(QuanLyToChucError.GhiChuTuChoiBatBuoc);

        TrangThai = TrangThaiYeuCauDangKy.TuChoi;
        LyDo = lyDo;
        GhiChuTuChoi = ghiChu;
        ThoiGianXetDuyet = DateTime.UtcNow;

        return Result.Success();
    }

    public Result AdminDuyet(string? ghiChu)
    {
        if (TrangThai != TrangThaiYeuCauDangKy.DaGui)
            return Result.Failure(QuanLyToChucError.HoSoKhongTheXetDuyet);

        if (string.IsNullOrWhiteSpace(DiaChiVi))
        {
            return Result.Failure(QuanLyToChucError.ThieuDiaChiVi);
        }

        TrangThai = TrangThaiYeuCauDangKy.XacNhan;
        GhiChuDuyet = ghiChu;
        ThoiGianXetDuyet = DateTime.UtcNow;

        return Result.Success();
    }
}

