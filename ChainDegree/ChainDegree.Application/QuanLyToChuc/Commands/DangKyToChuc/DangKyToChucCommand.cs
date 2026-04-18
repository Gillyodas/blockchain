using ChainDegree.Domain.QuanLyToChuc.Enums;
using ChainDegree.Domain.QuanLyToChuc.ValueObjects;
using ControlHub.SharedKernel.Results;
using MediatR;

namespace ChainDegree.Application.QuanLyToChuc.Commands.DangKyToChuc;

public sealed record DangKyToChucCommand(string TenToChuc, LoaiToChuc LoaiToChucDangKy, Guid TkId, string DiaChiVi) : IRequest<Result<Guid>>;
