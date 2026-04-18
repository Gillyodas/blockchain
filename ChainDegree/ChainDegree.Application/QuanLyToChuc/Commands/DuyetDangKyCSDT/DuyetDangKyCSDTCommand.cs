using System;
using System.Collections.Generic;
using System.Text;
using ControlHub.SharedKernel.Results;
using MediatR;

namespace ChainDegree.Application.QuanLyToChuc.Commands.DuyetDangKyCSDT;

public sealed record DuyetDangKyCSDTCommand(Guid YeuCauDangKyId, string? GhiChu) : IRequest<Result>;
