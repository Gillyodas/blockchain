using ChainDegree.Domain.DanhMuc.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChainDegree.Infrastructure.Persistence.Configurations.DanhMuc;

public class LinhVucConfiguration : IEntityTypeConfiguration<LinhVuc>
{
    public void Configure(EntityTypeBuilder<LinhVuc> builder)
    {
        builder.ToTable("LinhVuc");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Ten)
            .HasMaxLength(256)
            .IsRequired();

        builder.HasIndex(l => l.Ten).IsUnique();

        builder.Property(l => l.ThoiGianTao).IsRequired();
        builder.Property(l => l.ThoiGianCapNhat);
        builder.Property(l => l.ThoiGianXoa);
    }
}
