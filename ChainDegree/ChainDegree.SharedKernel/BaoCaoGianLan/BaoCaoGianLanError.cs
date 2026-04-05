using ControlHub.SharedKernel.Common.Errors;

namespace ChainDegree.SharedKernel.BaoCaoGianLan;

public static class BaoCaoGianLanError
{
    public static readonly Error BangCapIdTrong =
        Error.Validation("BaoCaoGianLan.BangCapIdTrong", "ID bằng cấp không hợp lệ.");

    public static readonly Error NguoiBaoCaoTrong =
        Error.Validation("BaoCaoGianLan.NguoiBaoCaoTrong", "Người báo cáo không hợp lệ.");

    public static readonly Error LyDoKhacCanGhiChu =
        Error.Validation("BaoCaoGianLan.LyDoKhacCanGhiChu", "Ghi chú là bắt buộc khi lý do báo cáo là 'Khác'.");

    public static readonly Error TrangThaiKhongHopLe =
        Error.Validation("BaoCaoGianLan.TrangThaiKhongHopLe", "Trạng thái báo cáo không hợp lệ để thực hiện thao tác này.");
}
