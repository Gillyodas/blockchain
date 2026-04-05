using System.ComponentModel;

namespace ChainDegree.Domain.TuyenDung.Enums;

public enum TrangThaiThongTinTuyenDung
{
    [Description("Đang tuyển")]
    DangTuyen = 1,
    
    [Description("Đã đóng")]
    DaDong = 2,
    
    [Description("Hết hạn")]
    HetHan = 3,
    
    [Description("Bản nháp")]
    BanNhap = 4
}
