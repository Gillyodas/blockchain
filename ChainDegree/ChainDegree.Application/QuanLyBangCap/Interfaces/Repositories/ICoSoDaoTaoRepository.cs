using System;
using System.Collections.Generic;
using System.Text;
using ChainDegree.Domain.QuanLyBangCap.Aggregates;
using ControlHub.SharedKernel.Results;

namespace ChainDegree.Application.QuanLyBangCap.Interfaces.Repositories;

public interface ICoSoDaoTaoRepository
{
    Task<Result<CoSoDaoTao>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Result<bool>> KiemTraTrungBangCapCapBoiCSDTBySinhVienIdAsync(Guid sinhVienId, CancellationToken cancellationToken);
}
