using ChainDegree.Domain.QuanLyBangCap.Enums;

namespace ChainDegree.API.QuanLyBangCap.Request;

public class TaoBangCapSinhVienRequest
{
    public Guid? SinhVienId { get; set; }
    public string? CCCD { get; set; }
    public string TenSinhVien { get; set; } = null!;
    public LoaiBangCap LoaiBangCap { get; set; }
    public string TenBangCap { get; set; } = null!;
    public Guid LinhVucId { get; set; }
    public double? Diem { get; set; }
    public DateTime NgayCap { get; set; }
    public DateTime NgayHetHan { get; set; }
    public string? FileBangCap { get; set; }
    public string? LinkBangCap { get; set; }
}
