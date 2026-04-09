using System;
using System.Collections.Generic;
using System.Text;
using ChainDegree.Domain.QuanLyBangCap.Enums;
using ControlHub.SharedKernel.Results;
using MediatR;

namespace ChainDegree.Application.QuanLyBangCap.Commands.TaoBangCapChoSinhVienCommand;

public sealed record TaoBangCapSinhVienCommand(
    Guid? SinhVienId,
    string? CCCD,
    string TenSinhVien,
    int LoaiBangCap,
    Guid LinhVucId,
    double? Diem,
    DateTime NgayCap,
    DateTime NgayHetHan,
    string? FileBangCap,
    string? LinkBangCap,
    Guid CoSoDaoTaoId
) : IRequest<Result<TaoBangCapSinhVienCommandResult>>;

public sealed record TaoBangCapSinhVienCommandResult(Guid BangCapId, int TrangThaiBangCapHienTai, int TrangThaiBlockchainHienTai, string MaBangXacThuc);
