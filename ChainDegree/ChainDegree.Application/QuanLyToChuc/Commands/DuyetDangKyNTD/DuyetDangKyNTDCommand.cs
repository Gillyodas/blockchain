using System;
using System.Collections.Generic;
using System.Text;
using ControlHub.SharedKernel.Results;
using MediatR;

namespace ChainDegree.Application.QuanLyToChuc.Commands.DuyetDangKyNTD;

public sealed record DuyetDangKyNTDCommand(Guid YeuCauDangKyId, string? GhiChu) : IRequest<Result>;
