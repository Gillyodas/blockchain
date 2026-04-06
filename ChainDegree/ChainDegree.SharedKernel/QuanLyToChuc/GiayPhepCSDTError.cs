using System;
using System.Collections.Generic;
using System.Text;
using ControlHub.SharedKernel.Common.Errors;

namespace ChainDegree.SharedKernel.QuanLyToChuc;

public static class GiayPhepCSDTError
{
    public static readonly Error GiayPhepCSDTHetHan =
        Error.Validation("QuanLyToChuc.GiayPhepCSDTHetHan", "Giấy phép đã hết hạn");

    public static readonly Error GiayPhepCSDTChuaDuocXacMinh =
        Error.Validation("QuanLyToChuc.GiayPhepCSDTChuaDuocXacMinh", "Giấy phép chưa được xác minh");

    public static readonly Error GiayPhepCSDTKhongHopLe =
        Error.Validation("QuanLyToChuc.GiayPhepCSDTKhongHopLe", "Giấy phép không hợp lệ");

    public static readonly Error NhaCungCapChuKySoKhongDuocTinTuong =
        Error.Validation("QuanLyToChuc.NhaCungCapChuKySoKhongDuocTinTuong", "Nhà cung cấp chữ ký số không được tin tưởng");

    public static readonly Error ChiDuocTaiLenLaiKhiBiTuChoi =
        Error.Validation("QuanLyToChuc.ChiDuocTaiLenLaiKhiBiTuChoi", "Chỉ được tải lên lại giấy phép khi trạng thái là Từ chối");
}
