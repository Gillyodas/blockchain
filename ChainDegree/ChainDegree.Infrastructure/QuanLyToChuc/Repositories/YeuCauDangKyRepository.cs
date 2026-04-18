using System;
using System.Collections.Generic;
using System.Text;
using ChainDegree.Application.QuanLyToChuc.Interfaces.Repositories;
using ChainDegree.Domain.QuanLyToChuc.Aggregates;
using ChainDegree.Domain.QuanLyToChuc.Enums;
using ChainDegree.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ChainDegree.Infrastructure.QuanLyToChuc.Repositories;

public class YeuCauDangKyRepository : IYeuCauDangKyRepository
{
    private readonly ChainDegreeDbContext _db;
    public YeuCauDangKyRepository(ChainDegreeDbContext db)
    {
        _db = db;
    }
    public async Task AddAsync(YeuCauDangKy yeuCauDangKy, CancellationToken cancellationToken)
    {
        await _db.AddAsync(yeuCauDangKy, cancellationToken);
    }

    public Task<bool> ExistsByDiaChiViAsync(string diaChiVi, CancellationToken cancellationToken)
    {
        return _db.YeuCauDangKys
            .AnyAsync(x => x.DiaChiVi == diaChiVi && x.TrangThai == TrangThaiYeuCauDangKy.XacNhan, cancellationToken);
    }

    public async Task<YeuCauDangKy?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _db.YeuCauDangKys.FindAsync(id, cancellationToken);
    }
}
