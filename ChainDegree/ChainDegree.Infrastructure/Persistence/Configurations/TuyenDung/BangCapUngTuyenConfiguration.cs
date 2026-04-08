using ChainDegree.Domain.TuyenDung.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChainDegree.Infrastructure.Persistence.Configurations.TuyenDung;

public class BangCapUngTuyenConfiguration : IEntityTypeConfiguration<BangCapUngTuyen>
{
    public void Configure(EntityTypeBuilder<BangCapUngTuyen> builder)
    {
        builder.ToTable("BangCapUngTuyen");

        builder.HasKey(b => new { b.HoSoUngTuyenId, b.BangCapId });

        builder.Property(b => b.HoSoUngTuyenId).IsRequired();
        builder.Property(b => b.BangCapId).IsRequired();
    }
}
