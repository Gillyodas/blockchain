using ChainDegree.API.QuanLyBangCap.Request;
using ChainDegree.Application.QuanLyBangCap.Commands.TaoBangCapChoSinhVien;
using ControlHub.API.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ChainDegree.API.QuanLyBangCap.Controllers;

[ApiController]
[Route("api/co-so-dao-tao")]
[EnableRateLimiting(RateLimitingExtensions.Policies.GeneralApi)]
public class QuanLyBangCapController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public QuanLyBangCapController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("{csdt_id}/bang-cap")]
    public async Task<IActionResult> TaoBangCapSinhVien(Guid csdt_id, TaoBangCapSinhVienRequest request, CancellationToken cancellationToken)
    {
        var command = new TaoBangCapSinhVienCommand
        (
            request.SinhVienId,
            request.CCCD,
            request.TenSinhVien,
            request.LoaiBangCap,
            request.TenBangCap,
            request.LinhVucId,
            request.Diem,
            request.NgayCap,
            request.NgayHetHan,
            request.FileBangCap,
            request.LinkBangCap,
            csdt_id
        );

        var result = await _mediator.Send(command, cancellationToken);

        if(result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result);
    }
}
