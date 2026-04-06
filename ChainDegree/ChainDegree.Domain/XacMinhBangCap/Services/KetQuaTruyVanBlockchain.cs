namespace ChainDegree.Domain.XacMinhBangCap.Services;

public class KetQuaTruyVanBlockchain
{
    public bool TonTai { get; private set; }
    public bool BiThuHoi { get; private set; }
    public string? DiaChiNhaPhatHanh { get; private set; }
    public long ThoiGianGhiNhan { get; private set; }

    public KetQuaTruyVanBlockchain(bool tonTai, bool biThuHoi, string? diaChiNhaPhatHanh, long thoiGianGhiNhan)
    {
        TonTai = tonTai;
        BiThuHoi = biThuHoi;
        DiaChiNhaPhatHanh = diaChiNhaPhatHanh;
        ThoiGianGhiNhan = thoiGianGhiNhan;
    }
}
