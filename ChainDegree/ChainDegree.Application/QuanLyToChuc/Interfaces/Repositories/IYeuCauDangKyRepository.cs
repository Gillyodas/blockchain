using System;
using System.Collections.Generic;
using System.Text;
using ChainDegree.Domain.QuanLyToChuc.Aggregates;

namespace ChainDegree.Application.QuanLyToChuc.Interfaces.Repositories;

public interface IYeuCauDangKyRepository
{
    Task AddAsync(YeuCauDangKy yeuCauDangKy, CancellationToken cancellationToken);
    Task<bool> ExistsByDiaChiViAsync(string diaChiVi, CancellationToken cancellationToken);
    Task<YeuCauDangKy?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
