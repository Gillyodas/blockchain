using System;
using System.Collections.Generic;
using System.Text;
using ChainDegree.Domain.QuanLyBangCap.Enums;
using ChainDegree.SharedKernel.QuanLyBangCap.BangCap;
using ControlHub.SharedKernel.Results;

namespace ChainDegree.Domain.QuanLyBangCap.Entities;

public class BangCap
{
    public Guid Id { get; private set; }
    public string Ten { get; private set; }
    public string File { get; private set; }
    public double Diem { get; private set; }
    public string Link { get; private set; }
    public DateTime NgayCap { get; private set; }
    public DateTime NgayHetHan { get; private set; }
    public string MaBamXacThuc { get; private set; }
    public string MaBamGiaoDich { get; private set; }
    public string Salt { get; private set; }
    public LoaiBangCap LoaiBangCap { get; private set; }
    public LyDoHuy? LyDoHuy { get; private set; }
    public LyDoThuHoi? LyDoThuHoi { get; private set; }
    public TrangThaiBlockchain? TrangThaiBlockchain { get; private set; }
    public TrangThaiBangCap TrangThaiBangCap { get; private set; }
    public DateTime ThoiGianTao { get; private set; }
    public DateTime ThoiGianCapNhat { get; private set; }
    public DateTime ThoiGianXoa { get; private set; }

    BangCap(Guid id, string ten, string file, double diem, string link, DateTime ngayCap, DateTime ngayHetHan, string maBamXacThuc, string maBamGiaoDich, string salt, LoaiBangCap loaiBangCap, LyDoHuy? lyDoHuy, LyDoThuHoi? lyDoThuHoi, TrangThaiBlockchain? trangThaiBlockchain, TrangThaiBangCap trangThaiBangCap, DateTime thoiGianTao, DateTime thoiGianCapNhat, DateTime thoiGianXoa)
    {
        Id = id;
        Ten = ten;
        File = file;
        Diem = diem;
        Link = link;
        NgayCap = ngayCap;
        NgayHetHan = ngayHetHan;
        MaBamXacThuc = maBamXacThuc;
        MaBamGiaoDich = maBamGiaoDich;
        Salt = salt;
        LoaiBangCap = loaiBangCap;
        LyDoHuy = lyDoHuy;
        LyDoThuHoi = lyDoThuHoi;
        TrangThaiBlockchain = trangThaiBlockchain;
        TrangThaiBangCap = trangThaiBangCap;
        ThoiGianTao = thoiGianTao;
        ThoiGianCapNhat = thoiGianCapNhat;
        ThoiGianXoa = thoiGianXoa;
    }

    public static Result<BangCap> Create(string ten, string file, double diem, string link, DateTime ngayCap, DateTime ngayHetHan, string maBamXacThuc, string maBamGiaoDich, string salt, LoaiBangCap loaiBangCap)
    {
        if(diem < 0)
        {
            return Result<BangCap>.Failure(BangCapError.DiemKhongHopLe);
        }
        BangCap bangCap = new BangCap(Guid.NewGuid(), ten, file, diem, link, ngayCap, ngayHetHan, maBamXacThuc, maBamGiaoDich, salt, loaiBangCap, null, null, null, TrangThaiBangCap.HoatDong, DateTime.UtcNow, DateTime.UtcNow, DateTime.MinValue);
        return Result<BangCap>.Success(bangCap);
    }
}
