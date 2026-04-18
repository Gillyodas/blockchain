using System;
using System.Collections.Generic;
using System.Text;
using ChainDegree.Application.QuanLyToChuc.Commands.DangKyToChuc;
using ChainDegree.Domain.QuanLyBangCap.Enums;
using ChainDegree.Domain.QuanLyToChuc.Enums;
using FluentValidation;

namespace ChainDegree.Application.QuanLyToChuc.Commands.DangKyToChuc;

public class DangKyToChucCommandValidator : AbstractValidator<DangKyToChucCommand>
{
    public DangKyToChucCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.TenToChuc)
            .NotEmpty().WithMessage("Tên tổ chức không được để trống.")
            .MaximumLength(255).WithMessage("Tên tổ chức không được vượt quá 255 ký tự.");

        RuleFor(x => x.TkId)
            .NotEmpty().WithMessage("Tài khoản id không được để trống.");

        RuleFor(x => x.LoaiToChucDangKy)
            .IsInEnum().WithMessage("Loại tổ chức đăng ký không hợp lệ.");

        RuleFor(x => x.DiaChiVi)
            .NotEmpty().WithMessage("Địa chỉ ví không được để trống.")
            .MaximumLength(255).WithMessage("Địa chỉ ví không được vượt quá 255 ký tự.");

        //When(x => x.LoaiToChucDangKy == LoaiToChuc.Issuer, () => {
        //    RuleFor(x => x.DanhSachGiayPhepCSDT)
        //        .NotEmpty().WithMessage("Danh sách giấy phép cơ sở đào tạo không được để trống.")
        //        .Must(list => list.Any(gp => gp.LoaiGiayPhep == LoaiGiayPhepCSDT.GiayPhepHoatDongGiaoDuc))
        //        .WithMessage("Thiếu Giấy phép hoạt động giáo dục.")
        //        .Must(list => list.Any(gp => gp.LoaiGiayPhep == LoaiGiayPhepCSDT.QuyetDinhThanhLapTruong))
        //        .WithMessage("Thiếu Quyết định thành lập trường.");
        //});

        //When(x => x.LoaiToChucDangKy == LoaiToChuc.Verifier, () => {
        //    RuleFor(x => x.DanhSachGiayPhepNhaTuyenDung)
        //        .NotEmpty().WithMessage("Danh sách giấy phép nhà tuyển dụng không được để trống.")
        //        .Must(list => list.Any(gp => gp.LoaiGiayPhep == LoaiGiayPhepNTD.GiayPhepDangKyKinhDoanh))
        //        .WithMessage("Nhà tuyển dụng phải có Giấy phép đăng ký kinh doanh.");
        //});
    }
}
