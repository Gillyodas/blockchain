using ChainDegree.Domain.TuyenDung.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChainDegree.Infrastructure.Persistence.Configurations.TuyenDung;

public class KetQuaPhanTichConfiguration : IEntityTypeConfiguration<KetQuaPhanTich>
{
    public void Configure(EntityTypeBuilder<KetQuaPhanTich> builder)
    {
        builder.ToTable("KetQuaPhanTich");

        builder.HasKey(k => k.Id);

        builder.Property(k => k.ThongTinTuyenDungId).IsRequired();
        builder.Property(k => k.HoSoUngTuyenId).IsRequired();

        builder.Property(k => k.PhanTramPhuHop).IsRequired();

        // KetLuan lưu JSON (điểm mạnh, điểm thiếu, đề xuất)
        builder.Property(k => k.KetLuan)
            .HasColumnType("nvarchar(max)")
            .IsRequired();

        builder.Property(k => k.ThoiGianPhanTich).IsRequired();

        builder.HasOne<ThongTinTuyenDung>()
            .WithMany()
            .HasForeignKey(k => k.ThongTinTuyenDungId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<HoSoUngTuyen>()
            .WithMany()
            .HasForeignKey(k => k.HoSoUngTuyenId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(k => k.HoSoUngTuyenId);
        builder.HasIndex(k => k.ThongTinTuyenDungId);
    }
}
