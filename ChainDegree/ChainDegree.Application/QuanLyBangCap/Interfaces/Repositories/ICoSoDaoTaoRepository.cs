using System;
using System.Collections.Generic;
using System.Text;
using ChainDegree.Domain.QuanLyBangCap.Aggregates;
using ChainDegree.Domain.QuanLyBangCap.Enums;
using ControlHub.SharedKernel.Results;

namespace ChainDegree.Application.QuanLyBangCap.Interfaces.Repositories;

public interface ICoSoDaoTaoRepository
{
    Task<CoSoDaoTao?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> TrungLapKhiCapBangChoSinhVien(Guid csdtId,Guid sinhVienId, LoaiBangCap loaiBangCap, Guid linhVucId, CancellationToken cancellationToken);
}
