using System.Threading;
using System.Threading.Tasks;

namespace ChainDegree.Domain.XacMinhBangCap.Services;

public interface IBlockchainVerifier
{
    Task<KetQuaTruyVanBlockchain> KiemTraBangCapOnChainAsync(string chuoiBamXacThuc, CancellationToken cancellationToken = default);
}
