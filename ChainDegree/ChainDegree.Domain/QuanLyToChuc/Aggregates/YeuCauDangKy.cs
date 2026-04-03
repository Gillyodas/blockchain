using System;
using System.Collections.Generic;
using System.Linq;
using ChainDegree.Domain.QuanLyToChuc.Enums;
using ChainDegree.Domain.QuanLyToChuc.ValueObjects;
using ChainDegree.SharedKernel.QuanLyToChuc;
using ControlHub.SharedKernel.Common.Errors;
using ControlHub.SharedKernel.Results;

namespace ChainDegree.Domain.QuanLyToChuc.Aggregates;

public class YeuCauDangKy
{
    public Guid Id { get; private set; }
    public string TenToChuc { get; private set; }
    public Guid TaiKhoanId { get; private set; }
    public LoaiToChuc Loai { get; private set; }
    public TrangThaiYeuCauDangKy TrangThai { get; private set; }

    // Thêm các trường cho Admin xét duyệt
    public LyDoTuChoi? LyDo { get; private set; }
    public string? GhiChuTuChoi { get; private set; }

    private YeuCauDangKy(Guid id, string tenToChuc, LoaiToChuc loai, Guid tkId, TrangThaiYeuCauDangKy trangThai)
    {
        Id = id;
        TenToChuc = tenToChuc;
        Loai = loai;
        TaiKhoanId = tkId;
        TrangThai = trangThai;
    }

    internal static Result<YeuCauDangKy> Create(string tenToChuc, LoaiToChuc loai, Guid taiKhoanId)
    {
        if (string.IsNullOrWhiteSpace(tenToChuc))
            return Result<YeuCauDangKy>.Failure(QuanLyToChucError.TenToChucTrong);
        var request = new YeuCauDangKy(Guid.NewGuid(),
            tenToChuc,
            loai,
            taiKhoanId,
            TrangThaiYeuCauDangKy.Nhap);
        return Result<YeuCauDangKy>.Success(request);
    }

    private readonly List<GiayPhepCSDT> _giayPhepCSDTs = new();
    public IReadOnlyCollection<GiayPhepCSDT> GiayPhepCSDTs => _giayPhepCSDTs.AsReadOnly();
    private readonly List<GiayPhepNhaTuyenDung> _giayPhepNTDs = new();
    public IReadOnlyCollection<GiayPhepNhaTuyenDung> GiayPhepNTDs => _giayPhepNTDs.AsReadOnly();

    public Result ThemGiayPhep(string duongDanLuuTru, LoaiGiayPhepCSDT loai)
    {
        if (TrangThai != TrangThaiYeuCauDangKy.Nhap)
            return Result.Failure(QuanLyToChucError.HoSoDaGuiKhongDuocSua);
        if (Loai != LoaiToChuc.Issuer)
            return Result.Failure(QuanLyToChucError.SaiLoaiGiayPhep);
        var result = GiayPhepCSDT.Create(duongDanLuuTru, loai);
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

        return Result.Success();
    }

    // THÊM: Logic Admin Từ chối
    public Result AdminTuChoi(LyDoTuChoi lyDo, string ghiChu)
    {
        if (TrangThai != TrangThaiYeuCauDangKy.DaGui)
            return Result.Failure(QuanLyToChucError.HoSoKhongTheXetDuyet);

        if (lyDo == Enums.LyDoTuChoi.Khac && string.IsNullOrWhiteSpace(ghiChu))
            return Result.Failure(QuanLyToChucError.GhiChuTuChoiBatBuoc);

        TrangThai = TrangThaiYeuCauDangKy.TuChoi;
        LyDo = lyDo;
        GhiChuTuChoi = ghiChu;

        return Result.Success();
    }

    // THÊM: Logic Admin Duyệt
    public Result AdminDuyet(string? ghiChu)
    {
        if (TrangThai != TrangThaiYeuCauDangKy.DaGui)
            return Result.Failure(QuanLyToChucError.HoSoKhongTheXetDuyet);

        TrangThai = TrangThaiYeuCauDangKy.XacNhan;
        GhiChuTuChoi = ghiChu;

        // Lưu ý: Sau bước này hệ thống sẽ phát tín hiệu Event để tạo CoSoDaoTao/NhaTuyenDung
        return Result.Success();
    }
}

