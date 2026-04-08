using ChainDegree.Domain.TuyenDung.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChainDegree.Infrastructure.Persistence.Configurations.TuyenDung;

public class ThongTinTuyenDungConfiguration : IEntityTypeConfiguration<ThongTinTuyenDung>
{
    public void Configure(EntityTypeBuilder<ThongTinTuyenDung> builder)
    {
        builder.ToTable("ThongTinTuyenDung");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Ten)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(t => t.MoTa)
            .HasMaxLength(4000)
            .IsRequired();

        builder.Property(t => t.LinhVucId).IsRequired();
        builder.Property(t => t.ThoiHanUngTuyen).IsRequired();
        builder.Property(t => t.NhaTuyenDungId).IsRequired();

        builder.HasIndex(t => t.NhaTuyenDungId);
        builder.HasIndex(t => t.LinhVucId);

        builder.Property(t => t.ThoiGianTao).IsRequired();
        builder.Property(t => t.ThoiGianCapNhat);
        builder.Property(t => t.ThoiGianXoa); // soft delete
    }
}
