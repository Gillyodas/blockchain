using ChainDegree.Domain.QuanLyBangCap.Entities;
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

        private CoSoDaoTao(Guid id, string ten, string diaChiViCSDT, Guid tkId, DateTime thoiGianTao, DateTime thoiGianCapNhat, DateTime thoiGianXoa)
        {
            Id = id;
            Ten = ten;
            DiaChiViCSDT = diaChiViCSDT;
            TKId = tkId;
            ThoiGianTao = thoiGianTao;
            ThoiGianCapNhat = thoiGianCapNhat;
            ThoiGianXoa = thoiGianXoa;
        }

        public static CoSoDaoTao Create(string ten, string diaChiViCSDT, Guid tkId)
        {
            return new CoSoDaoTao(Guid.NewGuid(), ten, diaChiViCSDT, tkId, DateTime.UtcNow, DateTime.UtcNow, DateTime.MinValue);
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

        public Result<BangCap> TaoBangCapChoSinhVien()
        {
            Account acc = 
        }
    }
}
