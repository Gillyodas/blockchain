using ChainDegree.API.QuanLyBangCap.Request;
using ChainDegree.Application.QuanLyBangCap.Commands.TaoBangCapChoSinhVienCommand;
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
    private readonly ILogger<QuanLyBangCapController> _logger;

    public QuanLyBangCapController(IMediator mediator, ILogger<QuanLyBangCapController> logger)
    {
        _mediator = mediator;
        _logger = logger;
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
            request.LinhVucId,
            request.Diem,
            request.NgayCap,
            request.NgayHetHan,
            request.FileBangCap,
            request.LinkBangCap,
            csdt_id
        );

        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }
}
