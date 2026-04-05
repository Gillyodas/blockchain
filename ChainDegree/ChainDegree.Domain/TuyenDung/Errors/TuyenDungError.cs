using ControlHub.SharedKernel.Common.Errors;

namespace ChainDegree.Domain.TuyenDung.Errors;

public class TuyenDungError
{
    // Nhà Tuyển Dụng
    public static readonly Error TenNhaTuyenDungTrong = Error.Validation("NhaTuyenDung.TenTrong", "Tên nhà tuyển dụng không được để trống.");
    public static readonly Error DiaChiNhaTuyenDungTrong = Error.Validation("NhaTuyenDung.DiaChiTrong", "Địa chỉ nhà tuyển dụng không được để trống.");
    public static readonly Error EmailNhaTuyenDungTrong = Error.Validation("NhaTuyenDung.EmailTrong", "Email nhà tuyển dụng không được để trống.");
    public static readonly Error SoDienThoaiNhaTuyenDungTrong = Error.Validation("NhaTuyenDung.SoDienThoaiTrong", "Số điện thoại nhà tuyển dụng không được để trống.");
    public static readonly Error TinTuyenDungKhongTonTai = Error.NotFound("NhaTuyenDung.TinTuyenDungKhongTonTai", "Tin tuyển dụng không tồn tại.");
    public static readonly Error GiayPhepKhongTonTai = Error.NotFound("NhaTuyenDung.GiayPhepKhongTonTai", "Giấy phép không tồn tại.");

    // Thông Tin Tuyển Dụng
    public static readonly Error ViTriKhongDuocTrong = Error.Validation("ThongTinTuyenDung.ViTriTrong", "Vị trí tuyển dụng không được để trống.");
    public static readonly Error MoTaKhongDuocTrong = Error.Validation("ThongTinTuyenDung.MoTaTrong", "Mô tả tuyển dụng không được để trống.");
    public static readonly Error HanUngTuyenKhongHopLe = Error.Validation("ThongTinTuyenDung.HanUngTuyenKhongHopLe", "Hạn ứng tuyển phải lớn hơn thời gian hiện tại.");

    // Hồ Sơ Ứng Tuyển
    public static readonly Error TinTuyenDungKhongHopLe = Error.Validation("HoSoUngTuyen.TinTuyenDungKhongHopLe", "Thông tin tuyển dụng không hợp lệ.");
    public static readonly Error SinhVienKhongHopLe = Error.Validation("HoSoUngTuyen.SinhVienKhongHopLe", "Thông tin sinh viên không hợp lệ.");
    public static readonly Error HoSoDaDuocXem = Error.Conflict("HoSoUngTuyen.HoSoDaDuocXem", "Hồ sơ đã được xử lý, không thể thay đổi thêm bằng cấp.");

    // Kết Quả Phân Tích
    public static readonly Error PhanTramPhuHopKhongHopLe = Error.Validation("KetQuaPhanTich.PhanTramKhongHopLe", "Phần trăm phù hợp phải từ 0 đến 100.");
    public static readonly Error KetLuanKhongDuocTrong = Error.Validation("KetQuaPhanTich.KetLuanTrong", "Kết luận phân tích không được để trống.");

    // Giấy Phép
    public static readonly Error DuongDanTrong = Error.Validation("GiayPhep.DuongDanTrong", "Đường dẫn tài liệu không được để trống.");
    
    // Bằng cấp
    public static readonly Error BangCapKhongHopLe = Error.Validation("BangCap.KhongHopLe", "Thông tin bằng cấp không hợp lệ.");
}
