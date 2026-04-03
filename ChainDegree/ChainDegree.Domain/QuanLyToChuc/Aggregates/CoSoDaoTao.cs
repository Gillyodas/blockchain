using System;
using ChainDegree.Domain.QuanLyToChuc.ValueObjects;
using ChainDegree.SharedKernel.QuanLyToChuc;
using ControlHub.SharedKernel.Results;

namespace ChainDegree.Domain.QuanLyToChuc.Aggregates;

public class CoSoDaoTao
{
    public Guid Id { get; private set; }
    public string Ten { get; private set; }
    public Guid TaiKhoanId { get; private set; }
    public UyTinToChuc UyTin { get; private set; }
    public DateTime ThoiGianTao { get; private set; }

    private CoSoDaoTao(Guid id, string ten, Guid taiKhoanId, UyTinToChuc uyTin)
    {
        Id = id;
        Ten = ten;
        TaiKhoanId = taiKhoanId;
        UyTin = uyTin;
        ThoiGianTao = DateTime.UtcNow;
    }

    internal static Result<CoSoDaoTao> Create(string ten, Guid taiKhoanId)
    {
        if (string.IsNullOrWhiteSpace(ten))
            return Result<CoSoDaoTao>.Failure(QuanLyToChucError.TenToChucTrong);

        return Result<CoSoDaoTao>.Success(new CoSoDaoTao(Guid.NewGuid(), ten, taiKhoanId, UyTinToChuc.KhoiTaoBanDau()));
    }
}
