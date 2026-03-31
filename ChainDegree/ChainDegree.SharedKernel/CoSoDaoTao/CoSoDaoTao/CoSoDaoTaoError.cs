using System;
using System.Collections.Generic;
using System.Text;
using ControlHub.SharedKernel.Common.Errors;

namespace ChainDegree.SharedKernel.CoSoDaoTao.CoSoDaoTao
{
    public class CoSoDaoTaoError
    {
        public static readonly Error SaiDinhDangCCCD =
            Error.Validation("CoSoDaoTao.SaiDinhDangCCCD", "Sai định dạng CCCD");

        public static readonly Error SaiDinhDangEmail =
            Error.Validation("CoSoDaoTao.SaiDinhDangEmail", "Sai định dạng email");
    }
}
