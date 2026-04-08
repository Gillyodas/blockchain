using ChainDegree.Domain.QuanLyBangCap.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChainDegree.Infrastructure.Persistence.Configurations.QuanLyBangCap;

public class CoSoDaoTaoConfiguration : IEntityTypeConfiguration<CoSoDaoTao>
{
    public void Configure(EntityTypeBuilder<CoSoDaoTao> builder)
    {
        builder.ToTable("CoSoDaoTao");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Ten)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(c => c.DiaChiViCSDT)
            .HasMaxLength(512)
            .IsRequired();

        // TKId là tên trong domain, map sang TaiKhoanId cho DB
        builder.Property(c => c.TKId)
            .HasColumnName("TaiKhoanId")
            .IsRequired();

        builder.Property(c => c.ThoiGianTao).IsRequired();
        builder.Property(c => c.ThoiGianCapNhat);
        builder.Property(c => c.ThoiGianXoa); // soft delete

        // Shadow property — truy vết YeuCauDangKy nguồn gốc (spec 2.1 bước 7a)
        // Không thuộc domain model, set bởi Application handler khi xử lý CoSoDaoTaoApprovedEvent
        builder.Property<Guid>("YeuCauDangKyId").IsRequired();

        // Value object: UyTinToChuc — flattened với tiền tố UyTin_
        builder.OwnsOne(c => c.UyTin, uyTin =>
        {
            uyTin.Property(u => u.DiemUyTin)
                .HasColumnName("UyTin_Diem")
                .IsRequired();

            uyTin.Property(u => u.SoLuongGiayPhep)
                .HasColumnName("UyTin_SoLuongGiayPhep")
                .IsRequired();

            uyTin.Property(u => u.SoLuongBangCapPhatHanh)
                .HasColumnName("UyTin_SoLuongBangCapPhatHanh")
                .IsRequired();

            uyTin.Property(u => u.SoLuongBangCapThuHoi)
                .HasColumnName("UyTin_SoLuongBangCapThuHoi")
                .IsRequired();

            uyTin.Property(u => u.SoLuongBangCapBiBaoCaoGianLan)
                .HasColumnName("UyTin_SoLuongBangCapBiBaoCaoGianLan")
                .IsRequired();

            uyTin.Property(u => u.SoLuongXacMinhHopLe)
                .HasColumnName("UyTin_SoLuongXacMinhHopLe")
                .IsRequired();

            uyTin.Property(u => u.Hang)
                .HasColumnName("UyTin_HangUyTin")
                .IsRequired();
        });

        // Owned collection: GiayPhepCSDT — bảng riêng, FK về CoSoDaoTao
        builder.OwnsMany(c => c.DanhSachGiayPhepCSDT, giayPhep =>
        {
            giayPhep.ToTable("GiayPhepCSDT");
            giayPhep.WithOwner().HasForeignKey("CoSoDaoTaoId");

            // Shadow PK vì GiayPhepCSDT là value object không có Id
            giayPhep.Property<int>("Id").ValueGeneratedOnAdd();
            giayPhep.HasKey("Id");

            giayPhep.Property(g => g.DuongDanLuuTru)
                .HasMaxLength(1024)
                .IsRequired();

            giayPhep.Property(g => g.LoaiGiayPhep).IsRequired();
            giayPhep.Property(g => g.TrangThai).IsRequired();
            giayPhep.Property(g => g.ThoiGianTaiLen).IsRequired();
            giayPhep.Property(g => g.ThoiGianDuocXacMinh);
            giayPhep.Property(g => g.ThoiGianHetHan);
            giayPhep.Property(g => g.DuocXacMinhBoiAdminId);

            // Nested owned: ThongTinChuKySo — nullable, flattened với tiền tố ChuKy_
            giayPhep.OwnsOne(g => g.ThongTinChuKySoGiayPhepCSDT, chuKy =>
            {
                chuKy.Property(c => c.CoChuKySo).HasColumnName("ChuKy_CoChuKySo");
                chuKy.Property(c => c.HopLe).HasColumnName("ChuKy_HopLe");
                chuKy.Property(c => c.NhaCungCap)
                    .HasColumnName("ChuKy_NhaCungCap")
                    .HasMaxLength(256);
                chuKy.Property(c => c.NhaCungCapDuocTinTuong)
                    .HasColumnName("ChuKy_NhaCungCapDuocTinTuong");
                chuKy.Property(c => c.NgayHetHan).HasColumnName("ChuKy_NgayHetHan");
                chuKy.Property(c => c.XacMinhLuc).HasColumnName("ChuKy_XacMinhLuc");
            });
        });

        builder.Navigation(c => c.DanhSachGiayPhepCSDT)
            .HasField("_danhSachGiayPhepCSDT")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
