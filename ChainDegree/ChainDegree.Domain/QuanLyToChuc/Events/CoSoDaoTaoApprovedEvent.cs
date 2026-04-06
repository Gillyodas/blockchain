using System;
using System.Collections.Generic;
using ChainDegree.Domain.QuanLyToChuc.ValueObjects;
using ControlHub.Domain.SharedKernel;

namespace ChainDegree.Domain.QuanLyToChuc.Events;

public class CoSoDaoTaoApprovedEvent : IDomainEvent
{
    public Guid YeuCauDangKyId { get; }
    public Guid TaiKhoanId { get; }
    public string TenToChuc { get; }
    public IReadOnlyCollection<GiayPhepCSDT> GiayPhepCSDTs { get; }
    public DateTime OccurredOn { get; }

    public CoSoDaoTaoApprovedEvent(Guid yeuCauDangKyId, Guid taiKhoanId, string tenToChuc, IReadOnlyCollection<GiayPhepCSDT> giayPhepCSDTs)
    {
        YeuCauDangKyId = yeuCauDangKyId;
        TaiKhoanId = taiKhoanId;
        TenToChuc = tenToChuc;
        GiayPhepCSDTs = giayPhepCSDTs;
        OccurredOn = DateTime.UtcNow;
    }
}
