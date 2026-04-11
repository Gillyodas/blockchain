using ChainDegree.Domain.QuanLyBangCap.Entities;
using ChainDegree.Domain.TuyenDung.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChainDegree.Infrastructure.Persistence.Configurations.TuyenDung;

public class HoSoUngTuyenConfiguration : IEntityTypeConfiguration<HoSoUngTuyen>
{
    public void Configure(EntityTypeBuilder<HoSoUngTuyen> builder)
    {
        builder.ToTable("HoSoUngTuyen");

        builder.HasKey(h => h.Id);

        builder.Property(h => h.SinhVienId).IsRequired();
        builder.Property(h => h.ThongTinTuyenDungId).IsRequired();
        builder.Property(h => h.TrangThaiUngTuyenHienTai).IsRequired();
        builder.Property(h => h.ThoiGianUngTuyen).IsRequired();
        builder.Property(h => h.ThoiGianCapNhat);
        builder.Property(h => h.ThoiGianXoa);

        builder.HasOne<SinhVien>()
            .WithMany()
            .HasForeignKey(h => h.SinhVienId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<ThongTinTuyenDung>()
            .WithMany()
            .HasForeignKey(h => h.ThongTinTuyenDungId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(h => new { h.SinhVienId, h.ThongTinTuyenDungId }).IsUnique()
            .HasFilter("[ThoiGianXoa] IS NULL"); // một SV chỉ nộp một lần cho mỗi tin tuyển dụng

        // Owned collection: BangCapUngTuyen — bảng riêng, composite PK
        builder.HasMany(h => h.BangCapUngTuyens)
            .WithOne()
            .HasForeignKey(b => b.HoSoUngTuyenId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(h => h.BangCapUngTuyens)
            .HasField("_bangCapUngTuyens")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
