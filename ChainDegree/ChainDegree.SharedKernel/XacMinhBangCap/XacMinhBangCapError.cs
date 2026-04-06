using ControlHub.SharedKernel.Common.Errors;

namespace ChainDegree.SharedKernel.XacMinhBangCap;

public static class XacMinhBangCapError
{
    public static readonly Error BangCapIdKhongHopLe = Error.Validation("XacMinhBangCap.BangCapIdKhongHopLe", "ID bằng cấp không hợp lệ.");
    public static readonly Error MaBamXacMinhTrong = Error.Validation("XacMinhBangCap.MaBamXacMinhTrong", "Mã băm xác minh không được để trống.");
}
