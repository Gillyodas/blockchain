using System;
using System.Collections.Generic;
using System.Text;
using ControlHub.Domain.SharedKernel;
using ControlHub.SharedKernel.Results;

namespace ChainDegree.Domain.QuanLyToChuc.ValueObjects;

public class ThongTinChuKySo : ValueObject
{
    public bool CoChuKySo { get; private set; }
    public bool HopLe { get; private set; }
    public string NhaCungCap { get; private set; }
    public bool NhaCungCapDuocTinTuong { get; private set; }
    public DateTime NgayHetHan { get; private set; }
    public DateTime XacMinhLuc { get; private set; }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }

    internal Result GanThongTinChuKySo()
    {
        throw new NotImplementedException();
    }
}
