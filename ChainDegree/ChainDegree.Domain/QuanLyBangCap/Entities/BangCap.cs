using System;
using System.Collections.Generic;
using System.Text;
using ChainDegree.Domain.QuanLyBangCap.Enums;
using ChainDegree.SharedKernel.QuanLyBangCap.BangCap;
using ControlHub.SharedKernel.Results;

namespace ChainDegree.Domain.QuanLyBangCap.Entities;

public class BangCap
{
    public Guid Id { get; private set; }
    public string Ten { get; private set; }
    public string? File { get; private set; }
    public double? Diem { get; private set; }
    public string? Link { get; private set; }
    public DateTime NgayCap { get; private set; }
    public DateTime? NgayHetHan { get; private set; }
    public string? MaBamXacThuc { get; private set; }
    public string? MaBamGiaoDich { get; private set; }
    public string? Salt { get; private set; }
    public LoaiBangCap LoaiBangCap { get; private set; }
    public Guid LinhVucId { get; private set; }
    public LyDoHuy? LyDoHuyBangCap { get; private set; }
    public LyDoThuHoi? LyDoThuHoiBangCap { get; private set; }
    public LyDoKhoiPhuc? LyDoKhoiPhucBangCap { get; private set; }
    public string? GhiChuHuy { get; private set; }
    public string? GhiChuThuHoi { get; private set; }
    public string? GhiChuKhoiPhuc { get; private set; }
    public TrangThaiBlockchain TrangThaiBlockchainHienTai { get; private set; }
    public TrangThaiBangCap TrangThaiBangCapHienTai { get; private set; }
    public Guid CoSoDaoTaoCapId { get; private set; }
    public Guid SinhVienId { get; private set; }
    public DateTime ThoiGianTao { get; private set; }
    public DateTime ThoiGianCapNhat { get; private set; } = DateTime.MinValue;
    public DateTime ThoiGianXoa { get; private set; } = DateTime.MinValue;

    private BangCap(
        Guid id, string ten, double? diem, LoaiBangCap loaiBangCap, Guid linhVucId,
        DateTime ngayCap, DateTime? ngayHetHan, string? file, string? link,
        TrangThaiBangCap trangThaiBangCap, TrangThaiBlockchain trangThaiBlockchain,
        DateTime thoiGianTao, Guid coSoDaoTapCapId, Guid sinhVienId)
    {
        Id = id;
        Ten = ten;
        File = file;
        Diem = diem;
        Link = link;
        NgayCap = ngayCap;
        NgayHetHan = ngayHetHan;
        LoaiBangCap = loaiBangCap;
        TrangThaiBlockchainHienTai = trangThaiBlockchain;
        TrangThaiBangCapHienTai = trangThaiBangCap;
        ThoiGianTao = thoiGianTao;
        CoSoDaoTaoCapId = coSoDaoTapCapId;
        SinhVienId = sinhVienId;
    }

    // Summary:
    // Tạo mới một bằng cấp với các thông tin cơ bản.
    // Điều kiện: Điểm phải lớn hơn hoặc bằng 0 nếu có, hạn sử dụng phải lớn hơn ngày cấp nếu có.
    // Hành động: Khởi tạo đối tượng bằng cấp với trạng thái ban đầu là "Nhập" và trạng thái blockchain là "Chờ duyệt".
    // Trả về: Kết quả thành công với đối tượng bằng cấp mới hoặc lỗi nếu điều kiện không hợp lệ.
    internal static Result<BangCap> Create(
        string ten,
        double? diem,
        LoaiBangCap loaiBangCap,
        Guid linhVucId,
        DateTime ngayCap,
        DateTime? ngayHetHan,
        string? file,
        string? link,
        Guid coSoDaoTaoCapId,
        Guid sinhVienId)
    {
        if (diem.HasValue && diem < 0)
            return Result<BangCap>.Failure(BangCapError.DiemKhongHopLe);

        if (ngayHetHan.HasValue && ngayHetHan <= ngayCap)
            return Result<BangCap>.Failure(BangCapError.HanSuDungBangCapKhongHopLe);

        var bangCap = new BangCap(
            Guid.NewGuid(), ten, diem, loaiBangCap, linhVucId,
            ngayCap, ngayHetHan, file, link,
            TrangThaiBangCap.ChuaXacNhan,
            TrangThaiBlockchain.ChoDuyet,
            DateTime.UtcNow, coSoDaoTaoCapId, sinhVienId
            );

        return Result<BangCap>.Success(bangCap);
    }

    // Summary:
    // Gán mã băm xác thực và salt trước khi gửi dữ liệu lên blockchain để xác thực.
    // Điều kiện: Chỉ được gán khi mã băm xác thực chưa tồn tại và trạng thái blockchain là "Chờ duyệt".
    // Hành động: Cập nhật mã băm xác thực, salt và thời gian cập nhật.
    // Trả về: Kết quả thành công với đối tượng bằng cấp đã được cập nhật hoặc lỗi nếu điều kiện không hợp lệ.
    internal Result GanMaBamXacThuc(string maBamXacThuc, string salt)
    {
        if (MaBamXacThuc is not null || TrangThaiBlockchainHienTai != TrangThaiBlockchain.ChoDuyet)
            return Result.Failure(BangCapError.TrangThaiKhongHopLe);

        MaBamXacThuc = maBamXacThuc;
        Salt = salt;
        ThoiGianCapNhat = DateTime.UtcNow;

        return Result.Success();
    }

    // Summary:
    // Gán mã băm giao dịch sau khi giao dịch được xác nhận trên blockchain.
    // Điều kiện: Chỉ được gán khi trạng thái bằng cấp là "Chưa xác nhận" và trạng thái blockchain là "Chờ duyệt".
    // Hành động: Cập nhật mã băm giao dịch, chuyển trạng thái blockchain sang "Xác nhận", chuyển trạng thái bằng cấp sang "Đã xác nhận", cập nhật thời gian cập nhật.
    // Trả về: Kết quả thành công với đối tượng bằng cấp đã được cập nhật hoặc lỗi nếu điều kiện không hợp lệ.
    internal Result GanMaBamGiaoDich(string maBamGiaoDich)
    {
        if (TrangThaiBangCapHienTai != TrangThaiBangCap.ChuaXacNhan || TrangThaiBlockchainHienTai != TrangThaiBlockchain.ChoDuyet)
            return Result.Failure(BangCapError.TrangThaiKhongHopLe);

        MaBamGiaoDich = maBamGiaoDich;
        TrangThaiBlockchainHienTai = TrangThaiBlockchain.XacNhan;
        TrangThaiBangCapHienTai = TrangThaiBangCap.DaXacNhan;
        ThoiGianCapNhat = DateTime.UtcNow;

        return Result.Success();
    }

    // Summary:
    // Đánh dấu bằng cấp là đã hủy với lý do hủy và ghi chú hủy.
    // Điều kiện: Chỉ được hủy bởi cơ sở đào tạo cấp bằng và chỉ được hủy khi trạng thái bằng cấp là "Chưa xác nhận".
    // Hành động: Cập nhật lý do hủy, ghi chú hủy, chuyển trạng thái bằng cấp sang "Đã hủy", cập nhật thời gian cập nhật.
    // Trả về: Kết quả thành công hoặc lỗi nếu điều kiện không hợp lệ.
    internal Result DanhDauHuy(LyDoHuy lyDoHuy, string? ghiChuHuy, Guid nguoiHuyId)
    {
        if(nguoiHuyId != CoSoDaoTaoCapId)
            return Result.Failure(BangCapError.BangCapChiDuocHuyBoiNguoiTao);

        if (TrangThaiBangCapHienTai != TrangThaiBangCap.ChuaXacNhan)
            return Result.Failure(BangCapError.BangCapChiDuocHuyKhiChuaXacNhan);

        if(lyDoHuy == LyDoHuy.Khac && string.IsNullOrEmpty(ghiChuHuy))
            return Result.Failure(BangCapError.GhiChuHuyKhongDuocBoTrongKhiLyDoHuyLaKhac);

        LyDoHuyBangCap = lyDoHuy;
        GhiChuHuy = ghiChuHuy;
        TrangThaiBangCapHienTai = TrangThaiBangCap.DaHuy;
        ThoiGianCapNhat = DateTime.UtcNow;
        return Result.Success();
    }

    // Summary:
    // Đánh dấu bằng cấp là đã thu hồi với lý do thu hồi và ghi chú thu hồi.
    // Điều kiện: Chỉ được thu hồi bởi cơ sở đào tạo cấp bằng và chỉ được thu hồi khi trạng thái bằng cấp là "Đã xác nhận" và trạng thái blockchain là "Xác nhận".
    // Hành động: Cập nhật lý do thu hồi, ghi chú thu hồi, cập nhật thời gian cập nhật.
    // Trả về: Kết quả thành công hoặc lỗi nếu điều kiện không hợp lệ.
    internal Result DanhDauThuHoi(LyDoThuHoi lyDoThuHoi, string? ghiChuThuHoi, Guid nguoiThuHoiId)
    {
        if(nguoiThuHoiId != CoSoDaoTaoCapId)
            return Result.Failure(BangCapError.BangCapChiDuocThuHoiBoiNguoiTao);

        if (TrangThaiBangCapHienTai != TrangThaiBangCap.DaXacNhan || TrangThaiBlockchainHienTai != TrangThaiBlockchain.XacNhan)
            return Result.Failure(BangCapError.TrangThaiKhongHopLe);

        if(lyDoThuHoi == LyDoThuHoi.Khac && string.IsNullOrEmpty(ghiChuThuHoi))
            return Result.Failure(BangCapError.GhiChuThuHoiKhongDuocBoTrongKhiLyDoThuHoiLaKhac);

        LyDoThuHoiBangCap = lyDoThuHoi;
        GhiChuThuHoi = ghiChuThuHoi;
        TrangThaiBangCapHienTai = TrangThaiBangCap.DaThuHoi;
        ThoiGianCapNhat = DateTime.UtcNow;
        return Result.Success();
    }

    // Summary:
    // Đánh dấu bằng cấp là đã khôi phục với lý do khôi phục và ghi chú khôi phục.
    // Điều kiện: Chỉ được khôi phục bởi cơ sở đào tạo cấp bằng và chỉ được khôi phục khi trạng thái bằng cấp là "Đã thu hồi".
    // Hành động: Cập nhật lý do khôi phục, ghi chú khôi phục, cập nhật thời gian cập nhật.
    // Trả về: Kết quả thành công hoặc lỗi nếu điều kiện không hợp lệ.
    // Note: Khi khôi phục bằng cấp sẽ có thêm dữ liệu LyDoKhoiPhuc, điều này đánh dấu bằng cấp này đã mất hiệu lực thực tế và chỉ ở trên blockchain như là 1 fact còn bằng cấp được khội phục thực sự đã được tạo mới với dữ liệu của bằng cũ.
    internal Result<BangCap> KhoiPhuc(Guid nguoiKhoiPhucId, LyDoKhoiPhuc lyDoKhoiPhuc, string ghiChuKhoiPhuc)
    {
        if(nguoiKhoiPhucId != CoSoDaoTaoCapId)
            return Result<BangCap>.Failure(BangCapError.BangCapChiDuocKhoiPhucBoiNguoiTao);

        if(TrangThaiBangCapHienTai != TrangThaiBangCap.DaThuHoi)
            return Result<BangCap>.Failure(BangCapError.BangCapChiDuocKhoiPhucKhiDaThuHoi);

        if(lyDoKhoiPhuc == LyDoKhoiPhuc.Khac && string.IsNullOrEmpty(ghiChuKhoiPhuc))
            return Result<BangCap>.Failure(BangCapError.GhiChuKhoiPhucKhongDuocBoTrongKhiLyDoKhoiPhucLaKhac);

        LyDoKhoiPhucBangCap = lyDoKhoiPhuc;
        GhiChuKhoiPhuc = ghiChuKhoiPhuc;
        ThoiGianCapNhat = DateTime.UtcNow;

        BangCap newBangCap = new BangCap(
            Guid.NewGuid(), this.Ten, this.Diem, this.LoaiBangCap, this.LinhVucId,
            this.NgayCap, this.NgayHetHan, this.File, this.Link,
            TrangThaiBangCap.ChuaXacNhan,
            TrangThaiBlockchain.ChoDuyet,
            DateTime.UtcNow, this.CoSoDaoTaoCapId, this.SinhVienId
            );

        return Result<BangCap>.Success(newBangCap);
    }
}
