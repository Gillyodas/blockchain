using System;
using System.Collections.Generic;
using System.Text;
using ChainDegree.Application.QuanLyBangCap.Interfaces.Repositories;
using ChainDegree.Domain.QuanLyBangCap.Entities;
using ChainDegree.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ChainDegree.Infrastructure.QuanLyBangCap.Repositories;

public class SinhVienRepository : ISinhVienRepository
{
    private readonly ChainDegreeDbContext _db;
    public SinhVienRepository(ChainDegreeDbContext db)
    {
        _db = db;
    }

    public async Task<SinhVien?> GetByCCCDAsync(string cccd, CancellationToken cancellationToken)
    {
        SinhVien? sinhVien = await _db.SinhViens.FirstOrDefaultAsync(sv => sv.CCCD == cccd);
        return sinhVien;
    }

    public async Task<SinhVien?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        SinhVien? sinhVien = await _db.SinhViens.FirstOrDefaultAsync(sv => sv.Id == id);
        return sinhVien;
    }
}
