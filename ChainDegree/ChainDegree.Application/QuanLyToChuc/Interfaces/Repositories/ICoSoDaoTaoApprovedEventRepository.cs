using System;
using System.Collections.Generic;
using System.Text;
using ChainDegree.Domain.QuanLyToChuc.Events;

namespace ChainDegree.Application.QuanLyToChuc.Interfaces.Repositories;

public interface ICoSoDaoTaoApprovedEventRepository
{
    Task<List<CoSoDaoTaoApprovedEvent>> GetUnprocessedEventsAsync(
        CancellationToken ct = default);

    void Add(CoSoDaoTaoApprovedEvent @event);

    void Update(CoSoDaoTaoApprovedEvent @event);
}
