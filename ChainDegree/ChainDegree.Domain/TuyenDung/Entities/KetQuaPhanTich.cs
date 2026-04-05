using System;
using ChainDegree.Domain.TuyenDung.Errors;
using ControlHub.SharedKernel.Results;

namespace ChainDegree.Domain.TuyenDung.Entities;

public class KetQuaPhanTich
{
    public Guid Id { get; private set; }
    public Guid ThongTinTuyenDungId { get; private set; }
    public Guid HoSoUngTuyenId { get; private set; }
    public double PhanTramPhuHop { get; private set; }
    public string KetLuan { get; private set; }
    public DateTime ThoiGianPhanTich { get; private set; }

    private KetQuaPhanTich(
        Guid id, 
        Guid thongTinTuyenDungId, 
        Guid hoSoUngTuyenId, 
        double phanTramPhuHop, 
        string ketLuan, 
        DateTime thoiGianPhanTich)
    {
        Id = id;
        ThongTinTuyenDungId = thongTinTuyenDungId;
        HoSoUngTuyenId = hoSoUngTuyenId;
        PhanTramPhuHop = phanTramPhuHop;
        KetLuan = ketLuan;
        ThoiGianPhanTich = thoiGianPhanTich;
    }

    internal static Result<KetQuaPhanTich> Create(
        Guid thongTinTuyenDungId, 
        Guid hoSoUngTuyenId, 
        double phanTramPhuHop, 
        string ketLuan)
    {
        if (phanTramPhuHop < 0 || phanTramPhuHop > 100)
            return Result<KetQuaPhanTich>.Failure(TuyenDungError.PhanTramPhuHopKhongHopLe);

        if (string.IsNullOrWhiteSpace(ketLuan))
            return Result<KetQuaPhanTich>.Failure(TuyenDungError.KetLuanKhongDuocTrong);

        return Result<KetQuaPhanTich>.Success(new KetQuaPhanTich(
            Guid.NewGuid(), 
            thongTinTuyenDungId, 
            hoSoUngTuyenId, 
            phanTramPhuHop, 
            ketLuan, 
            DateTime.UtcNow));
    }
}
