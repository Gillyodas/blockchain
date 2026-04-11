using ChainDegree.Domain.QuanLyBangCap.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChainDegree.Infrastructure.Persistence.Configurations.BaoCaoGianLan;

public class BaoCaoGianLanConfiguration : IEntityTypeConfiguration<Domain.BaoCaoGianLan.Aggregates.BaoCaoGianLan>
{
    public void Configure(EntityTypeBuilder<Domain.BaoCaoGianLan.Aggregates.BaoCaoGianLan> builder)
    {
        builder.ToTable("BaoCaoGianLan");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.BangCapId).IsRequired();
        builder.Property(b => b.CoSoDaoTaoId).IsRequired();
        builder.Property(b => b.NguoiBaoCaoId).IsRequired();
        builder.Property(b => b.LoaiNguoiBaoCao).IsRequired();
        builder.Property(b => b.LyDo).IsRequired();
        builder.Property(b => b.GhiChu).HasMaxLength(2000);
        builder.Property(b => b.TrangThai).IsRequired();
        builder.Property(b => b.ThoiGianBaoCao).IsRequired();
        builder.Property(b => b.ThoiGianCapNhat);

        // Shadow property — admin nào xử lý báo cáo (spec 10.2 bước 4)
        // Không thuộc domain model, set bởi Application handler khi TiepNhan/XacNhanGianLan/TuChoiBaoCao
        builder.Property<Guid?>("XuLyBoiAdminId");

        builder.HasOne<CoSoDaoTao>()
            .WithMany()
            .HasForeignKey(b => b.CoSoDaoTaoId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(b => b.BangCapId);
        builder.HasIndex(b => b.NguoiBaoCaoId);
    }
}
