using System;
using System.Collections.Generic;
using System.Text;
using ChainDegree.Domain.TuyenDung.Aggregates;

namespace ChainDegree.Application.QuanLyToChuc.Interfaces.Repositories;

public interface INhaTuyenDungRepository
{
    Task AddAsync(NhaTuyenDung nhaTuyenDung, CancellationToken cancellationToken);
}
