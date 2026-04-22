using ChainDegree.Application.QuanLyBangCap.Interfaces.Repositories;
using ChainDegree.Domain.QuanLyBangCap.Aggregates;
using ChainDegree.Domain.QuanLyBangCap.Enums;
using ChainDegree.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ChainDegree.Infrastructure.QuanLyBangCap.Repositories;

public class CoSoDaoTaoRepository : ICoSoDaoTaoRepository
{
    private readonly ChainDegreeDbContext _db;

    public CoSoDaoTaoRepository(ChainDegreeDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(CoSoDaoTao coSoDaoTao, CancellationToken cancellationToken)
    {
        await _db.CoSoDaoTaos.AddAsync(coSoDaoTao, cancellationToken);
    }

    public async Task<List<string?>> GetAllAddressWalletAsync(CancellationToken cancellationToken)
    {
        return await _db.CoSoDaoTaos
            .Select(csdt => (string?)csdt.DiaChiViCSDT)
            .ToListAsync(cancellationToken);
    }

    public async Task<CoSoDaoTao?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        CoSoDaoTao? csdt = await _db.CoSoDaoTaos.FindAsync(id, cancellationToken);
        return csdt;
    }

    public async Task<bool> TrungLapKhiCapBangChoSinhVien(Guid csdtId, Guid sinhVienId, LoaiBangCap loaiBangCap, Guid linhVucId, CancellationToken cancellationToken)
    {
        return await _db.BangCaps.AnyAsync(bc =>
            bc.CoSoDaoTaoCapId == csdtId &&
            bc.SinhVienId == sinhVienId &&
            bc.LoaiBangCap == (LoaiBangCap)loaiBangCap &&
            bc.LinhVucId == linhVucId,
            cancellationToken);
    }
}
