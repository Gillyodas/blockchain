using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChainDegree.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CoSoDaoTaoApprovedEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YeuCauDangKyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DiaChiVi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenToChuc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Payload = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2026, 4, 20, 16, 41, 55, 559, DateTimeKind.Utc).AddTicks(2626)),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsProcessed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RetryCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoSoDaoTaoApprovedEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LinhVuc",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThoiGianCapNhat = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ThoiGianXoa = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinhVuc", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NhaTuyenDung",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    DiaChiViNhaTuyenDung = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    TaiKhoanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    YeuCauDangKyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThoiGianCapNhat = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ThoiGianXoa = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhaTuyenDung", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SinhVien",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CCCD = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DiaChiViSinhVien = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    TaiKhoanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThoiGianCapNhat = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ThoiGianXoa = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SinhVien", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "YeuCauDangKy",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenToChuc = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    TaiKhoanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Loai = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThoiGianNop = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ThoiGianXetDuyet = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DiaChiVi = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    LyDo = table.Column<int>(type: "int", nullable: true),
                    GhiChuTuChoi = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    GhiChuDuyet = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YeuCauDangKy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NhatKyXacMinh",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BangCapId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NhaTuyenDungId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ThoiGianXacMinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaBamXacMinh = table.Column<string>(type: "nvarchar(66)", maxLength: 66, nullable: false),
                    KetQuaXacMinh = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhatKyXacMinh", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NhatKyXacMinh_NhaTuyenDung_NhaTuyenDungId",
                        column: x => x.NhaTuyenDungId,
                        principalTable: "NhaTuyenDung",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "NhaTuyenDung_GiayPhepNTD",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DuongDanLuuTru = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    LoaiGiayPhep = table.Column<int>(type: "int", nullable: false),
                    ThoiGianTaiLen = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThoiGianDuocXacMinh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    XacMinhBoiAdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    NhaTuyenDungId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhaTuyenDung_GiayPhepNTD", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NhaTuyenDung_GiayPhepNTD_NhaTuyenDung_NhaTuyenDungId",
                        column: x => x.NhaTuyenDungId,
                        principalTable: "NhaTuyenDung",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThongTinTuyenDung",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    LinhVucId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ThoiHanUngTuyen = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NhaTuyenDungId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThoiGianCapNhat = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ThoiGianXoa = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongTinTuyenDung", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThongTinTuyenDung_LinhVuc_LinhVucId",
                        column: x => x.LinhVucId,
                        principalTable: "LinhVuc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ThongTinTuyenDung_NhaTuyenDung_NhaTuyenDungId",
                        column: x => x.NhaTuyenDungId,
                        principalTable: "NhaTuyenDung",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoSoDaoTao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DiaChiViCSDT = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    TaiKhoanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThoiGianCapNhat = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ThoiGianXoa = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UyTin_Diem = table.Column<int>(type: "int", nullable: false),
                    UyTin_SoLuongGiayPhep = table.Column<int>(type: "int", nullable: false),
                    UyTin_SoLuongXacMinhHopLe = table.Column<int>(type: "int", nullable: false),
                    UyTin_SoLuongBangCapBiBaoCaoGianLan = table.Column<int>(type: "int", nullable: false),
                    UyTin_SoLuongBangCapThuHoi = table.Column<int>(type: "int", nullable: false),
                    UyTin_SoLuongBangCapPhatHanh = table.Column<int>(type: "int", nullable: false),
                    UyTin_HangUyTin = table.Column<int>(type: "int", nullable: false),
                    YeuCauDangKyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoSoDaoTao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoSoDaoTao_YeuCauDangKy_YeuCauDangKyId",
                        column: x => x.YeuCauDangKyId,
                        principalTable: "YeuCauDangKy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "YeuCauDangKy_GiayPhepCSDT",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DuongDanLuuTru = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    LoaiGiayPhep = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    ThoiGianTaiLen = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThoiGianDuocXacMinh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ThoiGianHetHan = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DuocXacMinhBoiAdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ChuKy_CoChuKySo = table.Column<bool>(type: "bit", nullable: true),
                    ChuKy_HopLe = table.Column<bool>(type: "bit", nullable: true),
                    ChuKy_NhaCungCap = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ChuKy_NhaCungCapDuocTinTuong = table.Column<bool>(type: "bit", nullable: true),
                    ChuKy_NgayHetHan = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ChuKy_XacMinhLuc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    YeuCauDangKyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YeuCauDangKy_GiayPhepCSDT", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YeuCauDangKy_GiayPhepCSDT_YeuCauDangKy_YeuCauDangKyId",
                        column: x => x.YeuCauDangKyId,
                        principalTable: "YeuCauDangKy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "YeuCauDangKy_GiayPhepNTD",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DuongDanLuuTru = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    LoaiGiayPhep = table.Column<int>(type: "int", nullable: false),
                    ThoiGianTaiLen = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThoiGianDuocXacMinh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    XacMinhBoiAdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    YeuCauDangKyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YeuCauDangKy_GiayPhepNTD", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YeuCauDangKy_GiayPhepNTD_YeuCauDangKy_YeuCauDangKyId",
                        column: x => x.YeuCauDangKyId,
                        principalTable: "YeuCauDangKy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HoSoUngTuyen",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ThongTinTuyenDungId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SinhVienId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ThoiGianUngTuyen = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThaiUngTuyenHienTai = table.Column<int>(type: "int", nullable: false),
                    ThoiGianCapNhat = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ThoiGianXoa = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoSoUngTuyen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HoSoUngTuyen_SinhVien_SinhVienId",
                        column: x => x.SinhVienId,
                        principalTable: "SinhVien",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HoSoUngTuyen_ThongTinTuyenDung_ThongTinTuyenDungId",
                        column: x => x.ThongTinTuyenDungId,
                        principalTable: "ThongTinTuyenDung",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BangCap",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    File = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Diem = table.Column<double>(type: "float", nullable: true),
                    Link = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    NgayCap = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayHetHan = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MaBamXacThuc = table.Column<string>(type: "nvarchar(66)", maxLength: 66, nullable: true),
                    MaBamGiaoDich = table.Column<string>(type: "nvarchar(66)", maxLength: 66, nullable: true),
                    Salt = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LoaiBangCap = table.Column<int>(type: "int", nullable: false),
                    LinhVucId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LyDoHuyBangCap = table.Column<int>(type: "int", nullable: true),
                    LyDoThuHoiBangCap = table.Column<int>(type: "int", nullable: true),
                    LyDoKhoiPhucBangCap = table.Column<int>(type: "int", nullable: true),
                    GhiChuHuy = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    GhiChuThuHoi = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    GhiChuKhoiPhuc = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    TrangThaiBlockchainHienTai = table.Column<int>(type: "int", nullable: false),
                    TrangThaiBangCapHienTai = table.Column<int>(type: "int", nullable: false),
                    CoSoDaoTaoCapId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SinhVienId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThoiGianCapNhat = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ThoiGianXoa = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BangCap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BangCap_CoSoDaoTao_CoSoDaoTaoCapId",
                        column: x => x.CoSoDaoTaoCapId,
                        principalTable: "CoSoDaoTao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BangCap_LinhVuc_LinhVucId",
                        column: x => x.LinhVucId,
                        principalTable: "LinhVuc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BangCap_SinhVien_SinhVienId",
                        column: x => x.SinhVienId,
                        principalTable: "SinhVien",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BaoCaoGianLan",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BangCapId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NguoiBaoCaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CoSoDaoTaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoaiNguoiBaoCao = table.Column<int>(type: "int", nullable: false),
                    LyDo = table.Column<int>(type: "int", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    ThoiGianBaoCao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThoiGianCapNhat = table.Column<DateTime>(type: "datetime2", nullable: true),
                    XuLyBoiAdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaoCaoGianLan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaoCaoGianLan_CoSoDaoTao_CoSoDaoTaoId",
                        column: x => x.CoSoDaoTaoId,
                        principalTable: "CoSoDaoTao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GiayPhepCSDT",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DuongDanLuuTru = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    LoaiGiayPhep = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    ThoiGianTaiLen = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThoiGianDuocXacMinh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ThoiGianHetHan = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DuocXacMinhBoiAdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ChuKy_CoChuKySo = table.Column<bool>(type: "bit", nullable: true),
                    ChuKy_HopLe = table.Column<bool>(type: "bit", nullable: true),
                    ChuKy_NhaCungCap = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ChuKy_NhaCungCapDuocTinTuong = table.Column<bool>(type: "bit", nullable: true),
                    ChuKy_NgayHetHan = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ChuKy_XacMinhLuc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CoSoDaoTaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiayPhepCSDT", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GiayPhepCSDT_CoSoDaoTao_CoSoDaoTaoId",
                        column: x => x.CoSoDaoTaoId,
                        principalTable: "CoSoDaoTao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KetQuaPhanTich",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ThongTinTuyenDungId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HoSoUngTuyenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhanTramPhuHop = table.Column<double>(type: "float", nullable: false),
                    KetLuan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThoiGianPhanTich = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KetQuaPhanTich", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KetQuaPhanTich_HoSoUngTuyen_HoSoUngTuyenId",
                        column: x => x.HoSoUngTuyenId,
                        principalTable: "HoSoUngTuyen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KetQuaPhanTich_ThongTinTuyenDung_ThongTinTuyenDungId",
                        column: x => x.ThongTinTuyenDungId,
                        principalTable: "ThongTinTuyenDung",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BangCapUngTuyen",
                columns: table => new
                {
                    HoSoUngTuyenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BangCapId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BangCapUngTuyen", x => new { x.HoSoUngTuyenId, x.BangCapId });
                    table.ForeignKey(
                        name: "FK_BangCapUngTuyen_BangCap_BangCapId",
                        column: x => x.BangCapId,
                        principalTable: "BangCap",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BangCapUngTuyen_HoSoUngTuyen_HoSoUngTuyenId",
                        column: x => x.HoSoUngTuyenId,
                        principalTable: "HoSoUngTuyen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BangCap_CoSoDaoTaoCapId",
                table: "BangCap",
                column: "CoSoDaoTaoCapId");

            migrationBuilder.CreateIndex(
                name: "IX_BangCap_LinhVucId",
                table: "BangCap",
                column: "LinhVucId");

            migrationBuilder.CreateIndex(
                name: "IX_BangCap_MaBamXacThuc",
                table: "BangCap",
                column: "MaBamXacThuc",
                unique: true,
                filter: "[MaBamXacThuc] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BangCap_SinhVienId",
                table: "BangCap",
                column: "SinhVienId");

            migrationBuilder.CreateIndex(
                name: "IX_BangCapUngTuyen_BangCapId",
                table: "BangCapUngTuyen",
                column: "BangCapId");

            migrationBuilder.CreateIndex(
                name: "IX_BaoCaoGianLan_BangCapId",
                table: "BaoCaoGianLan",
                column: "BangCapId");

            migrationBuilder.CreateIndex(
                name: "IX_BaoCaoGianLan_CoSoDaoTaoId",
                table: "BaoCaoGianLan",
                column: "CoSoDaoTaoId");

            migrationBuilder.CreateIndex(
                name: "IX_BaoCaoGianLan_NguoiBaoCaoId",
                table: "BaoCaoGianLan",
                column: "NguoiBaoCaoId");

            migrationBuilder.CreateIndex(
                name: "IX_CoSoDaoTao_YeuCauDangKyId",
                table: "CoSoDaoTao",
                column: "YeuCauDangKyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UnprocessedEvents",
                table: "CoSoDaoTaoApprovedEvents",
                columns: new[] { "IsProcessed", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_GiayPhepCSDT_CoSoDaoTaoId",
                table: "GiayPhepCSDT",
                column: "CoSoDaoTaoId");

            migrationBuilder.CreateIndex(
                name: "IX_HoSoUngTuyen_SinhVienId_ThongTinTuyenDungId",
                table: "HoSoUngTuyen",
                columns: new[] { "SinhVienId", "ThongTinTuyenDungId" },
                unique: true,
                filter: "[ThoiGianXoa] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_HoSoUngTuyen_ThongTinTuyenDungId",
                table: "HoSoUngTuyen",
                column: "ThongTinTuyenDungId");

            migrationBuilder.CreateIndex(
                name: "IX_KetQuaPhanTich_HoSoUngTuyenId",
                table: "KetQuaPhanTich",
                column: "HoSoUngTuyenId");

            migrationBuilder.CreateIndex(
                name: "IX_KetQuaPhanTich_ThongTinTuyenDungId",
                table: "KetQuaPhanTich",
                column: "ThongTinTuyenDungId");

            migrationBuilder.CreateIndex(
                name: "IX_LinhVuc_Ten",
                table: "LinhVuc",
                column: "Ten",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NhatKyXacMinh_BangCapId",
                table: "NhatKyXacMinh",
                column: "BangCapId");

            migrationBuilder.CreateIndex(
                name: "IX_NhatKyXacMinh_NhaTuyenDungId",
                table: "NhatKyXacMinh",
                column: "NhaTuyenDungId");

            migrationBuilder.CreateIndex(
                name: "IX_NhaTuyenDung_GiayPhepNTD_NhaTuyenDungId",
                table: "NhaTuyenDung_GiayPhepNTD",
                column: "NhaTuyenDungId");

            migrationBuilder.CreateIndex(
                name: "IX_SinhVien_CCCD",
                table: "SinhVien",
                column: "CCCD",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SinhVien_TaiKhoanId",
                table: "SinhVien",
                column: "TaiKhoanId");

            migrationBuilder.CreateIndex(
                name: "IX_ThongTinTuyenDung_LinhVucId",
                table: "ThongTinTuyenDung",
                column: "LinhVucId");

            migrationBuilder.CreateIndex(
                name: "IX_ThongTinTuyenDung_NhaTuyenDungId",
                table: "ThongTinTuyenDung",
                column: "NhaTuyenDungId");

            migrationBuilder.CreateIndex(
                name: "IX_YeuCauDangKy_DiaChiVi",
                table: "YeuCauDangKy",
                column: "DiaChiVi");

            migrationBuilder.CreateIndex(
                name: "IX_YeuCauDangKy_TaiKhoanId",
                table: "YeuCauDangKy",
                column: "TaiKhoanId");

            migrationBuilder.CreateIndex(
                name: "IX_YeuCauDangKy_GiayPhepCSDT_YeuCauDangKyId",
                table: "YeuCauDangKy_GiayPhepCSDT",
                column: "YeuCauDangKyId");

            migrationBuilder.CreateIndex(
                name: "IX_YeuCauDangKy_GiayPhepNTD_YeuCauDangKyId",
                table: "YeuCauDangKy_GiayPhepNTD",
                column: "YeuCauDangKyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BangCapUngTuyen");

            migrationBuilder.DropTable(
                name: "BaoCaoGianLan");

            migrationBuilder.DropTable(
                name: "CoSoDaoTaoApprovedEvents");

            migrationBuilder.DropTable(
                name: "GiayPhepCSDT");

            migrationBuilder.DropTable(
                name: "KetQuaPhanTich");

            migrationBuilder.DropTable(
                name: "NhatKyXacMinh");

            migrationBuilder.DropTable(
                name: "NhaTuyenDung_GiayPhepNTD");

            migrationBuilder.DropTable(
                name: "YeuCauDangKy_GiayPhepCSDT");

            migrationBuilder.DropTable(
                name: "YeuCauDangKy_GiayPhepNTD");

            migrationBuilder.DropTable(
                name: "BangCap");

            migrationBuilder.DropTable(
                name: "HoSoUngTuyen");

            migrationBuilder.DropTable(
                name: "CoSoDaoTao");

            migrationBuilder.DropTable(
                name: "SinhVien");

            migrationBuilder.DropTable(
                name: "ThongTinTuyenDung");

            migrationBuilder.DropTable(
                name: "YeuCauDangKy");

            migrationBuilder.DropTable(
                name: "LinhVuc");

            migrationBuilder.DropTable(
                name: "NhaTuyenDung");
        }
    }
}
