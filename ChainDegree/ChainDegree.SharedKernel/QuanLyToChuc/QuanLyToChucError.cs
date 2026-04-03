using ControlHub.SharedKernel.Common.Errors;

namespace ChainDegree.SharedKernel.QuanLyToChuc;

public static class QuanLyToChucError
{
    public static readonly Error TenToChucTrong = Error.Validation("QuanLyToChuc.TenToChucTrong", "Tên tổ chức không được để trống");
    public static readonly Error FileUrlTrong = Error.Validation("QuanLyToChuc.FileUrlTrong", "Đường dẫn file không được để trống");
    public static readonly Error DuongDanFileTrong = Error.Validation("QuanLyToChuc.DuongDanFileTrong", "Đường dẫn file không được để trống"); // Để tương thích với code cũ
    public static readonly Error HoSoDaGuiKhongDuocSua = Error.Validation("QuanLyToChuc.HoSoDaGuiKhongDuocSua", "Hồ sơ đã gửi hoặc đã được duyệt không được phép chỉnh sửa");
    public static readonly Error SaiLoaiGiayPhep = Error.Validation("QuanLyToChuc.SaiLoaiGiayPhep", "Loại giấy phép không phù hợp với loại tổ chức");
    public static readonly Error HoSoKhongPhaiBanNhap = Error.Validation("QuanLyToChuc.HoSoKhongPhaiBanNhap", "Chỉ có thể nộp hồ sơ khi đang ở trạng thái bản nháp");
    public static readonly Error ThieuGiayPhepBatBuocCSDT = Error.Validation("QuanLyToChuc.ThieuGiayPhepBatBuocCSDT", "Thiếu Giấy phép hoạt động hoặc Quyết định thành lập trường");
    public static readonly Error ThieuGiayPhepBatBuocNTD = Error.Validation("QuanLyToChuc.ThieuGiayPhepBatBuocNTD", "Thiếu Giấy phép đăng ký kinh doanh");
    public static readonly Error HoSoKhongTheXetDuyet = Error.Validation("QuanLyToChuc.HoSoKhongTheXetDuyet", "Chỉ có thể xét duyệt hồ sơ đang chờ duyệt");
    public static readonly Error GhiChuTuChoiBatBuoc = Error.Validation("QuanLyToChuc.GhiChuTuChoiBatBuoc", "Ghi chú là bắt buộc khi chọn lý do từ chối khác");
}
