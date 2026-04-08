using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace ChainDegree.Application.QuanLyBangCap.Commands.TaoBangCapChoSinhVienCommand;

public sealed record TaoBangCapSinhVienCommand(
    Guid SinhVienId,
    Guid CoSoDaoTaoId
) : IRequest<Guid>;
