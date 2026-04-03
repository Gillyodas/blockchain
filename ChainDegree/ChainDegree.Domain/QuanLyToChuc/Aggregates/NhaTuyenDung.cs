using System;
using ChainDegree.Domain.QuanLyToChuc.ValueObjects;
using ChainDegree.SharedKernel.QuanLyToChuc;
using ControlHub.SharedKernel.Results;

namespace ChainDegree.Domain.QuanLyToChuc.Aggregates;

public class NhaTuyenDung
{
    public Guid Id { get; private set; }
    public string TenCongTy { get; private set; }
    public Guid TaiKhoanId { get; private set; }
    public UyTinToChuc UyTin { get; private set; }
    public DateTime ThoiGianTao { get; private set; }

    private NhaTuyenDung(Guid id, string tenCongTy, Guid taiKhoanId, UyTinToChuc uyTin)
    {
        Id = id;
        TenCongTy = tenCongTy;
        TaiKhoanId = taiKhoanId;
        UyTin = uyTin;
        ThoiGianTao = DateTime.UtcNow;
    }

    internal static Result<NhaTuyenDung> Create(string tenCongTy, Guid taiKhoanId)
    {
        if (string.IsNullOrWhiteSpace(tenCongTy))
            return Result<NhaTuyenDung>.Failure(QuanLyToChucError.TenToChucTrong);

        return Result<NhaTuyenDung>.Success(new NhaTuyenDung(Guid.NewGuid(), tenCongTy, taiKhoanId, UyTinToChuc.KhoiTaoBanDau()));
    }
}
