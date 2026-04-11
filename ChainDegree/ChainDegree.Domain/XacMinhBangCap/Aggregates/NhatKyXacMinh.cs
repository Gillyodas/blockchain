using System;
using ChainDegree.Domain.XacMinhBangCap.Enums;
using ChainDegree.SharedKernel.XacMinhBangCap;
using ControlHub.SharedKernel.Results;

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

        private NhatKyXacMinh() { } 

        private NhatKyXacMinh(Guid bangCapId, string maBamXacMinh, KetQuaXacMinh ketQua, Guid? nhaTuyenDungId)
        {
            Id = Guid.NewGuid();
            BangCapId = bangCapId;
            MaBamXacMinh = maBamXacMinh;
            KetQuaXacMinh = ketQua;
            NhaTuyenDungId = nhaTuyenDungId;
            ThoiGianXacMinh = DateTime.UtcNow;
        }

        public static Result<NhatKyXacMinh> GhiNhan(Guid bangCapId, string maBamXacMinh, KetQuaXacMinh ketQua, Guid? nhaTuyenDungId = null)
        {
            if (bangCapId == Guid.Empty)
                return Result<NhatKyXacMinh>.Failure(XacMinhBangCapError.BangCapIdKhongHopLe);

            if (string.IsNullOrWhiteSpace(maBamXacMinh))
                return Result<NhatKyXacMinh>.Failure(XacMinhBangCapError.MaBamXacMinhTrong);

            return Result<NhatKyXacMinh>.Success(new NhatKyXacMinh(bangCapId, maBamXacMinh, ketQua, nhaTuyenDungId));
        }
    }
}
