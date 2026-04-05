using System.ComponentModel;

namespace ChainDegree.Domain.TuyenDung.Enums;

public enum KieuGiayPhepNTD
{
    [Description("Giấy phép kinh doanh")]
    GiayPhepKinhDoanh = 1,
    
    [Description("Giấy chứng nhận thuế")]
    GiayChungNhanThue = 2,
    
    [Description("Giấy chứng nhận hiệp hội")]
    GiayChungNhanHiepHoi = 3,
    
    [Description("Khác")]
    Khac = 4
}
