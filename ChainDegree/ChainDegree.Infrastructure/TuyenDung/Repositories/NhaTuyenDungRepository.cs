using System;
using System.Collections.Generic;
using System.Text;
using ChainDegree.Application.QuanLyToChuc.Interfaces.Repositories;
using ChainDegree.Domain.TuyenDung.Aggregates;

namespace ChainDegree.Infrastructure.TuyenDung.Repositories;

public class NhaTuyenDungRepository : INhaTuyenDungRepository
{
    public Task AddAsync(NhaTuyenDung nhaTuyenDung, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
