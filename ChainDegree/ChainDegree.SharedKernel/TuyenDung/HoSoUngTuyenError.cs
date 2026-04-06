using ControlHub.SharedKernel.Common.Errors;

namespace ChainDegree.SharedKernel.TuyenDung;

public static class HoSoUngTuyenError
{
    public static readonly Error TinTuyenDungKhongHopLe = Error.Validation("HoSoUngTuyen.TinTuyenDungKhongHopLe", "ID tin tuyển dụng không hợp lệ.");
    public static readonly Error SinhVienKhongHopLe = Error.Validation("HoSoUngTuyen.SinhVienKhongHopLe", "ID sinh viên không hợp lệ.");
    public static readonly Error HoSoDaDuocXem = Error.Validation("HoSoUngTuyen.HoSoDaDuocXem", "Hồ sơ đã được xem, không thể chỉnh sửa.");
    public static readonly Error HoSoKhongTheThuhoi = Error.Validation("HoSoUngTuyen.HoSoKhongTheThuhoi", "Chỉ có thể thu hồi hồ sơ khi đang ở trạng thái Chờ xem.");
}
