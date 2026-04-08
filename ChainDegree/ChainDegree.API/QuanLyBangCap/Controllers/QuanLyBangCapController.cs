using ChainDegree.Application.QuanLyBangCap.Commands.TaoBangCapChoSinhVienCommand;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChainDegree.API.QuanLyBangCap.Controllers;

public class QuanLyBangCapController : ControllerBase
{
    public async Task<IActionResult> TaoBangCapChoSinhVien(Guid sinhVienId, Guid CSDTId)
    {
        var command = new TaoBangCapChoSinhVienCommand
        {
            SinhVienId = sinhVienId,
            CoSoDaoTaoId = CSDTId
        };

        var result = await Mediator.Send(command);
    }
}
