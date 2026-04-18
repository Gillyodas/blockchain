using System;
using System.Collections.Generic;
using ControlHub.Domain.SharedKernel;

namespace ChainDegree.Domain.QuanLyToChuc.ValueObjects;

public class ThongTinChuKySo : ValueObject
{
    public bool CoChuKySo { get; private set; }
    public bool HopLe { get; private set; }
    public string NhaCungCap { get; private set; } = string.Empty;
    public bool NhaCungCapDuocTinTuong { get; private set; }
    public DateTime NgayHetHan { get; private set; }
    public DateTime XacMinhLuc { get; private set; }

    private ThongTinChuKySo() { }

    private ThongTinChuKySo(bool coChuKySo, bool hopLe, string nhaCungCap, bool nhaCungCapDuocTinTuong, DateTime ngayHetHan, DateTime xacMinhLuc)
    {
        CoChuKySo = coChuKySo;
        HopLe = hopLe;
        NhaCungCap = nhaCungCap;
        NhaCungCapDuocTinTuong = nhaCungCapDuocTinTuong;
        NgayHetHan = ngayHetHan;
        XacMinhLuc = xacMinhLuc;
    }

    internal static ThongTinChuKySo Tao(bool coChuKySo, bool hopLe, string nhaCungCap, bool nhaCungCapDuocTinTuong, DateTime ngayHetHan)
    {
        return new ThongTinChuKySo(coChuKySo, hopLe, nhaCungCap, nhaCungCapDuocTinTuong, ngayHetHan, DateTime.UtcNow);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return CoChuKySo;
        yield return HopLe;
        yield return NhaCungCap;
        yield return NhaCungCapDuocTinTuong;
        yield return NgayHetHan;
        yield return XacMinhLuc;
    }
}
