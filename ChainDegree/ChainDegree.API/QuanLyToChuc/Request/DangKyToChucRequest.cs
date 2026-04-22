using ChainDegree.Domain.QuanLyToChuc.Enums;

namespace ChainDegree.API.QuanLyToChuc.Request;

public class DangKyToChucRequest
{
    public string TenToChuc { get; set; } = null!;
    public LoaiToChuc LoaiToChucDangKy { get; set; }
    public string DiaChiVi { get; set; } = null!;
}
