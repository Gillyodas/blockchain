using System;
using System.Collections.Generic;
using System.Text;
using ChainDegree.Domain.QuanLyBangCap.Enums;
using ControlHub.SharedKernel.Results;
using MediatR;

namespace ChainDegree.Application.QuanLyBangCap.Commands.TaoBangCapChoSinhVien;

public sealed record TaoBangCapSinhVienCommand(
    Guid? SinhVienId,
    string? CCCD,
    string TenSinhVien,
    LoaiBangCap LoaiBangCap,
    string TenBangCap,
    Guid LinhVucId,
    double? Diem,
    DateTime NgayCap,
    DateTime NgayHetHan,
    string? FileBangCap,
    string? LinkBangCap,
    Guid CoSoDaoTaoId
) : IRequest<Result<TaoBangCapSinhVienCommandResult>>;

public sealed record TaoBangCapSinhVienCommandResult(Guid BangCapId, TrangThaiBangCap TrangThaiBangCapHienTai, TrangThaiBlockchain TrangThaiBlockchainHienTai, string MaBangXacThuc);
