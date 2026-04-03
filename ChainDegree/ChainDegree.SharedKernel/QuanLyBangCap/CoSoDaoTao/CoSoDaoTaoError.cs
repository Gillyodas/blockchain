using System;
using System.Collections.Generic;
using System.Text;
using ControlHub.SharedKernel.Common.Errors;

namespace ChainDegree.SharedKernel.QuanLyBangCap.CoSoDaoTao
{
    public class CoSoDaoTaoError
    {
        public static readonly Error SaiDinhDangCCCD =
            Error.Validation("CoSoDaoTao.SaiDinhDangCCCD", "Sai định dạng CCCD");

        public static readonly Error SaiDinhDangEmail =
            Error.Validation("CoSoDaoTao.SaiDinhDangEmail", "Sai định dạng email");

        public static readonly Error ThieuThongTinGiayPhepCSDT =
            Error.Validation("CoSoDaoTao.ThieuThongTinGiayPhepCSDT", "Thiếu thông tin giấy phép cơ sở đào tạo, danh sách giấy phép cơ sở đào tạo không được bỏ trống");
    }
}
