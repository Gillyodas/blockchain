using System.ComponentModel;

namespace ChainDegree.Domain.TuyenDung.Enums;

public enum TrangThaiUngTuyen
{
    [Description("Chờ xem")]
    ChoXem = 0,
    
    [Description("Đã xem")]
    DaXem = 1,
    
    [Description("Chấp nhận")]
    ChapNhan = 2,
    
    [Description("Từ chối")]
    TuChoi = 3
}
