using ChainDegree.Domain.QuanLyBangCap.Entities;
using ChainDegree.Domain.QuanLyBangCap.Enums;
using ChainDegree.Domain.QuanLyBangCap.ValueObjects;
using ChainDegree.SharedKernel.QuanLyBangCap.BangCap;
using ChainDegree.SharedKernel.QuanLyBangCap.CoSoDaoTao;
using ControlHub.Domain.Identity.Aggregates;
using ControlHub.SharedKernel.Results;

namespace ChainDegree.Domain.QuanLyBangCap.Aggregates
{
    public class CoSoDaoTao
    {
        public Guid Id { get; private set; }
        public string Ten { get; private set; }
        public string DiaChiViCSDT { get; private set; }
        public Guid TKId { get; private set; }
        public DateTime ThoiGianTao { get; private set; }
        public DateTime ThoiGianCapNhat { get; private set; }
        public DateTime ThoiGianXoa { get; private set; }
        public List<GiayPhepCSDT> DanhSachGiayPhepCSDT { get; private set; } = new List<GiayPhepCSDT>();

        private CoSoDaoTao(Guid id, string ten, string diaChiViCSDT,
            Guid tkId, DateTime thoiGianTao, DateTime thoiGianCapNhat, DateTime thoiGianXoa)
        {
            Id = id;
            Ten = ten;
            DiaChiViCSDT = diaChiViCSDT;
            TKId = tkId;
            ThoiGianTao = thoiGianTao;
            ThoiGianCapNhat = thoiGianCapNhat;
            ThoiGianXoa = thoiGianXoa;
        }

        public static Result Create(string ten, string diaChiViCSDT, Guid tkId,
            List<GiayPhepCSDT> danhSachGiayPhepCSDT)
        {
            CoSoDaoTao csdt = new CoSoDaoTao(Guid.NewGuid(), ten, diaChiViCSDT, tkId,
                DateTime.UtcNow, DateTime.UtcNow, DateTime.MinValue);

            if(danhSachGiayPhepCSDT == null || danhSachGiayPhepCSDT.Count <= 0)
                return Result.Failure(CoSoDaoTaoError.ThieuThongTinGiayPhepCSDT);

            foreach(var giayPhep in danhSachGiayPhepCSDT)
            {
                csdt.DanhSachGiayPhepCSDT.Add(giayPhep);
            }

            return Result.Success();
        }

        public Result<SinhVien> TaoSinhVien(string ten, string cccd, string email, string diaChiViSinhVien, Guid tkId)
        {
            return SinhVien.Create(ten, cccd, email, diaChiViSinhVien, tkId);
        }

        public Result HuyLienKetSinhVien()
        {
            throw new NotImplementedException();
        }

        public Result<SinhVien> CapNhatThongTinSinhVien()
        {
            throw new NotImplementedException();
        }

        public Result<CoSoDaoTao> CapNhatThongTinCSDT(string ten, string diaChiViCSDT)
        {
            Ten = ten;
            DiaChiViCSDT = diaChiViCSDT;
            ThoiGianCapNhat = DateTime.UtcNow;
            return Result<CoSoDaoTao>.Success(this);
        }

        public Result TaoBangCapChoSinhVien(
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
            // Nếu là BangDiem thì phải có điểm, ngược lại không được có điểm
            if(loaiBangCap == LoaiBangCap.BangDiem)
            {
                if(diem == null)
                {
                    return Result.Failure(BangCapError.ThieuThuocTinhDiem);
                }

            }
            else
            {
                if(diem != null)
                {
                    return Result.Failure(BangCapError.ThuaThuocTinhDiem);
                }
            }

            if(ngayCap == ngayHetHan)
            {
                return Result.Failure(BangCapError.HanSuDungBangCapKhongHopLe);
            }
        }
    }
}
