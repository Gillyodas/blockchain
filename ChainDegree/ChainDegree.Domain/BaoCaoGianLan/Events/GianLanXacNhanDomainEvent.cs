using ControlHub.Domain.SharedKernel;

namespace ChainDegree.Domain.BaoCaoGianLan.Events;

public class GianLanXacNhanDomainEvent : IDomainEvent
{
    public Guid BaoCaoId { get; }
    public Guid BangCapId { get; }
    public Guid CoSoDaoTaoId { get; }
    public DateTime OccurredOn { get; }

    public GianLanXacNhanDomainEvent(Guid baoCaoId, Guid bangCapId, Guid coSoDaoTaoId)
    {
        BaoCaoId = baoCaoId;
        BangCapId = bangCapId;
        CoSoDaoTaoId = coSoDaoTaoId;
        OccurredOn = DateTime.UtcNow;
    }
}
