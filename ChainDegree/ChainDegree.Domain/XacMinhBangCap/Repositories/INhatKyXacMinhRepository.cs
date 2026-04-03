using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChainDegree.Domain.XacMinhBangCap.Aggregates;

namespace ChainDegree.Domain.XacMinhBangCap.Repositories
{
    public interface INhatKyXacMinhRepository
    {
        Task ThemNhatKyAsync(NhatKyXacMinh nhatKy, CancellationToken cancellationToken = default);
        Task<IEnumerable<NhatKyXacMinh>> LayDanhSachTheoBangCapIdAsync(Guid bangCapId, CancellationToken cancellationToken = default);
    }
}
