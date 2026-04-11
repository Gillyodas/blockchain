using System;
using System.Collections.Generic;
using System.Text;
using ChainDegree.Application.QuanLyBangCap.Interfaces.Repositories;
using ChainDegree.Domain.QuanLyBangCap.Entities;
using ChainDegree.Infrastructure.Persistence;

namespace ChainDegree.Infrastructure.QuanLyBangCap.Repositories;

public class BangCapRepository : IBangCapRepository
{
    private readonly ChainDegreeDbContext _db;
    public BangCapRepository(ChainDegreeDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(BangCap bangCap, CancellationToken cancellationToken = default)
    {
        await _db.AddAsync(bangCap, cancellationToken);
    }
}
