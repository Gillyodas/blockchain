using System;
using System.Collections.Generic;
using System.Text;
using ControlHub.SharedKernel.Common.Errors;

namespace ChainDegree.SharedKernel.QuanLyBangCap.SinhVien;

public class SinhVienError
{
    //Application errors
    public static readonly Error KhongTimThaySinhVien =
        Error.NotFound("BangCap.KhongTimThaySinhVien", "Không tìm thấy sinh viên để tạo bằng cấp");
}
