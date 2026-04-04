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

            Result<UyTinToChuc> resultKhoiTaoUyTinBanDau = UyTinToChuc.KhoiTaoBanDau(danhSachGiayPhepCSDT.Count());
            if(resultKhoiTaoUyTinBanDau.IsFailure)
                return Result<CoSoDaoTao>.Failure(resultKhoiTaoUyTinBanDau.Error);

            CoSoDaoTao csdt = new CoSoDaoTao(Guid.NewGuid(), ten, diaChiViCSDT, tkId, danhSachGiayPhepCSDT, resultKhoiTaoUyTinBanDau.Value);

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
            DateTime ngayHetHan,
            string salt,
            Guid coSoDaoTaoId,
            Guid sinhVienId
            )
        {
            
        }
    }
}
