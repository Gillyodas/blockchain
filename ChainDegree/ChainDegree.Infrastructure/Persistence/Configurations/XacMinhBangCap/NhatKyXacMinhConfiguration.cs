using ChainDegree.Domain.TuyenDung.Aggregates;
using ChainDegree.Domain.XacMinhBangCap.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChainDegree.Infrastructure.Persistence.Configurations.XacMinhBangCap;

public class NhatKyXacMinhConfiguration : IEntityTypeConfiguration<NhatKyXacMinh>
{
    public void Configure(EntityTypeBuilder<NhatKyXacMinh> builder)
    {
        builder.ToTable("NhatKyXacMinh");

        builder.HasKey(n => n.Id);

        builder.Property(n => n.BangCapId).IsRequired();
        builder.Property(n => n.NhaTuyenDungId); // nullable — null nếu xác minh công khai

        builder.Property(n => n.MaBamXacMinh)
            .HasMaxLength(66) // keccak256 hex
            .IsRequired();

        builder.Property(n => n.KetQuaXacMinh).IsRequired();
        builder.Property(n => n.ThoiGianXacMinh).IsRequired();

        builder.HasOne<NhaTuyenDung>()
            .WithMany()
            .HasForeignKey(n => n.NhaTuyenDungId)
            .IsRequired(false) // nullable
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(n => n.BangCapId);
        builder.HasIndex(n => n.NhaTuyenDungId);
    }
}
