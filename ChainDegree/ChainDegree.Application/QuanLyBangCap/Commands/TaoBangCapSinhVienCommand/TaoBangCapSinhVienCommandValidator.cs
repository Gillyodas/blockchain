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
        RuleFor(x => x)
            .Must(model =>
                !((model.SinhVienId == null || model.SinhVienId == Guid.Empty) &&
                  string.IsNullOrWhiteSpace(model.CCCD))
            )
            .WithMessage("SinhVienId và CCCD không được để trống cùng lúc.");

        RuleFor(x => x.TenSinhVien)
            .NotEmpty().WithMessage("Tên sinh viên không được để trống.")
            .MaximumLength(255).WithMessage("Tên sinh viên không được vượt quá 255 ký tự.");

        RuleFor(x => x.LoaiBangCap)
            .NotEmpty().WithMessage("Loại bằng cấp không được để trống.")
            .Must(x => new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 99 }.Contains(x)).WithMessage("Loại bằng cấp không hợp lệ.");

        RuleFor(x => x.LinhVucId)
            .NotEmpty().WithMessage("Lĩnh vực không được để trống.");

        RuleFor(x => x.NgayCap)
            .NotEmpty().WithMessage("Ngày cấp không được để trống.");

        RuleFor(x => x.NgayHetHan)
            .NotEmpty().WithMessage("Ngày hết hạn không được để trống.")
            .GreaterThan(x => x.NgayCap).WithMessage("Ngày hết hạn phải lớn hơn ngày cấp.");

        RuleFor(x => x.CoSoDaoTaoId)
            .NotEmpty().WithMessage("Cơ sở đào tạo không được để trống.");


    }
}
