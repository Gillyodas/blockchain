using System;
using System.Collections.Generic;
using System.Text;
using ChainDegree.Application.QuanLyToChuc.Interfaces.Repositories;
using ChainDegree.Domain.QuanLyToChuc.Events;
using ChainDegree.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChainDegree.Infrastructure.QuanLyToChuc.Repositories;

public class CoSoDaoTaoApprovedEventRepository : ICoSoDaoTaoApprovedEventRepository
{
    private readonly ChainDegreeDbContext _db;
    private readonly ILogger<CoSoDaoTaoApprovedEventRepository> _logger;
    public CoSoDaoTaoApprovedEventRepository(ChainDegreeDbContext db, ILogger<CoSoDaoTaoApprovedEventRepository> logger)
    {
        _db = db;
        _logger = logger;
    }
    public async Task<List<CoSoDaoTaoApprovedEvent>> GetUnprocessedEventsAsync(CancellationToken ct = default)
    {
        _logger.LogDebug("Truy vấn CoSoDaoTaoApprovedEvent chưa được xử lý");
        var allEvents = await _db.CoSoDaoTaoApprovedEvents
                .ToListAsync(ct);
        _logger.LogDebug("Tải {Count} event từ database", allEvents.Count);

        var unprocessedEvents = allEvents
                .Where(e => !e.IsProcessed && e.RetryCount < 5)
                .OrderBy(e => e.CreatedAt)
                .ToList();

        _logger.LogDebug(
            "Tìm thấy {Count} event chưa đươc xử lý (IsProcessed=false, RetryCount<5)",
            unprocessedEvents.Count);

        return unprocessedEvents;
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
