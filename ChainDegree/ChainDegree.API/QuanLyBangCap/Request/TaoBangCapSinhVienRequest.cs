namespace ChainDegree.API.QuanLyBangCap.Request;

public class TaoBangCapSinhVienRequest
{
    public Guid? SinhVienId { get; set; }
    public string? CCCD { get; set; }
    public string TenSinhVien { get; set; } = null!;
    public int LoaiBangCap { get; set; }
    public Guid LinhVucId { get; set; }
    public double? Diem { get; set; }
    public DateTime NgayCap { get; set; }
    public DateTime NgayHetHan { get; set; }
    public string? FileBangCap { get; set; }
    public string? LinkBangCap { get; set; }
}
