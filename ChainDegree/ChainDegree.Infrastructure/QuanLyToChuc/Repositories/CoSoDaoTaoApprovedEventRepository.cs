using System;
using System.Collections.Generic;
using System.Text;
using ChainDegree.Application.QuanLyToChuc.Interfaces.Repositories;
using ChainDegree.Domain.QuanLyToChuc.Events;
using ChainDegree.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ChainDegree.Infrastructure.QuanLyToChuc.Repositories;

public class CoSoDaoTaoApprovedEventRepository : ICoSoDaoTaoApprovedEventRepository
{
    private readonly ChainDegreeDbContext _db;
    public CoSoDaoTaoApprovedEventRepository(ChainDegreeDbContext db)
    {
        _db = db;
    }
    public async Task<List<CoSoDaoTaoApprovedEvent>> GetUnprocessedEventsAsync(CancellationToken ct = default)
    {
        return await _db.CoSoDaoTaoApprovedEvents
            .Where(e => e.ShouldProcess)
            .OrderBy(e => e.CreatedAt)
            .ToListAsync(ct);
    }

    public void Add(CoSoDaoTaoApprovedEvent @event)
    {
        _db.CoSoDaoTaoApprovedEvents.Add(@event);
    }

    public void Update(CoSoDaoTaoApprovedEvent @event)
    {
        _db.CoSoDaoTaoApprovedEvents.Update(@event);
    }
}
