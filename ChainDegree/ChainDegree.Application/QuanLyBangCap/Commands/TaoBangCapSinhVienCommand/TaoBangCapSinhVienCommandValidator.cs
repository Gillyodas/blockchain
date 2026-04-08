using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using FluentValidation;

namespace ChainDegree.Application.QuanLyBangCap.Commands.TaoBangCapChoSinhVienCommand;

public class TaoBangCapSinhVienCommandValidator : AbstractValidator<TaoBangCapSinhVienCommand>
{
    public TaoBangCapSinhVienCommandValidator()
    {
        RuleFor(x => x.SinhVienId)
            .NotEmpty().WithMessage("SinhVienId không được để trống.");
    }
}
