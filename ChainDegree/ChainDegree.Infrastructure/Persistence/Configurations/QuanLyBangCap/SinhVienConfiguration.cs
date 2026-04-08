using ChainDegree.Domain.QuanLyBangCap.Entities;
using ControlHub.Domain.Identity.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChainDegree.Infrastructure.Persistence.Configurations.QuanLyBangCap;

public class SinhVienConfiguration : IEntityTypeConfiguration<SinhVien>
{
    public void Configure(EntityTypeBuilder<SinhVien> builder)
    {
        builder.ToTable("SinhVien");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Ten)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(s => s.CCCD)
            .HasMaxLength(12)
            .IsRequired();

        builder.HasIndex(s => s.CCCD).IsUnique(); // unique xuyên CSDT

        // Email là value object từ ControlHub — convert sang/từ string
        builder.Property(s => s.Email)
            .HasColumnName("Email")
            .HasMaxLength(256)
            .IsRequired()
            .HasConversion(
                email => email.ToString(),
                str => Email.Create(str).Value);

        builder.Property(s => s.DiaChiViSinhVien)
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(s => s.TKId)
            .HasColumnName("TaiKhoanId")
            .IsRequired();

        builder.Property(s => s.ThoiGianTao).IsRequired();
        builder.Property(s => s.ThoiGianCapNhat);
        builder.Property(s => s.ThoiGianXoa); // soft delete khi không còn CSDT nào quản lý
    }
}
