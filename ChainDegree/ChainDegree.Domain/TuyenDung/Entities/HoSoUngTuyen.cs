using System;
using System.Collections.Generic;
using System.Linq;
using ChainDegree.Domain.TuyenDung.Aggregates;
using ControlHub.SharedKernel.Common.Errors;
using ControlHub.SharedKernel.Results;

namespace ChainDegree.Domain.TuyenDung.Entities;

public class HoSoUngTuyen
{
    private readonly List<BangCapUngTuyen> _bangCapUngTuyens = new();

    public Guid Id { get; private set; }
    public Guid ThongTinTuyenDungId { get; private set; }
    public Guid SinhVienId { get; private set; }
    public DateTime ThoiGianUngTuyen { get; private set; }
    public TrangThaiUngTuyen TrangThaiUngTuyenHienTai { get; private set; }
    public DateTime ThoiGianCapNhat { get; private set; } = DateTime.MinValue;
    public DateTime ThoiGianXoa { get; private set; } = DateTime.MinValue;

    public IReadOnlyCollection<BangCapUngTuyen> BangCapUngTuyens => _bangCapUngTuyens.AsReadOnly();

    private HoSoUngTuyen(Guid id, Guid thongTinTuyenDungId, Guid sinhVienId, DateTime thoiGianUngTuyen, TrangThaiUngTuyen trangThai)
    {
        Id = id;
        ThongTinTuyenDungId = thongTinTuyenDungId;
        SinhVienId = sinhVienId;
        ThoiGianUngTuyen = thoiGianUngTuyen;
        TrangThaiUngTuyenHienTai = trangThai;
    }

    internal static Result<HoSoUngTuyen> Create(Guid thongTinTuyenDungId, Guid sinhVienId)
    {
        if (thongTinTuyenDungId == Guid.Empty)
            return Result<HoSoUngTuyen>.Failure(HoSoUngTuyenError.TinTuyenDungKhongHopLe);

        if (sinhVienId == Guid.Empty)
            return Result<HoSoUngTuyen>.Failure(HoSoUngTuyenError.SinhVienKhongHopLe);

        var hoSoUngTuyen = new HoSoUngTuyen(
            Guid.NewGuid(),
            thongTinTuyenDungId,
            sinhVienId,
            DateTime.UtcNow,
            TrangThaiUngTuyen.ChoXem
        );

        return Result<HoSoUngTuyen>.Success(hoSoUngTuyen);
    }

    public Result ThemBangCapUngTuyen(Guid bangCapId)
    {
        if (TrangThaiUngTuyenHienTai != TrangThaiUngTuyen.ChoXem)
            return Result.Failure(HoSoUngTuyenError.HoSoDaDuocXem);

        if (_bangCapUngTuyens.Any(x => x.BangCapId == bangCapId))
            return Result.Success();

        var result = BangCapUngTuyen.Create(Id, bangCapId);
        if (result.IsFailure)
            return result;

        _bangCapUngTuyens.Add(result.Value);
        ThoiGianCapNhat = DateTime.UtcNow;
        return Result.Success();
    }

    public Result XoaBangCapUngTuyen(Guid bangCapId)
    {
        if (TrangThaiUngTuyenHienTai != TrangThaiUngTuyen.ChoXem)
            return Result.Failure(HoSoUngTuyenError.HoSoDaDuocXem);

        var item = _bangCapUngTuyens.FirstOrDefault(x => x.BangCapId == bangCapId);
        if (item != null)
        {
            _bangCapUngTuyens.Remove(item);
            ThoiGianCapNhat = DateTime.UtcNow;
        }

        return Result.Success();
    }

    public Result CapNhatTrangThai(TrangThaiUngTuyen trangThaiMoi)
    {
        TrangThaiUngTuyenHienTai = trangThaiMoi;
        ThoiGianCapNhat = DateTime.UtcNow;
        return Result.Success();
    }
}
