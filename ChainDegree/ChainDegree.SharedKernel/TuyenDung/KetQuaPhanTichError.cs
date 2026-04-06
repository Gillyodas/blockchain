using ControlHub.SharedKernel.Common.Errors;

namespace ChainDegree.SharedKernel.TuyenDung;

public static class KetQuaPhanTichError
{
    public static readonly Error PhanTramPhuHopKhongHopLe = Error.Validation("KetQuaPhanTich.PhanTramPhuHopKhongHopLe", "Phần trăm phù hợp phải nằm trong khoảng 0–100.");
    public static readonly Error KetLuanKhongDuocTrong = Error.Validation("KetQuaPhanTich.KetLuanKhongDuocTrong", "Kết luận không được để trống.");
}
