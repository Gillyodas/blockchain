using ChainDegree.Domain.BaoCaoGianLan.Enums;
using ChainDegree.Domain.QuanLyToChuc.Enums;
using ChainDegree.Domain.QuanLyToChuc.ValueObjects;
using ChainDegree.Domain.TuyenDung.Entities;
using ChainDegree.Domain.TuyenDung.Enums;
using ChainDegree.SharedKernel.TuyenDung;
using ControlHub.SharedKernel.Results;

namespace ChainDegree.Domain.TuyenDung.Aggregates;

public class NhaTuyenDung
{
    public Guid Id { get; private set; }
    public string Ten { get; private set; } = null!;
    public string DiaChi { get; private set; } = null!;
    public string DiaChiViNhaTuyenDung { get; private set; } = null!;
    public Guid TaiKhoanId { get; private set; }
    public Guid YeuCauDangKyId { get; private set; }
    public DateTime ThoiGianTao { get; private set; } = DateTime.UtcNow;
    public DateTime? ThoiGianCapNhat { get; private set; }
    public DateTime? ThoiGianXoa { get; private set; }

    private readonly List<ThongTinTuyenDung> _thongTinTuyenDungs = new();
    public IReadOnlyCollection<ThongTinTuyenDung> ThongTinTuyenDungs => _thongTinTuyenDungs.AsReadOnly();
    private readonly List<GiayPhepNhaTuyenDung> _giayPheps = new();
    public IReadOnlyCollection<GiayPhepNhaTuyenDung> GiayPheps => _giayPheps.AsReadOnly();

    private NhaTuyenDung() { }

    private NhaTuyenDung(Guid id, string ten, string diaChiViNhaTuyenDung, Guid taiKhoanId, Guid yeuCauDangKyId, List<GiayPhepNhaTuyenDung> danhSachGiayPhepNTD)
    {
        Id = id; 
        Ten = ten; 
        DiaChiViNhaTuyenDung = diaChiViNhaTuyenDung; 
        TaiKhoanId = taiKhoanId; 
        YeuCauDangKyId = yeuCauDangKyId;
        _giayPheps = new List<GiayPhepNhaTuyenDung>(danhSachGiayPhepNTD);
    }

    public static Result<NhaTuyenDung> Create(string ten, string diaChiViNhaTuyenDung, Guid taiKhoanId, Guid yeuCauDangKyId, List<GiayPhepNhaTuyenDung> danhSachGiayPhepNTD)
    {
        if (string.IsNullOrWhiteSpace(ten))
            return Result<NhaTuyenDung>.Failure(TuyenDungError.TenNhaTuyenDungTrong);

        return Result<NhaTuyenDung>.Success(new NhaTuyenDung(Guid.NewGuid(), ten, diaChiViNhaTuyenDung, taiKhoanId, yeuCauDangKyId, danhSachGiayPhepNTD));
    }

    public Result CapNhatThongTinNhaTuyenDung(string ten, string diaChi)
    {
        if (string.IsNullOrWhiteSpace(ten))
            return Result.Failure(TuyenDungError.TenNhaTuyenDungTrong);

        Ten = ten;
        DiaChi = diaChi;
        ThoiGianCapNhat = DateTime.UtcNow;
        return Result.Success();
    }

    public Result<ThongTinTuyenDung> TaoTTTD(string ten, string moTa, Guid linhVucId, DateTime thoiHanUngTuyen)
    {
        var result = ThongTinTuyenDung.Create(ten, moTa, linhVucId, thoiHanUngTuyen, Id);
        if (result.IsSuccess)
        {
            _thongTinTuyenDungs.Add(result.Value);
            ThoiGianCapNhat = DateTime.UtcNow;
        }
        return result;
    }

    public Result CapNhatTTTD(Guid tttdId, string ten, string moTa, Guid linhVucId, DateTime thoiHanUngTuyen)
    {
        var tttd = _thongTinTuyenDungs.FirstOrDefault(x => x.Id == tttdId);
        if (tttd == null)
            return Result.Failure(TuyenDungError.TinTuyenDungKhongTonTai);

        var result = tttd.CapNhatTTTD(ten, moTa, linhVucId, thoiHanUngTuyen);
        if (result.IsSuccess)
            ThoiGianCapNhat = DateTime.UtcNow;

        return result;
    }

    public Result XoaTTTD(Guid tttdId)
    {
        var tttd = _thongTinTuyenDungs.FirstOrDefault(x => x.Id == tttdId);
        if (tttd == null)
            return Result.Failure(TuyenDungError.TinTuyenDungKhongTonTai);

        var result = tttd.XoaTTTD();
        if (result.IsSuccess) ThoiGianCapNhat = DateTime.UtcNow;
        return result;
    }

    public Result ThemGiayPhep(string duongDan, LoaiGiayPhepNTD loai)
    {
        var result = GiayPhepNhaTuyenDung.Create(duongDan, loai);
        if (result.IsSuccess)
        {
            _giayPheps.Add(result.Value);
            ThoiGianCapNhat = DateTime.UtcNow;
        }
        return result;
    }
    public Result CapNhatGiayPhep(string oldPath, string newPath, LoaiGiayPhepNTD loai)
    {
        var gp = _giayPheps.FirstOrDefault(x => x.DuongDanLuuTru == oldPath);
        if (gp == null)
            return Result.Failure(TuyenDungError.GiayPhepKhongTonTai);

        _giayPheps.Remove(gp);
        var result = GiayPhepNhaTuyenDung.Create(newPath, loai);
        if (result.IsSuccess)
        {
            _giayPheps.Add(result.Value);
            ThoiGianCapNhat = DateTime.UtcNow;
        }
        return result;
    }
    public Result XoaGiayPhep(string path)
    {
        var gp = _giayPheps.FirstOrDefault(x => x.DuongDanLuuTru == path);
        if (gp == null)
            return Result.Failure(TuyenDungError.GiayPhepKhongTonTai);

        _giayPheps.Remove(gp);
        ThoiGianCapNhat = DateTime.UtcNow;
        return Result.Success();
    }

    public Result DuyetHoSoUngTuyen(HoSoUngTuyen hoSo, TrangThaiUngTuyen trangThai)
    {
        return hoSo.CapNhatTrangThai(trangThai);
    }

    public Result<ChainDegree.Domain.BaoCaoGianLan.Aggregates.BaoCaoGianLan> BaoCaoGianLanBangCap(Guid bangCapId, Guid coSoDaoTaoId, LyDoBaoCaoGianLan lyDo, string? ghiChu)
    {
        var result = ChainDegree.Domain.BaoCaoGianLan.Aggregates.BaoCaoGianLan.Create(bangCapId, this.Id, LoaiNguoiBaoCao.NhaTuyenDung, coSoDaoTaoId, lyDo, ghiChu);
        if (result.IsSuccess)
            ThoiGianCapNhat = DateTime.UtcNow;
        return result;
    }
}
