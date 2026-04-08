using System;
using System.Collections.Generic;
using System.Text;
using ChainDegree.Application.QuanLyBangCap.Interfaces.Repositories;
using ChainDegree.Domain.QuanLyBangCap.Aggregates;
using MediatR;

namespace ChainDegree.Application.QuanLyBangCap.Commands.TaoBangCapChoSinhVienCommand;

public class TaoBangCapSinhVienCommandHandler : IRequestHandler<TaoBangCapSinhVienCommand, Guid>
{
    private readonly IBangCapRepository _bangCapRepository;
    public TaoBangCapSinhVienCommandHandler(IBangCapRepository bangCapRepository)
    {
        _bangCapRepository = bangCapRepository;
    }
    public Task<Guid> Handle(TaoBangCapSinhVienCommand request, CancellationToken cancellationToken)
    {
        CoSoDaoTao csdt = new CoSoDaoTao();
        BangCap bc = csdt.TaoBangCapChoSinhVien("Bằng tốt nghiệp đại học", 3.5, LoaiBangCap.DaiHoc, Guid.NewGuid(), null, null, DateTime.Now, null, request.SinhVienId, new List<BangCap>());

        _bangCapRepository.AddAsync(bc);

    }
}
