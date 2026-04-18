using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace ChainDegree.Application.QuanLyToChuc.Commands.DuyetDangKyCSDT;

public class DuyetDangKyCSDTCommandValidator : AbstractValidator<DuyetDangKyCSDTCommand>
{
    public DuyetDangKyCSDTCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.YeuCauDangKyId)
            .NotEmpty()
            .WithMessage("ID yêu cầu đăng ký Cơ sở đào tạo không được để trống.");

        RuleFor(x => x.GhiChu)
            .MaximumLength(500)
            .WithMessage("Ghi chú duyệt không được vượt quá 500 ký tự.");
    }
}
