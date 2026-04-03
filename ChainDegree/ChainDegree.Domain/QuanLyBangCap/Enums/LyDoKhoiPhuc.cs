using System;
using System.Collections.Generic;
using System.Text;

namespace ChainDegree.Domain.QuanLyBangCap.Enums;

public enum LyDoKhoiPhuc
{
    SuaLoiNhapLieu = 0,
    SinhVienKhongConViPham = 1,
    /// <summary>Quá thời gian cấm, SV được phục hồi</summary>
    DaoHanPhucHoi = 2,
    XacNhanKhongGianLan = 3,
    QuyDinhDuocCapNhat = 4,
    QuyetDinhPhapLyBiHuy = 5,
    Khac = 99
}
