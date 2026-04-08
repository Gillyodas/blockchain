using ChainDegree.Domain.QuanLyToChuc.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChainDegree.Infrastructure.Persistence.Configurations.QuanLyToChuc;

public class YeuCauDangKyConfiguration : IEntityTypeConfiguration<YeuCauDangKy>
{
    public void Configure(EntityTypeBuilder<YeuCauDangKy> builder)
    {
        builder.ToTable("YeuCauDangKy");

        builder.HasKey(y => y.Id);

        builder.Property(y => y.TenToChuc)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(y => y.TaiKhoanId).IsRequired();
        builder.Property(y => y.Loai).IsRequired();
        builder.Property(y => y.TrangThai).IsRequired();
        builder.Property(y => y.ThoiGianTao).IsRequired();
        builder.Property(y => y.ThoiGianNop);
        builder.Property(y => y.ThoiGianXetDuyet);
        builder.Property(y => y.LyDo);         // nullable enum
        builder.Property(y => y.GhiChuTuChoi).HasMaxLength(1024);
        builder.Property(y => y.GhiChuDuyet).HasMaxLength(1024);

        // Owned collection: GiayPhepCSDT của YeuCauDangKy — bảng riêng
        builder.OwnsMany(y => y.GiayPhepCSDTs, giayPhep =>
        {
            giayPhep.ToTable("YeuCauDangKy_GiayPhepCSDT");
            giayPhep.WithOwner().HasForeignKey("YeuCauDangKyId");

            giayPhep.Property<int>("Id").ValueGeneratedOnAdd();
            giayPhep.HasKey("Id");

            giayPhep.Property(g => g.DuongDanLuuTru).HasMaxLength(1024).IsRequired();
            giayPhep.Property(g => g.LoaiGiayPhep).IsRequired();
            giayPhep.Property(g => g.TrangThai).IsRequired();
            giayPhep.Property(g => g.ThoiGianTaiLen).IsRequired();
            giayPhep.Property(g => g.ThoiGianDuocXacMinh);
            giayPhep.Property(g => g.ThoiGianHetHan);
            giayPhep.Property(g => g.DuocXacMinhBoiAdminId);

            giayPhep.OwnsOne(g => g.ThongTinChuKySoGiayPhepCSDT, chuKy =>
            {
                chuKy.Property(c => c.CoChuKySo).HasColumnName("ChuKy_CoChuKySo");
                chuKy.Property(c => c.HopLe).HasColumnName("ChuKy_HopLe");
                chuKy.Property(c => c.NhaCungCap).HasColumnName("ChuKy_NhaCungCap").HasMaxLength(256);
                chuKy.Property(c => c.NhaCungCapDuocTinTuong).HasColumnName("ChuKy_NhaCungCapDuocTinTuong");
                chuKy.Property(c => c.NgayHetHan).HasColumnName("ChuKy_NgayHetHan");
                chuKy.Property(c => c.XacMinhLuc).HasColumnName("ChuKy_XacMinhLuc");
            });
        });

        builder.Navigation(y => y.GiayPhepCSDTs)
            .HasField("_giayPhepCSDTs")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        // Owned collection: GiayPhepNhaTuyenDung của YeuCauDangKy — bảng riêng
        builder.OwnsMany(y => y.GiayPhepNTDs, giayPhep =>
        {
            giayPhep.ToTable("YeuCauDangKy_GiayPhepNTD");
            giayPhep.WithOwner().HasForeignKey("YeuCauDangKyId");

            giayPhep.Property<int>("Id").ValueGeneratedOnAdd();
            giayPhep.HasKey("Id");

            giayPhep.Property(g => g.DuongDanLuuTru).HasMaxLength(1024).IsRequired();
            giayPhep.Property(g => g.LoaiGiayPhep).IsRequired();
            giayPhep.Property(g => g.TrangThai).IsRequired();
        });

        builder.Navigation(y => y.GiayPhepNTDs)
            .HasField("_giayPhepNTDs")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
