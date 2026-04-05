using System;
using ChainDegree.Domain.TuyenDung.Errors;
using ControlHub.SharedKernel.Results;

namespace ChainDegree.Domain.TuyenDung.Entities;

public class BangCapUngTuyen
{
    public Guid HoSoUngTuyenId { get; private set; }
    public Guid BangCapId { get; private set; }

    private BangCapUngTuyen(Guid hoSoUngTuyenId, Guid bangCapId)
    {
        HoSoUngTuyenId = hoSoUngTuyenId;
        BangCapId = bangCapId;
    }

    internal static Result<BangCapUngTuyen> Create(Guid hoSoUngTuyenId, Guid bangCapId)
    {
        if (hoSoUngTuyenId == Guid.Empty || bangCapId == Guid.Empty)
            return Result<BangCapUngTuyen>.Failure(TuyenDungError.BangCapKhongHopLe);

        return Result<BangCapUngTuyen>.Success(new BangCapUngTuyen(hoSoUngTuyenId, bangCapId));
    }
}
