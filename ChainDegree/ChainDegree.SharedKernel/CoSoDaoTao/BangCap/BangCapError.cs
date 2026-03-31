using System;
using System.Collections.Generic;
using System.Text;
using ControlHub.SharedKernel.Common.Errors;

namespace ChainDegree.SharedKernel.CoSoDaoTao.BangCap;

public class BangCapError
{
    public static readonly Error DiemKhongHopLe =
        Error.Validation("BangCap.DiemKhongHopLe", "Điểm không hợp lệ, phải lớn hơn hoặc bằng 0");
}
