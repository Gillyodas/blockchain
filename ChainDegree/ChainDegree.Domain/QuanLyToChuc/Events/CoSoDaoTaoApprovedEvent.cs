using System;
using System.Collections.Generic;
using ChainDegree.Domain.Common.Events;
using ChainDegree.Domain.QuanLyToChuc.ValueObjects;
using ControlHub.Domain.SharedKernel;

namespace ChainDegree.Domain.QuanLyToChuc.Events;

public class CoSoDaoTaoApprovedEvent : OutboxEvent
{
    public Guid YeuCauDangKyId { get; set; }
    public string DiaChiVi { get; set; } = string.Empty;
    public string TenToChuc { get; set; } = string.Empty;

    public static CoSoDaoTaoApprovedEvent Create(
        Guid yeuCauDangKyId,
        string diaChiVi,
        string tenToChuc,
        List<string> danhSachDiaChiViDaDuyet)
    { 
        var @event = new CoSoDaoTaoApprovedEvent
        {
            YeuCauDangKyId = yeuCauDangKyId,
            DiaChiVi = diaChiVi,
            TenToChuc = tenToChuc,
            EventType = "CoSoDaoTaoApprovedEvent",
        };
        @event.SetPayload(danhSachDiaChiViDaDuyet);
        return @event;
    }
}
