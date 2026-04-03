using System;
using System.Collections.Generic;
using System.Text;
using ChainDegree.Domain.QuanLyToChuc.Enums;
using ChainDegree.SharedKernel.QuanLyToChuc;
using ControlHub.Domain.SharedKernel;
using ControlHub.SharedKernel.Results;

namespace ChainDegree.Domain.QuanLyToChuc.ValueObjects;

public class GiayPhepNhaTuyenDung : ValueObject
{
    public string DuongDanLuuTru { get; private set; }
    public LoaiGiayPhepNTD LoaiGiayPhep { get; private set; }
    public TrangThaiXacMinh TrangThai { get; private set; }
    private GiayPhepNhaTuyenDung(string duongDanLuuTru, LoaiGiayPhepNTD loai, TrangThaiXacMinh trangThai)
    {
        DuongDanLuuTru = duongDanLuuTru;
        LoaiGiayPhep = loai;
        TrangThai = trangThai;
    }
    internal static Result<GiayPhepNhaTuyenDung> Create(string duongDanLuuTru, LoaiGiayPhepNTD loai)
    {
        if (string.IsNullOrWhiteSpace(duongDanLuuTru))
            return Result<GiayPhepNhaTuyenDung>.Failure(QuanLyToChucError.DuongDanFileTrong);
        var giayPhep = new GiayPhepNhaTuyenDung(duongDanLuuTru, loai, TrangThaiXacMinh.ChoXacMinh);
        return Result<GiayPhepNhaTuyenDung>.Success(giayPhep);
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return DuongDanLuuTru;
        yield return LoaiGiayPhep;
        yield return TrangThai;
    }
}
