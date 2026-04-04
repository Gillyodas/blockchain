using ChainDegree.Domain.QuanLyBangCap.Entities;
using ChainDegree.Domain.QuanLyBangCap.Enums;
using ChainDegree.Domain.QuanLyToChuc.Enums;
using ChainDegree.Domain.QuanLyToChuc.ValueObjects;
using ChainDegree.SharedKernel.QuanLyBangCap.CoSoDaoTao;
using ControlHub.SharedKernel.Results;

namespace ChainDegree.Domain.QuanLyBangCap.Aggregates
{
    public class CoSoDaoTao
    {
        public Guid Id { get; private set; }
        public string Ten { get; private set; }
        public string DiaChiViCSDT { get; private set; }
        public Guid TKId { get; private set; }
        public DateTime ThoiGianTao { get; private set; } = DateTime.UtcNow;
        public DateTime? ThoiGianCapNhat { get; private set; }
        public DateTime? ThoiGianXoa { get; private set; }
        private readonly List<GiayPhepCSDT> _danhSachGiayPhepCSDT = new();
        public IReadOnlyCollection<GiayPhepCSDT> DanhSachGiayPhepCSDT
            => _danhSachGiayPhepCSDT.AsReadOnly();
        public UyTinToChuc UyTin { get; private set; }

        private CoSoDaoTao(Guid id, string ten, string diaChiViCSDT,
            Guid tkId, List<GiayPhepCSDT> danhSachGiayPhepCSDT, UyTinToChuc uyTin)
        {
            Id = id;
            Ten = ten;
            DiaChiViCSDT = diaChiViCSDT;
            TKId = tkId;
            _danhSachGiayPhepCSDT = new List<GiayPhepCSDT>(danhSachGiayPhepCSDT); ;
            UyTin = uyTin;
        }

        public static Result<CoSoDaoTao> Create(string ten, string diaChiViCSDT, Guid tkId,
            List<GiayPhepCSDT> danhSachGiayPhepCSDT)
        {
            // Ít nhất phải có 2 giấy phép: Giấy phép hoạt động giáo dục và Quyết định thành lập trường
            if (danhSachGiayPhepCSDT == null || danhSachGiayPhepCSDT.Count < 2)
                return Result<CoSoDaoTao>.Failure(CoSoDaoTaoError.ThieuThongTinGiayPhepCSDT);

            if (!danhSachGiayPhepCSDT.Any(gp => gp.LoaiGiayPhep == LoaiGiayPhepCSDT.GiayPhepHoatDongGiaoDuc))
                return Result<CoSoDaoTao>.Failure(CoSoDaoTaoError.ThieuGiayPhepHoatDongGiaoDuc);
            if (!danhSachGiayPhepCSDT.Any(gp => gp.LoaiGiayPhep == LoaiGiayPhepCSDT.QuyetDinhThanhLapTruong))
                return Result<CoSoDaoTao>.Failure(CoSoDaoTaoError.ThieuQuyetDinhThanhLapTruong);

            UyTinToChuc uyTinKhoiTao = UyTinToChuc.KhoiTao(danhSachGiayPhepCSDT.Count());

            CoSoDaoTao csdt = new CoSoDaoTao(Guid.NewGuid(), ten, diaChiViCSDT, tkId, danhSachGiayPhepCSDT, uyTinKhoiTao);

            return Result<CoSoDaoTao>.Success(csdt);
        }

        public Result<SinhVien> TaoSinhVien(string ten, string cccd, string email, string diaChiViSinhVien, Guid tkId)
        {
            return SinhVien.Create(ten, cccd, email, diaChiViSinhVien, tkId);
        }

        public Result HuyLienKetSinhVien()
        {
            throw new NotImplementedException();
        }

        public Result CapNhatThongTinCSDT(string ten, string diaChiViCSDT)
        {
            Ten = ten;
            DiaChiViCSDT = diaChiViCSDT;
            ThoiGianCapNhat = DateTime.UtcNow;
            return Result.Success();
        }

        public Result ThemGiayPhepCSDT(GiayPhepCSDT giayPhepCSDT)
        {
            _danhSachGiayPhepCSDT.Add(giayPhepCSDT);
            UyTin = UyTin.ThemGiayPhep();
            ThoiGianCapNhat = DateTime.UtcNow;
            return Result.Success();
        }

        public Result<BangCap> TaoBangCapChoSinhVien(
            string ten,
            double? diem,
            LoaiBangCap loaiBangCap,
            Guid linhVucId,
            string? file,
            string? link,
            DateTime ngayCap,
            DateTime? ngayHetHan,
            Guid sinhVienId)
        {
            return BangCap.Create(ten, diem, loaiBangCap, linhVucId, ngayCap, ngayHetHan, file, link, this.Id, sinhVienId);
        }

        public Result GanMaBamXacThucChoBangCap(BangCap bangCap, string maBamXacThuc, string salt)
        {
            return bangCap.GanMaBamXacThuc(maBamXacThuc, salt);
        }

        public Result GanMaBamGiaoDichChoBangCap(BangCap bangCap, string maBamGiaoDich)
        {
            var result = bangCap.GanMaBamGiaoDich(maBamGiaoDich);
            if (result.IsFailure) return result;

            UyTin = UyTin.CongDiemCapBangThanhCong();
            return result;
        }

        public Result HuyBangCap(BangCap bangCap, LyDoHuy lyDoHuy, string? ghiChuHuy)
        {
            var result = bangCap.DanhDauHuy(lyDoHuy, ghiChuHuy, this.Id);
            if (result.IsFailure) return result;

            if (lyDoHuy == LyDoHuy.LoiNhapLieu || lyDoHuy == LyDoHuy.NhapTrungLap)
                UyTin = UyTin.TruDiemHuyBangLoiNhapLieu();

            return result;
        }

        public Result ThuHoiBangCap(BangCap bangCap, LyDoThuHoi lyDoThuHoi, string? ghiChuThuHoi)
        {
            var result = bangCap.DanhDauThuHoi(lyDoThuHoi, ghiChuThuHoi, this.Id);
            if (result.IsFailure) return result;

            if (lyDoThuHoi == LyDoThuHoi.BangGia || lyDoThuHoi == LyDoThuHoi.GianLanXacNhan)
                UyTin = UyTin.TruDiemThuHoiGianLan();
            else if (lyDoThuHoi == LyDoThuHoi.ThayDoiQuyDinh)
                UyTin = UyTin.TruDiemThuHoiBang();

            return result;
        }

        public Result<BangCap> KhoiPhucBangCap(BangCap bangCap, LyDoKhoiPhuc lyDoKhoiPhuc, string ghiChuKhoiPhuc)
        {
            return bangCap.KhoiPhuc(this.Id, lyDoKhoiPhuc, ghiChuKhoiPhuc);
        }
    }
}
