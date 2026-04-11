namespace ChainDegree.Domain.DanhMuc.Entities;

public class LinhVuc
{
    public Guid Id { get; private set; }
    public string Ten { get; private set; }
    public DateTime ThoiGianTao { get; private set; }
    public DateTime? ThoiGianCapNhat { get; private set; }
    public DateTime? ThoiGianXoa { get; private set; }

    private LinhVuc() { }

    private LinhVuc(Guid id, string ten)
    {
        Id = id;
        Ten = ten;
        ThoiGianTao = DateTime.UtcNow;
    }

    public static LinhVuc Create(string ten)
    {
        return new LinhVuc(Guid.NewGuid(), ten);
    }

    public void CapNhat(string ten)
    {
        Ten = ten;
        ThoiGianCapNhat = DateTime.UtcNow;
    }

    public void Xoa()
    {
        ThoiGianXoa = DateTime.UtcNow;
    }
}
