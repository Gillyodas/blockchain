using System.Threading.Tasks;
using ChainDegree.Domain.TuyenDung.Entities;
using ControlHub.SharedKernel.Results;

namespace ChainDegree.Domain.TuyenDung.Services;

public interface IAiAnalysisService
{
    Task<Result<KetQuaPhanTich>> AnalyzeApplicationAsync(HoSoUngTuyen application, ThongTinTuyenDung jobPosting);
}
