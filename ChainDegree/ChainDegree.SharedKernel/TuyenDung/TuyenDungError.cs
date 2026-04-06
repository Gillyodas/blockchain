using ControlHub.SharedKernel.Common.Errors;

namespace ChainDegree.SharedKernel.TuyenDung;

public static class TuyenDungError
{
    public static readonly Error DuongDanGiayPhepTrong = Error.Validation("TuyenDung.DuongDanGiayPhepTrong", "Đường dẫn lưu trữ giấy phép không được để trống.");
    public static readonly Error TenNhaTuyenDungTrong = Error.Validation("TuyenDung.TenNhaTuyenDungTrong", "Tên nhà tuyển dụng không được để trống.");
    public static readonly Error TinTuyenDungKhongTonTai = Error.NotFound("TuyenDung.TinTuyenDungKhongTonTai", "Tin tuyển dụng không tồn tại.");
    public static readonly Error GiayPhepKhongTonTai = Error.NotFound("TuyenDung.GiayPhepKhongTonTai", "Giấy phép không tồn tại.");
}
