using System.Threading;
using System.Threading.Tasks;

namespace ChainDegree.Domain.XacMinhBangCap.Services
{
    public class KetQuaTruyVanBlockchain
    {
        public bool TonTai { get; set; }
        public bool BiThuHoi { get; set; }
        public string DiaChiNhaPhatHanh { get; set; }
        public long ThoiGianGhiNhan { get; set; }
    }

    public interface IBlockchainVerifier
    {
        // Truy vấn trực tiếp Smart Contract
        Task<KetQuaTruyVanBlockchain> KiemTraBangCapOnChainAsync(string chuoiBamXacThuc, CancellationToken cancellationToken = default);
    }
}
