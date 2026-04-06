using ControlHub.SharedKernel.Common.Errors;

namespace ChainDegree.SharedKernel.TuyenDung;

public static class ThongTinTuyenDungError
{
    public static readonly Error ViTriKhongDuocTrong = Error.Validation("TuyenDung.ViTriKhongDuocTrong", "Vị trí tuyển dụng không được để trống.");
    public static readonly Error MoTaKhongDuocTrong = Error.Validation("TuyenDung.MoTaKhongDuocTrong", "Mô tả không được để trống.");
    public static readonly Error HanUngTuyenKhongHopLe = Error.Validation("TuyenDung.HanUngTuyenKhongHopLe", "Hạn ứng tuyển không hợp lệ.");
}
