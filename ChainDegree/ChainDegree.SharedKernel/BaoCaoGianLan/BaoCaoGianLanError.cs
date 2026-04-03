using ControlHub.SharedKernel.Common.Errors;

namespace ChainDegree.SharedKernel.BaoCaoGianLan;

public static class BaoCaoGianLanError
{
    public static readonly Error LyDoTrong = Error.Validation("BaoCaoGianLan.LyDoTrong", "Lý do báo cáo không được để trống.");
    public static readonly Error BangCapTrong = Error.Validation("BaoCaoGianLan.BangCapTrong", "Mã bằng cấp không hợp lệ.");
    public static readonly Error NguoiBaoCaoTrong = Error.Validation("BaoCaoGianLan.NguoiBaoCaoTrong", "Người báo cáo không hợp lệ.");
    public static readonly Error TrangThaiKhongHopLe = Error.Validation("BaoCaoGianLan.TrangThaiKhongHopLe", "Trạng thái báo cáo không hợp lệ để thực hiện thao tác này.");
}
