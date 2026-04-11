using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using ChainDegree.Domain.QuanLyBangCap.Enums;
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
            .IsInEnum().WithMessage("Loại bằng cấp không hợp lệ.");

        RuleFor(x => x.TenBangCap)
            .NotEmpty().WithMessage("Tên bằng cấp không được để trống.")
            .MaximumLength(255).WithMessage("Tên bằng cấp không được vượt quá 255 ký tự.");

        RuleFor(x => x.LinhVucId)
            .NotEmpty().WithMessage("Lĩnh vực không được để trống.");

        RuleFor(x => x.NgayCap)
            .NotEmpty().WithMessage("Ngày cấp không được để trống.");

        RuleFor(x => x.NgayHetHan)
            .NotEmpty().WithMessage("Ngày hết hạn không được để trống.")
            .GreaterThan(x => x.NgayCap).WithMessage("Ngày hết hạn phải lớn hơn ngày cấp.");

        RuleFor(x => x.CoSoDaoTaoId)
            .NotEmpty().WithMessage("Cơ sở đào tạo không được để trống.");

        RuleFor(x => x.Diem)
            .NotEmpty().WithMessage("Bảng điểm thì bắt buộc phải có điểm.")
            .InclusiveBetween(0, 10).WithMessage("Điểm phải nằm trong khoảng từ 0 đến 10.")
            .When(x => x.LoaiBangCap == LoaiBangCap.BangDiem);

        RuleFor(x => x.Diem)
            .InclusiveBetween(0, 10).WithMessage("Điểm không hợp lệ.")
            .When(x => x.Diem.HasValue);

        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.FileBangCap) || !string.IsNullOrWhiteSpace(x.LinkBangCap))
            .When(x => x.LoaiBangCap != LoaiBangCap.BangDiem)
            .WithMessage("Bằng cấp phải có tệp đính kèm hoặc đường dẫn liên kết.");
    }
}
