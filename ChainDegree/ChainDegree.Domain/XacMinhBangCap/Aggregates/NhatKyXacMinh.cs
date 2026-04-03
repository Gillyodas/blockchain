using System;
using ChainDegree.Domain.XacMinhBangCap.Enums;

namespace ChainDegree.Domain.XacMinhBangCap.Aggregates
{
    public class NhatKyXacMinh
    {
        public Guid Id { get; private set; }
        public Guid BangCapId { get; private set; }
        public Guid? NhaTuyenDungId { get; private set; }
        public DateTime ThoiGianXacMinh { get; private set; }
        public string MaBamXacMinh { get; private set; }
        public KetQuaXacMinh KetQuaXacMinh { get; private set; }

        protected NhatKyXacMinh() { }

        private NhatKyXacMinh(Guid bangCapId, string maBamXacMinh, KetQuaXacMinh ketQua, Guid? nhaTuyenDungId)
        {
            if (bangCapId == Guid.Empty)
                throw new ArgumentException("BangCapId không được để trống.");
            if (string.IsNullOrWhiteSpace(maBamXacMinh))
                throw new ArgumentException("MaBamXacMinh không được để trống.");

            Id = Guid.NewGuid();
            BangCapId = bangCapId;
            MaBamXacMinh = maBamXacMinh;
            KetQuaXacMinh = ketQua;
            NhaTuyenDungId = nhaTuyenDungId;
            ThoiGianXacMinh = DateTime.UtcNow; 
        }

        public static NhatKyXacMinh GhiNhan(Guid bangCapId, string maBamXacMinh, KetQuaXacMinh ketQua, Guid? nhaTuyenDungId = null)
        {
            return new NhatKyXacMinh(bangCapId, maBamXacMinh, ketQua, nhaTuyenDungId);
        }
    }
}
