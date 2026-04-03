using System;
using System.Collections.Generic;
using System.Text;
using ControlHub.SharedKernel.Common.Errors;

namespace ChainDegree.SharedKernel.QuanLyBangCap.BangCap;

public class BangCapError
{
    public static readonly Error DiemKhongHopLe =
        Error.Validation("BangCap.DiemKhongHopLe", "Điểm không hợp lệ, phải lớn hơn hoặc bằng 0");

    public static readonly Error HanSuDungBangCapKhongHopLe =
        Error.Validation("BangCap.HanSuDungBangCapKhongHopLe", "Hạn sử dụng bằng cấp không hợp lệ, phải lớn hơn ngày cấp");

    public static readonly Error ThieuThuocTinhDiem =
        Error.Validation("BangCap.ThieuThuocTinhDiem", "Thiếu thuộc tính điểm, điểm phải được cung cấp khi loại bằng cấp yêu cầu có điểm");

    public static readonly Error ThuaThuocTinhDiem =
        Error.Validation("BangCap.ThuaThuocTinhDiem", "Thừa thuộc tính điểm, điểm phải được bỏ qua khi loại bằng cấp yêu cầu không có điểm");

    public static readonly Error ThieuThongTinGiaoDichBangCap = 
        Error.Validation("BangCap.ThieuThongTinGiaoDichBangCap", "Thiếu thông tin giao dịch bằng cấp, mã băm xác thực, mã băm giao dịch và salt phải được cung cấp");

    public static readonly Error ThuaThongTinGiaoDichBangCap = 
        Error.Validation("BangCap.ThuaThongTinGiaoDichBangCap", "Thừa thông tin giao dịch bằng cấp, mã băm xác thực, mã băm giao dịch và salt phải được bỏ qua");

    public static readonly Error TrangThaiKhongHopLe = 
        Error.Validation("BangCap.TrangThaiKhongHopLe", "Trạng thái bằng cấp không hợp lệ");

    public static readonly Error BangCapDaDuocXacNhan = 
        Error.Validation("BangCap.BangCapDaDuocXacNhan", "Bằng cấp đã được xác nhận, không thể thực hiện hành động này");

     public static readonly Error BangCapDaDuocThuHoi = 
        Error.Validation("BangCap.BangCapDaDuocThuHoi", "Bằng cấp đã được thu hồi, không thể thực hiện hành động này");

    public static readonly Error BangCapDaDuocHuy = 
        Error.Validation("BangCap.BangCapDaDuocHuy", "Bằng cấp đã được hủy, không thể thực hiện hành động này");

    public static readonly Error BangCapChiDuocHuyBoiNguoiTao = 
        Error.Validation("BangCap.BangCapChiDuocHuyBoiNguoiTao", "Bằng cấp chỉ được hủy bởi người tạo, không thể thực hiện hành động này");

    public static readonly Error BangCapChiDuocThuHoiBoiNguoiTao = 
        Error.Validation("BangCap.BangCapChiDuocThuHoiBoiNguoiTao", "Bằng cấp chỉ được thu hồi bởi người tạo, không thể thực hiện hành động này");

    public static readonly Error BangCapChiDuocHuyKhiChuaXacNhan = 
        Error.Validation("BangCap.BangCapChiDuocHuyKhiChuaXacNhan", "Bằng cấp chỉ được hủy khi chưa xác nhận, không thể thực hiện hành động này");

    public static readonly Error BangCapChiDuocKhoiPhucBoiNguoiTao =
        Error.Validation("BangCap.BangCapChiDuocKhoiPhucBoiNguoiTao", "Bằng cấp chỉ được khôi phục bởi người tạo, không thể thực hiện hành động này");

    public static readonly Error BangCapChiDuocKhoiPhucKhiDaThuHoi =
        Error.Validation("BangCap.BangCapChiDuocKhoiPhucKhiDaThuHoi", "Bằng cấp chỉ được khôi phục khi đã thu hồi, không thể thực hiện hành động này");

    public static readonly Error GhiChuHuyKhongDuocBoTrongKhiLyDoHuyLaKhac =
        Error.Validation("BangCap.GhiChuHuyKhongDuocBoTrongKhiLyDoHuyLaKhac", "Ghi chú hủy không được bỏ trống khi lý do hủy là khác");

    public static readonly Error GhiChuKhoiPhucKhongDuocBoTrongKhiLyDoKhoiPhucLaKhac =
        Error.Validation("BangCap.GhiChuKhoiPhucKhongDuocBoTrongKhiLyDoKhoiPhucLaKhac", "Ghi chú khôi phục không được bỏ trống khi lý do khôi phục là khác");

    public static readonly Error GhiChuThuHoiKhongDuocBoTrongKhiLyDoThuHoiLaKhac =
        Error.Validation("BangCap.GhiChuThuHoiKhongDuocBoTrongKhiLyDoThuHoiLaKhac", "Ghi chú thu hồi không được bỏ trống khi lý do thu hồi là khác");
}
