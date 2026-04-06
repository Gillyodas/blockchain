using ControlHub.SharedKernel.Common.Errors;

namespace ChainDegree.SharedKernel.TuyenDung;

public static class BangCapUngTuyenError
{
    public static readonly Error BangCapKhongHopLe = Error.Validation("BangCapUngTuyen.BangCapKhongHopLe", "ID hồ sơ hoặc bằng cấp không hợp lệ.");
}
