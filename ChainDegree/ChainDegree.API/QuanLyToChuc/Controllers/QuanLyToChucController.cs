using ChainDegree.API.QuanLyToChuc.Request;
using ChainDegree.Application.QuanLyToChuc.Commands.DangKyToChuc;
using ChainDegree.Application.QuanLyToChuc.Commands.DuyetDangKyCSDT;
using ChainDegree.Application.QuanLyToChuc.Commands.DuyetDangKyNTD;
using ControlHub.API.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ChainDegree.API.QuanLyToChuc.Controllers;

[ApiController]
[Route("api/to-chuc")]
[EnableRateLimiting(RateLimitingExtensions.Policies.GeneralApi)]
public class QuanLyToChucController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public QuanLyToChucController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("{tk_id}/dang-ky")]
    public async Task<IActionResult> DangKyToChuc(Guid tk_id, DangKyToChucRequest request, CancellationToken cancellationToken)
    {
        var command = new DangKyToChucCommand
        (
            request.TenToChuc,
            request.LoaiToChucDangKy,
            request.DiaChiVi,
            tk_id
        );
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        return Ok(result);
    }

    [HttpPut("{yeuCauDangKyCSDTId}/duyet")]
    public async Task<IActionResult> DuyetDangKyCSDT(Guid yeuCauDangKyCSDTId, DuyetDangKyCSDTRequest request, CancellationToken cancellationToken)
    {
        var command = new DuyetDangKyCSDTCommand
        (
            yeuCauDangKyCSDTId,
            request.GhiChu
        );
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        return Ok(result);
    }

    [HttpPut("{yeuCauDangKyNTDId}/tu-choi")]
    public async Task<IActionResult> DuyetDangKyNTD(Guid yeuCauDangKyNTDId, DuyetDangKyNTDRequest request, CancellationToken cancellationToken)
    {
        var command = new DuyetDangKyNTDCommand
        (
            yeuCauDangKyNTDId,
            request.GhiChu
        );
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        return Ok(result);
    }
}
