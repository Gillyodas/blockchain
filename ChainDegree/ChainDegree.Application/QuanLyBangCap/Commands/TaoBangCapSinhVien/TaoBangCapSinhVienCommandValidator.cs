using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using ChainDegree.Domain.QuanLyBangCap.Enums;
using FluentValidation;

namespace ChainDegree.Application.QuanLyBangCap.Commands.TaoBangCapChoSinhVien;

public class TaoBangCapSinhVienCommandValidator : AbstractValidator<TaoBangCapSinhVienCommand>
{
    public TaoBangCapSinhVienCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x)
            .Must(model => (model.SinhVienId.HasValue && model.SinhVienId != Guid.Empty) ||
                          !string.IsNullOrWhiteSpace(model.CCCD))
            .WithMessage("Phải cung cấp SinhVienId hoặc CCCD để định danh sinh viên.");

        RuleFor(x => x.TenSinhVien)
            .NotEmpty().WithMessage("Tên sinh viên không được để trống.")
            .MaximumLength(255).WithMessage("Tên sinh viên không được vượt quá 255 ký tự.");

        RuleFor(x => x.TenBangCap)
            .NotEmpty().WithMessage("Tên bằng cấp không được để trống.")
            .MaximumLength(255).WithMessage("Tên bằng cấp không được vượt quá 255 ký tự.");

        RuleFor(x => x.LoaiBangCap)
            .IsInEnum().WithMessage("Loại bằng cấp không hợp lệ.");

        RuleFor(x => x.LinhVucId)
            .NotEmpty().WithMessage("Vui lòng chọn lĩnh vực đào tạo.");

        RuleFor(x => x.NgayCap)
            .NotEqual(default(DateTime)).WithMessage("Ngày cấp không hợp lệ.");

        RuleFor(x => x.NgayHetHan)
            .NotEqual(default(DateTime)).WithMessage("Ngày hết hạn không hợp lệ.")
            .GreaterThan(x => x.NgayCap).WithMessage("Ngày hết hạn phải sau ngày cấp.");

        When(x => x.LoaiBangCap == LoaiBangCap.BangDiem, () => {
            RuleFor(x => x.Diem)
                .NotNull().WithMessage("Bảng điểm bắt buộc phải có điểm số.")
                .InclusiveBetween(0, 10).WithMessage("Điểm phải nằm trong khoảng từ 0 đến 10.");
        });

        When(x => x.LoaiBangCap != LoaiBangCap.BangDiem, () => {
            RuleFor(x => x)
                .Must(x => !string.IsNullOrWhiteSpace(x.FileBangCap) || !string.IsNullOrWhiteSpace(x.LinkBangCap))
                .WithMessage("Bằng cấp phải có minh chứng (Tệp đính kèm hoặc Đường dẫn).");

            RuleFor(x => x.Diem)
                .InclusiveBetween(0, 10).WithMessage("Điểm không hợp lệ.")
                .When(x => x.Diem.HasValue);
        });

        RuleFor(x => x.CoSoDaoTaoId)
            .NotEmpty().WithMessage("Thiếu thông tin cơ sở đào tạo.");
    }
}
