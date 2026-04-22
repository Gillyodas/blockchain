using ChainDegree.Domain.QuanLyBangCap.Aggregates;
using ChainDegree.Domain.QuanLyToChuc.Aggregates;
using ChainDegree.Domain.QuanLyToChuc.ValueObjects;
using ChainDegree.Domain.TuyenDung.Aggregates;
using ChainDegree.Domain.TuyenDung.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChainDegree.Infrastructure.Persistence.Configurations.TuyenDung;

public class NhaTuyenDungConfiguration : IEntityTypeConfiguration<NhaTuyenDung>
{
    public void Configure(EntityTypeBuilder<NhaTuyenDung> builder)
    {
        builder.ToTable("NhaTuyenDung");

        builder.HasKey(n => n.Id);

        builder.Property(n => n.Ten)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(n => n.DiaChi)
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(n => n.DiaChiViNhaTuyenDung)
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(n => n.TaiKhoanId).IsRequired();
        builder.Property(n => n.YeuCauDangKyId).IsRequired();

        builder.Property(n => n.ThoiGianTao).IsRequired();
        builder.Property(n => n.ThoiGianCapNhat);
        builder.Property(n => n.ThoiGianXoa); // soft delete

        builder.HasOne<YeuCauDangKy>()
            .WithOne()
            .HasForeignKey<NhaTuyenDung>("YeuCauDangKyId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        // Owned collection: GiayPhepNhaTuyenDung — private field, không có public property
        builder.OwnsMany(n => n.GiayPheps, giayPhep =>  // ← Use property name
        {
            giayPhep.ToTable("NhaTuyenDung_GiayPhepNTD");
            giayPhep.WithOwner().HasForeignKey("NhaTuyenDungId");

            giayPhep.Property<int>("Id").ValueGeneratedOnAdd();
            giayPhep.HasKey("Id");

            giayPhep.Property(g => g.DuongDanLuuTru).HasMaxLength(1024).IsRequired();
            giayPhep.Property(g => g.LoaiGiayPhep).IsRequired();
            giayPhep.Property(g => g.ThoiGianTaiLen).IsRequired();
            giayPhep.Property(g => g.ThoiGianDuocXacMinh);
            giayPhep.Property(g => g.XacMinhBoiAdminId);
        });

        // Relationship: ThongTinTuyenDung — private backing field, không có public property
        // EF dùng tên field "_thongTinTuyenDungs" để populate collection khi load aggregate từ DB
        builder.HasMany(n => n.ThongTinTuyenDungs)
            .WithOne()
            .HasForeignKey(t => t.NhaTuyenDungId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
