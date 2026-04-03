using System;
using System.Collections.Generic;
using System.Text;
using ControlHub.SharedKernel.Results;
using ChainDegree.Domain.QuanLyToChuc.Enums;
using ChainDegree.SharedKernel.QuanLyToChuc;
using ControlHub.Domain.SharedKernel;

namespace ChainDegree.Domain.QuanLyToChuc.ValueObjects;

public class GiayPhepCSDT : ValueObject
{
    public string DuongDanLuuTru { get; private set; }
    public LoaiGiayPhepCSDT LoaiGiayPhep { get; private set; }
    public TrangThaiXacMinh TrangThai { get; private set; }
    private GiayPhepCSDT(string duongDanLuuTru, LoaiGiayPhepCSDT loai, TrangThaiXacMinh trangThai)
    {
        DuongDanLuuTru = duongDanLuuTru;
        LoaiGiayPhep = loai;
        TrangThai = trangThai;
    }
    internal static Result<GiayPhepCSDT> Create(string duongDanLuuTru, LoaiGiayPhepCSDT loai)
    {
        if (string.IsNullOrWhiteSpace(duongDanLuuTru))
            return Result<GiayPhepCSDT>.Failure(QuanLyToChucError.DuongDanFileTrong);
        var giayPhep = new GiayPhepCSDT(duongDanLuuTru, loai, TrangThaiXacMinh.ChoXacMinh);
        return Result<GiayPhepCSDT>.Success(giayPhep);
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return DuongDanLuuTru;
        yield return LoaiGiayPhep;
        yield return TrangThai;
    }
}
