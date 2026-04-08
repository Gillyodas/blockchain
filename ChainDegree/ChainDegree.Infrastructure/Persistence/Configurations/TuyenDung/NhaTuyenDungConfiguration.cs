using ChainDegree.Domain.TuyenDung.Aggregates;
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

        // Owned collection: GiayPhepNhaTuyenDung — private field, không có public property
        builder.OwnsMany<Domain.TuyenDung.ValueObjects.GiayPhepNhaTuyenDung>("_giayPheps", giayPhep =>
        {
            giayPhep.ToTable("NhaTuyenDung_GiayPhepNTD");
            giayPhep.WithOwner().HasForeignKey("NhaTuyenDungId");

            giayPhep.Property<int>("Id").ValueGeneratedOnAdd();
            giayPhep.HasKey("Id");

            giayPhep.Property(g => g.DuongDanLuuTru).HasMaxLength(1024).IsRequired();
            giayPhep.Property(g => g.KieuGiayPhep).IsRequired();
            giayPhep.Property(g => g.ThoiGianTaiLen).IsRequired();
            giayPhep.Property(g => g.ThoiGianDuocXacMinh);
            giayPhep.Property(g => g.XacMinhBoiAdminId);
        });

        // Relationship: ThongTinTuyenDung — private backing field, không có public property
        // EF dùng tên field "_thongTinTuyenDungs" để populate collection khi load aggregate từ DB
        builder.HasMany<Domain.TuyenDung.Entities.ThongTinTuyenDung>("_thongTinTuyenDungs")
            .WithOne()
            .HasForeignKey(t => t.NhaTuyenDungId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
