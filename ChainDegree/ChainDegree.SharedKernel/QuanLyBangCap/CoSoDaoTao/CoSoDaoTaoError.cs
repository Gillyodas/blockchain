using System;
using System.Collections.Generic;
using System.Text;
using ControlHub.SharedKernel.Common.Errors;

namespace ChainDegree.SharedKernel.QuanLyBangCap.CoSoDaoTao
{
    public static class CoSoDaoTaoError
    {
        public static readonly Error TenSinhVienTrong =
            Error.Validation("CoSoDaoTao.TenSinhVienTrong", "Tên sinh viên không được để trống");

        public static readonly Error SaiDinhDangCCCD =
            Error.Validation("CoSoDaoTao.SaiDinhDangCCCD", "Sai định dạng CCCD");

        public static readonly Error SaiDinhDangEmail =
            Error.Validation("CoSoDaoTao.SaiDinhDangEmail", "Sai định dạng email");

        public static readonly Error ThieuThongTinGiayPhepCSDT =
            Error.Validation("CoSoDaoTao.ThieuThongTinGiayPhepCSDT",
                "Thiếu thông tin giấy phép cơ sở đào tạo, ít nhất phải có 2 loại giấy phép là Giấy phép hoạt động giáo dục và Quyết định thành lập trường");

        public static readonly Error ThieuGiayPhepHoatDongGiaoDuc =
            Error.Validation("CoSoDaoTao.ThieuGiayPhepHoatDongGiaoDuc", "Thiếu giấy phép hoạt động giáo dục");

        public static readonly Error ThieuQuyetDinhThanhLapTruong =
            Error.Validation("CoSoDaoTao.ThieuQuyetDinhThanhLapTruong", "Thiếu quyết định thành lập trường");

        public static readonly Error SinhVienDaCoBangCapCungLoaiVaLinhVuc =
            Error.Conflict("CoSoDaoTao.SinhVienDaCoBangCapCungLoaiVaLinhVuc", "Sinh viên đã có bằng cấp cùng loại và cùng lĩnh vực đang hiệu lực.");

        //Application errors
        public static readonly Error KhongTimThayCoSoDaoTao =
            Error.NotFound("CoSoDaoTao.KhongTimThayCoSoDaoTao", "Không tìm thấy cơ sở đào tạo");

        public static readonly Error ThatBaiKhiTimKiemCoSoDaoTao =
            Error.Failure("CoSoDaoTao.ThatBaiKhiTimKiemCoSoDaoTao", "Thất bại khi tìm kiếm cơ sở đào tạo");
    }
}
