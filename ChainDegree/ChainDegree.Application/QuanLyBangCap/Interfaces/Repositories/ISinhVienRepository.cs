using System;
using System.Collections.Generic;
using System.Text;
using ChainDegree.Domain.QuanLyBangCap.Entities;
using ControlHub.SharedKernel.Results;

namespace ChainDegree.Application.QuanLyBangCap.Interfaces.Repositories;

public interface ISinhVienRepository
{
    Task<SinhVien?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<SinhVien?> GetByCCCDAsync(string cccd, CancellationToken cancellationToken);
}
