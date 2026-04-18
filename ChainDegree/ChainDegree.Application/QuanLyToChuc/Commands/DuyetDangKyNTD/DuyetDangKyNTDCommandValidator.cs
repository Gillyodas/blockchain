using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace ChainDegree.Application.QuanLyToChuc.Commands.DuyetDangKyNTD;

public class DuyetDangKyNTDCommandValidator : AbstractValidator<DuyetDangKyNTDCommand>
{
    public DuyetDangKyNTDCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.YeuCauDangKyId)
            .NotEmpty()
            .WithMessage("ID yêu cầu đăng ký Nhà tuyển dụng không được để trống.");

        RuleFor(x => x.GhiChu)
            .MaximumLength(500)
            .WithMessage("Ghi chú duyệt không được vượt quá 500 ký tự.");
    }
}
