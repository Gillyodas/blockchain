using ChainDegree.Domain.BaoCaoGianLan.Aggregates;
using ChainDegree.Domain.DanhMuc.Entities;
using ChainDegree.Domain.QuanLyBangCap.Aggregates;
using ChainDegree.Domain.QuanLyBangCap.Entities;
using ChainDegree.Domain.QuanLyToChuc.Aggregates;
using ChainDegree.Domain.QuanLyToChuc.Events;
using ChainDegree.Domain.TuyenDung.Aggregates;
using ChainDegree.Domain.TuyenDung.Entities;
using ChainDegree.Domain.XacMinhBangCap.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace ChainDegree.Infrastructure.Persistence;

public class ChainDegreeDbContext : DbContext
{
    public ChainDegreeDbContext(DbContextOptions<ChainDegreeDbContext> options)
        : base(options)
    {
    }

    // QuanLyBangCap
    public DbSet<CoSoDaoTao> CoSoDaoTaos { get; set; } = default!;
    public DbSet<BangCap> BangCaps { get; set; } = default!;
    public DbSet<SinhVien> SinhViens { get; set; } = default!;

    // QuanLyToChuc
    public DbSet<YeuCauDangKy> YeuCauDangKys { get; set; } = default!;

    // TuyenDung
    public DbSet<NhaTuyenDung> NhaTuyenDungs { get; set; } = default!;
    public DbSet<ThongTinTuyenDung> ThongTinTuyenDungs { get; set; } = default!;
    public DbSet<HoSoUngTuyen> HoSoUngTuyens { get; set; } = default!;
    public DbSet<BangCapUngTuyen> BangCapUngTuyens { get; set; } = default!;
    public DbSet<KetQuaPhanTich> KetQuaPhanTichs { get; set; } = default!;

    // BaoCaoGianLan
    public DbSet<BaoCaoGianLan> BaoCaoGianLans { get; set; } = default!;

    // XacMinhBangCap
    public DbSet<NhatKyXacMinh> NhatKyXacMinhs { get; set; } = default!;

    // DanhMuc
    public DbSet<LinhVuc> LinhVucs { get; set; } = default!;

    // Events
    public DbSet<CoSoDaoTaoApprovedEvent> CoSoDaoTaoApprovedEvents { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChainDegreeDbContext).Assembly);

        // Soft delete global query filters
        modelBuilder.Entity<CoSoDaoTao>().HasQueryFilter(e => e.ThoiGianXoa == null);
        modelBuilder.Entity<BangCap>().HasQueryFilter(e => e.ThoiGianXoa == null);
        modelBuilder.Entity<SinhVien>().HasQueryFilter(e => e.ThoiGianXoa == null);
        modelBuilder.Entity<NhaTuyenDung>().HasQueryFilter(e => e.ThoiGianXoa == null);
        modelBuilder.Entity<ThongTinTuyenDung>().HasQueryFilter(e => e.ThoiGianXoa == null);
        modelBuilder.Entity<HoSoUngTuyen>().HasQueryFilter(e => e.ThoiGianXoa == null);
        modelBuilder.Entity<LinhVuc>().HasQueryFilter(e => e.ThoiGianXoa == null);

        base.OnModelCreating(modelBuilder);
    }
}
