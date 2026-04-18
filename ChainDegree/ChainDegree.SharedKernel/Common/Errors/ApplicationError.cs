using System;
using System.Collections.Generic;
using System.Text;
using ControlHub.SharedKernel.Common.Errors;

namespace ChainDegree.SharedKernel.Common.Errors;

public static class ApplicationError
{
    public static readonly Error ConcurrencyError =
        Error.Conflict("ConcurrencyError", "Đã xảy ra lỗi đồng thời. Vui lòng thử lại sau.");

    public static readonly Error RepositoryError =
        Error.Failure("RepositoryError", "Đã xảy ra lỗi khi truy cập dữ liệu. Vui lòng thử lại sau.");

    public static readonly Error Cancelled =
        Error.Failure("Cancelled", "Yêu cầu đã bị hủy.");

    public static readonly Error UnknownError =
        Error.Failure("UnknownError", "Đã xảy ra lỗi không xác định. Vui lòng thử lại sau.");
}
