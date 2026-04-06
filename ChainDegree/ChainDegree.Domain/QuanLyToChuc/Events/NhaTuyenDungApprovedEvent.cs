using System;
using System.Collections.Generic;
using ChainDegree.Domain.QuanLyToChuc.ValueObjects;
using ControlHub.Domain.SharedKernel;

namespace ChainDegree.Domain.QuanLyToChuc.Events;

public class NhaTuyenDungApprovedEvent : IDomainEvent
{
    public Guid YeuCauDangKyId { get; }
    public Guid TaiKhoanId { get; }
    public string TenToChuc { get; }
    public IReadOnlyCollection<GiayPhepNhaTuyenDung> GiayPhepNTDs { get; }
    public DateTime OccurredOn { get; }

    public NhaTuyenDungApprovedEvent(Guid yeuCauDangKyId, Guid taiKhoanId, string tenToChuc, IReadOnlyCollection<GiayPhepNhaTuyenDung> giayPhepNTDs)
    {
        YeuCauDangKyId = yeuCauDangKyId;
        TaiKhoanId = taiKhoanId;
        TenToChuc = tenToChuc;
        GiayPhepNTDs = giayPhepNTDs;
        OccurredOn = DateTime.UtcNow;
    }
}
