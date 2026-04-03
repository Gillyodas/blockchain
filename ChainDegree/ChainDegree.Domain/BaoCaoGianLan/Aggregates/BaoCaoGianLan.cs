using System;
using ChainDegree.Domain.BaoCaoGianLan.Enums;
using ChainDegree.SharedKernel.BaoCaoGianLan;
using ControlHub.SharedKernel.Results;

namespace ChainDegree.Domain.BaoCaoGianLan.Aggregates;

public class BaoCaoGianLan
{
    public Guid Id { get; private set; }
    // Thay vì lưu lại full bằng cấp, ta chỉ cần lưu mã Hash duy nhất để truy vết
    public string CredentialHash { get; private set; } 
    public Guid NguoiBaoCaoId { get; private set; } 
    public string LyDo { get; private set; }
    public string? GhiChu { get; private set; }
    public TrangThaiBaoCao TrangThai { get; private set; }
    public bool? LaGianLan { get; private set; } 
    public DateTime ThoiGianBaoCao { get; private set; }

    private BaoCaoGianLan(Guid id, string credentialHash, Guid nguoiBaoCaoId, string lyDo, string? ghiChu)
    {
        Id = id;
        CredentialHash = credentialHash;
        NguoiBaoCaoId = nguoiBaoCaoId;
        LyDo = lyDo;
        GhiChu = ghiChu;
        TrangThai = TrangThaiBaoCao.ChoXuLy; // Mặc định lúc mới tạo là Chờ xử lý
        ThoiGianBaoCao = DateTime.UtcNow;
    }

    // 1. NGƯỜI DÙNG TẠO BÁO CÁO MỚI
    public static Result<BaoCaoGianLan> Create(string credentialHash, Guid nguoiBaoCaoId, string lyDo, string? ghiChu)
    {
        if (string.IsNullOrWhiteSpace(credentialHash))
            return Result<BaoCaoGianLan>.Failure(BaoCaoGianLanError.BangCapTrong);

        if (nguoiBaoCaoId == Guid.Empty)
            return Result<BaoCaoGianLan>.Failure(BaoCaoGianLanError.NguoiBaoCaoTrong);

        if (string.IsNullOrWhiteSpace(lyDo))
            return Result<BaoCaoGianLan>.Failure(BaoCaoGianLanError.LyDoTrong);

        var baoCao = new BaoCaoGianLan(Guid.NewGuid(), credentialHash, nguoiBaoCaoId, lyDo, ghiChu);
        return Result<BaoCaoGianLan>.Success(baoCao);
    }

    // 2. ADMIN TIẾP NHẬN ĐIỀU TRA
    public Result TiepNhan()
    {
        if (TrangThai != TrangThaiBaoCao.ChoXuLy)
            return Result.Failure(BaoCaoGianLanError.TrangThaiKhongHopLe);

        TrangThai = TrangThaiBaoCao.DangXuLy;
        return Result.Success();
    }

    // 3. ADMIN CHỐT KẾT QUẢ CUỐI CÙNG
    public Result GiaiQuyet(bool xacNhanGianLan)
    {
        if (TrangThai != TrangThaiBaoCao.DangXuLy)
            return Result.Failure(BaoCaoGianLanError.TrangThaiKhongHopLe);

        TrangThai = xacNhanGianLan ? TrangThaiBaoCao.DaXuLy : TrangThaiBaoCao.TuChoi;
        LaGianLan = xacNhanGianLan;

        // Note: Nếu xacNhanGianLan = true, 
        // Lúc này sẽ phát Domain Event để trường ĐH bị trừ ngay 200 điểm uy tín.

        return Result.Success();
    }
}
