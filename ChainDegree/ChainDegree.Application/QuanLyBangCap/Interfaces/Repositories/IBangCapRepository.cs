using System;
using System.Collections.Generic;
using System.Text;
using ChainDegree.Domain.QuanLyBangCap.Entities;
using ControlHub.SharedKernel.Results;

namespace ChainDegree.Application.QuanLyBangCap.Interfaces.Repositories;

public interface IBangCapRepository
{
    public Task AddAsync(BangCap bangCap, CancellationToken cancellationToken = default);
}
