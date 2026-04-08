using ChainDegree.Domain.QuanLyBangCap.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChainDegree.Infrastructure.Persistence.Configurations.QuanLyBangCap;

public class BangCapConfiguration : IEntityTypeConfiguration<BangCap>
{
    public void Configure(EntityTypeBuilder<BangCap> builder)
    {
        builder.ToTable("BangCap");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Ten)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(b => b.File).HasMaxLength(1024);
        builder.Property(b => b.Link).HasMaxLength(1024);
        builder.Property(b => b.Diem);
        builder.Property(b => b.NgayCap).IsRequired();
        builder.Property(b => b.NgayHetHan);

        // Dữ liệu blockchain
        builder.Property(b => b.MaBamXacThuc).HasMaxLength(66); // keccak256 hex = 0x + 64 chars
        builder.Property(b => b.MaBamGiaoDich).HasMaxLength(66);
        builder.Property(b => b.Salt).HasMaxLength(256);

        // Ghi chú
        builder.Property(b => b.GhiChuHuy).HasMaxLength(1024);
        builder.Property(b => b.GhiChuThuHoi).HasMaxLength(1024);
        builder.Property(b => b.GhiChuKhoiPhuc).HasMaxLength(1024);

        // Enums
        builder.Property(b => b.LoaiBangCap).IsRequired();
        builder.Property(b => b.TrangThaiBangCapHienTai).IsRequired();
        builder.Property(b => b.TrangThaiBlockchainHienTai).IsRequired();
        builder.Property(b => b.LyDoHuyBangCap);      // nullable enum
        builder.Property(b => b.LyDoThuHoiBangCap);   // nullable enum
        builder.Property(b => b.LyDoKhoiPhucBangCap); // nullable enum

        // FKs — không có navigation properties (aggregate boundary)
        builder.Property(b => b.CoSoDaoTaoCapId).IsRequired();
        builder.Property(b => b.SinhVienId).IsRequired();
        builder.Property(b => b.LinhVucId).IsRequired();

        builder.HasIndex(b => b.MaBamXacThuc).IsUnique().HasFilter("[MaBamXacThuc] IS NOT NULL");
        builder.HasIndex(b => b.CoSoDaoTaoCapId);
        builder.HasIndex(b => b.SinhVienId);

        builder.Property(b => b.ThoiGianTao).IsRequired();
        builder.Property(b => b.ThoiGianCapNhat);
        builder.Property(b => b.ThoiGianXoa); // soft delete
    }
}
