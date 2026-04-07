# ĐẶC TẢ QUY TRÌNH NGHIỆP VỤ — HỆ THỐNG CHAINDEGREE

> Phiên bản 2.0 — Năm 2026 | Tài liệu nội bộ — Nhóm 1

---

## MỤC LỤC

1. [Tổng quan hệ thống](#1-tổng-quan-hệ-thống)
2. [Quy trình đăng ký & xác thực tổ chức (Issuer / Verifier)](#2-quy-trình-đăng-ký--xác-thực-tổ-chức)
   - 2.1. Đăng ký trở thành Cơ sở đào tạo (Issuer)
   - 2.2. Đăng ký trở thành Nhà tuyển dụng (Verifier)
3. [Quy trình quản lý sinh viên (Holder)](#3-quy-trình-quản-lý-sinh-viên-holder)
4. [Quy trình cấp bằng / chứng chỉ (Issue Credential)](#4-quy-trình-cấp-bằng--chứng-chỉ-issue-credential)
5. [Quy trình hủy bằng (Cancel Credential)](#5-quy-trình-hủy-bằng-cancel-credential)
6. [Quy trình thu hồi bằng (Revoke Credential)](#6-quy-trình-thu-hồi-bằng-revoke-credential)
7. [Quy trình khôi phục bằng đã thu hồi (Restore Credential)](#7-quy-trình-khôi-phục-bằng-đã-thu-hồi-restore-credential)
8. [Quy trình cập nhật bằng cấp (Update Credential)](#8-quy-trình-cập-nhật-bằng-cấp-update-credential)
9. [Quy trình xác minh bằng (Verify Credential)](#9-quy-trình-xác-minh-bằng-verify-credential)
10. [Quy trình báo cáo gian lận (Report Fraud)](#10-quy-trình-báo-cáo-gian-lận-report-fraud)
11. [Quy trình tuyển dụng hỗ trợ bởi AI](#11-quy-trình-tuyển-dụng-hỗ-trợ-bởi-ai)
12. [Phụ lục: Trạng thái & ràng buộc hệ thống](#12-phụ-lục-trạng-thái--ràng-buộc-hệ-thống)

---

## 1. Tổng quan hệ thống

ChainDegree là nền tảng quản lý và xác minh bằng cấp học thuật dựa trên blockchain riêng tư (Hyperledger Besu). Hệ thống áp dụng mô hình lưu trữ hybrid: toàn bộ dữ liệu chi tiết lưu trong SQL Server, chỉ duy nhất một giá trị hash (keccak256) của từng bằng cấp được ghi lên blockchain để đảm bảo tính bất biến và có thể kiểm toán độc lập.

### 1.1. Các Actor trong hệ thống

| Actor | Vai trò | Mô tả |
|---|---|---|
| Account | Tài khoản gốc | Mọi người dùng đều bắt đầu với vai trò Account. Có thể nâng cấp lên Issuer hoặc Verifier nếu đăng ký thành công. |
| Issuer (Cơ sở đào tạo) | Cấp bằng | Tổ chức giáo dục được Admin phê duyệt. Có quyền tạo sinh viên, cấp, hủy, thu hồi và khôi phục bằng. |
| Verifier (Nhà tuyển dụng) | Xác minh bằng | Doanh nghiệp được Admin phê duyệt. Có quyền xác minh bằng, đăng tin tuyển dụng và báo cáo gian lận. |
| Holder (Sinh viên) | Sở hữu bằng | Được Issuer tạo tài khoản. Xem, chia sẻ bằng và ứng tuyển. Có thể thuộc nhiều Issuer. |
| Admin | Quản trị | Duyệt hồ sơ Issuer và Verifier. Cấp/hủy quyền. Không tạm khóa tổ chức. |

### 1.2. Nguyên tắc luồng dữ liệu

- **Ghi blockchain:** Chỉ thực hiện sau khi ghi SQL Server thành công. Nếu ghi blockchain thất bại, sẽ đưa vào hàng đợi Outbox để retry — không bao giờ để trạng thái không nhất quán.
- **Hash bằng cấp:** `credentialHash = keccak256(toàn bộ dữ liệu bằng cấp đã chuẩn hóa + salt)`. Là bằng chứng duy nhất ghi trên smart contract. Hash không được thay đổi sau khi tạo. Smart contract chỉ lưu: `credentialHash → { exists, revoked, issuerAddress, timestamp }` — mọi metadata nghiệp vụ khác (lý do thu hồi, ghi chú, thông tin sinh viên) đều lưu tại SQL Server.
- **Cập nhật bằng cấp:** Khi CSDT cần sửa thông tin bằng, hệ thống tự động thu hồi bản cũ + cấp lại bản mới trên blockchain, đảm bảo tính bất biến.
- **Điểm uy tín (UyTinToChuc):** Tính toán off-chain tại SQL Server. UyTin là tín hiệu tin cậy (AI và NTD tham khảo), không phải cơ chế kiểm soát quyền.

---

## 2. Quy trình đăng ký & xác thực tổ chức

### 2.1. Đăng ký trở thành Cơ sở đào tạo (Issuer)

#### 2.1.1. Điều kiện tiên quyết

- Tổ chức đã có tài khoản Account trên hệ thống (đã xác thực email).
- Tổ chức chưa ở trạng thái Issuer hoặc đang chờ duyệt.

#### 2.1.2. Luồng chính

| Bước | Hành động | Kết quả / Ghi chú |
|---|---|---|
| 1 | Tổ chức điền thông tin hồ sơ: **tên tổ chức** (bắt buộc), địa chỉ (bắt buộc), mã số trường (nếu có), website. Email và số điện thoại liên hệ tổ chức được lấy tự động từ tài khoản Account — chỉ cần cung cấp tường minh nếu tổ chức muốn dùng email/sdt **khác** với tài khoản (ví dụ: đăng ký qua hợp đồng hợp tác). | `YeuCauDangKy` được tạo với `TrangThai = Nhap`. |
| 2 | Tổ chức upload tối thiểu 2 giấy tờ bắt buộc (xem bảng 2.1.5). Hỗ trợ định dạng PDF có chữ ký số. Mỗi giấy tờ cần kèm **ngày hết hiệu lực** của giấy phép (do domain `GiayPhepCSDT` yêu cầu `ThoiGianHetHan > hiện tại`). | File được lưu vào storage. Bản ghi `GiayPhepCSDT` được tạo với `TrangThaiXacMinhGiayPhep = ChoXacMinh`. |
| 3 | **[Tự động]** Hệ thống chạy verify chữ ký số trên từng file PDF vừa upload (PKI verification). | `ThongTinChuKySo` được cập nhật: `CoChuKySo`, `HopLe`, `NhaCungCap`, `NhaCungCapDuocTinTuong`, `NgayHetHan`, `XacMinhLuc`. |
| 4 | **[Tự động]** Hệ thống cập nhật `TrangThaiXacMinhGiayPhep` của `GiayPhepCSDT` theo kết quả verify. | Xem chi tiết tại mục 2.1.4. |
| 5 | Tổ chức xác nhận nộp hồ sơ. | Trạng thái `YeuCauDangKy` chuyển thành `DaGui`. `ThoiGianNop` được ghi nhận. Admin nhận thông báo. |
| 6 | Admin xem xét hồ sơ: thông tin tổ chức + kết quả verify chữ ký số từng giấy tờ. | Admin có thể xem chi tiết `ThongTinChuKySo` của từng file. |
| 7a | **[Admin DUYỆT]** Admin nhấn Phê duyệt + ghi ghi chú (tuỳ chọn). | Hệ thống raise `CoSoDaoTaoApprovedEvent` → Application handler tạo `CoSoDaoTao` + gắn Role Issuer. `UyTinToChuc` khởi tạo = `50 × số giấy phép đã nộp` (tối thiểu 100 với 2 giấy bắt buộc). `TrangThaiYeuCauDangKy = XacNhan`. `ThoiGianXetDuyet` được ghi nhận. Tổ chức nhận email thông báo. |
| 7b | **[Admin TỪ CHỐI]** Admin nhấn Từ chối + chọn `LyDoTuChoi` (enum) + ghi `GhiChuTuChoi` (bắt buộc). Nếu `LyDoTuChoi = Khac` thì `GhiChuTuChoi` không được để trống. | `TrangThaiYeuCauDangKy = TuChoi`. `ThoiGianXetDuyet` được ghi nhận. Tổ chức nhận email kèm lý do. |

#### 2.1.3. Xử lý sau khi bị từ chối

- **Giấy phép bị từ chối:** Tổ chức có thể upload giấy phép mới thay thế. `TrangThaiXacMinhGiayPhep` chuyển từ `TuChoi` → `TaiLenLai` → `ChoXacMinh`. Hệ thống tự động verify lại chữ ký số.
- **Toàn bộ đăng ký bị từ chối:** Tổ chức phải tạo `YeuCauDangKy` mới từ đầu.

#### 2.1.4. Kết quả verify chữ ký số và hành động tương ứng

| Trạng thái | Điều kiện | Ảnh hưởng đến duyệt |
|---|---|---|
| `SIGNATURE_VALID` | `CoChuKySo=true`, `HopLe=true`, `NhaCungCapDuocTinTuong=true`, `NgayHetHan > Now` | Admin thấy badge ✓ Hợp lệ. Ưu tiên xét duyệt nhanh. |
| `SIGNATURE_VALID` (cert hết hạn) | `HopLe=true` nhưng `NgayHetHan < Now` | Admin thấy cảnh báo ⚠ Cert đã hết hạn. Admin có thể duyệt nhưng điểm uy tín ban đầu thấp hơn. |
| `SIGNATURE_INVALID` | `CoChuKySo=true` nhưng `HopLe=false` (file đã bị chỉnh sửa sau khi ký) | Admin thấy cảnh báo 🚫 File bị thay đổi. Thông thường từ chối. |
| `UNTRUSTED_CA` | `HopLe=true` nhưng `NhaCungCapDuocTinTuong=false` | Admin thấy cảnh báo ⚠ CA không được công nhận. Admin quyết định thủ công. |
| `NO_SIGNATURE` | `CoChuKySo=false` | Admin thấy thông tin ℹ Không có chữ ký số. Admin xác minh bằng phương thức khác. |

#### 2.1.5. Loại giấy tờ của Cơ sở đào tạo

| Loại giấy tờ | Mô tả | Yêu cầu |
|---|---|---|
| `GiayPhepHoatDongGiaoDuc` | Giấy phép do Bộ GD&ĐT hoặc cơ quan có thẩm quyền cấp | **BẮT BUỘC** |
| `QuyetDinhThanhLapTruong` | Quyết định thành lập trường / tổ chức đào tạo | **BẮT BUỘC** |
| `GiayCongNhanNganhDaoTao` | Giấy công nhận chương trình / ngành đào tạo cụ thể | Tùy chọn |
| `XacNhanDangKyVoiBoGDDT` | Xác nhận đơn vị đã đăng ký với Bộ Giáo dục và Đào tạo | Tùy chọn |

> Tổ chức phải upload đủ 2 giấy tờ BẮT BUỘC trước khi được phép nộp hồ sơ. Giấy tờ tùy chọn giúp tăng điểm uy tín ban đầu khi Admin phê duyệt (+50 điểm/tờ có chữ ký số hợp lệ).

---

### 2.2. Đăng ký trở thành Nhà tuyển dụng (Verifier)

#### 2.2.1. Điều kiện tiên quyết

- Tổ chức đã có tài khoản Account trên hệ thống.
- Tổ chức chưa ở trạng thái Verifier hoặc đang chờ duyệt.

#### 2.2.2. Luồng chính

| Bước | Hành động | Kết quả / Ghi chú |
|---|---|---|
| 1 | Doanh nghiệp điền thông tin: **tên công ty** (bắt buộc), địa chỉ (bắt buộc), lĩnh vực hoạt động (bắt buộc). Email và số điện thoại được lấy tự động từ tài khoản Account — chỉ cần cung cấp tường minh nếu tổ chức muốn dùng thông tin **khác** với tài khoản. | `YeuCauDangKy` được tạo với `TrangThai = Nhap`. |
| 2 | Upload tối thiểu 1 giấy tờ bắt buộc (xem bảng 2.2.4). | Bản ghi `GiayPhepNhaTuyenDung` được tạo, `TrangThaiXacMinhGiayPhep = ChoXacMinh`. Không yêu cầu ngày hết hiệu lực (khác CSDT). |
| 3 | **[Tự động]** Hệ thống verify chữ ký số. | `ThongTinChuKySo` được cập nhật (cùng cơ chế với `GiayPhepCSDT`). |
| 4 | Doanh nghiệp xác nhận nộp hồ sơ. | `TrangThaiYeuCauDangKy = DaGui`. `ThoiGianNop` được ghi nhận. Admin nhận thông báo. |
| 5 | Admin xét duyệt. | Tương tự bước 6–7 của Issuer. Khi duyệt: raise `NhaTuyenDungApprovedEvent` → Application handler tạo `NhaTuyenDung` + gắn Role Verifier. `ThoiGianXetDuyet` được ghi nhận. |

#### 2.2.3. Điểm khác biệt giữa Issuer và Verifier

| Tiêu chí | Issuer (Cơ sở đào tạo) | Verifier (Nhà tuyển dụng) |
|---|---|---|
| Mục đích đăng ký | Cấp phát, hủy, thu hồi và khôi phục bằng cấp | Xác minh bằng, tuyển dụng, báo cáo gian lận |
| Giấy tờ chứng minh | Tư cách đào tạo, cấp bằng | Tư cách pháp nhân kinh doanh |
| Tác động sau duyệt | Mở quyền Issue, Revoke, quản lý sinh viên | Mở quyền Verify, đăng tin tuyển dụng |
| Smart contract | Địa chỉ ví được ghi vào `authorizedIssuers` | Không ghi on-chain, chỉ track off-chain |
| Số giấy tờ bắt buộc tối thiểu | 2 | 1 |

#### 2.2.4. Loại giấy tờ của Nhà tuyển dụng

| Loại giấy tờ | Mô tả | Yêu cầu |
|---|---|---|
| `GiayPhepDangKyKinhDoanh` | Giấy chứng nhận đăng ký doanh nghiệp do Sở KH&ĐT cấp | **BẮT BUỘC** |
| `MaSoThue` | Xác nhận mã số thuế doanh nghiệp từ cơ quan thuế | Tùy chọn |
| `CongVanXacNhanBoPhanHR` | Công văn xác nhận tổ chức có bộ phận tuyển dụng hoạt động | Tùy chọn |

---

## 3. Quy trình quản lý sinh viên (Holder)

Sinh viên (Holder) **không tự đăng ký tài khoản**. Tài khoản được tạo bởi Issuer. Một sinh viên có thể thuộc nhiều Issuer (ví dụ: học ĐH A cấp Cử nhân, tiếp tục học ThS tại ĐH B). Hệ thống nhận biết cùng 1 sinh viên qua **CCCD**.

### 3.1. Tạo sinh viên thủ công

| Bước | Hành động | Kết quả |
|---|---|---|
| 1 | Issuer vào **Quản lý Sinh viên** → **Thêm sinh viên**. | Form nhập liệu hiển thị. |
| 2 | Nhập thông tin: Họ tên, CCCD, Email. Domain `SinhVien` lưu `Ten`, `CCCD`, `Email`, `DiaChiViSinhVien` (ví blockchain do hệ thống cấp). Số điện thoại thu thập riêng cho tài khoản ControlHub Identity, không lưu trong entity `SinhVien`. | Dữ liệu được validate: CCCD đúng định dạng 12 số, email đúng định dạng. |
| 3 | **[Tự động]** Hệ thống kiểm tra CCCD đã tồn tại chưa. | Nếu đã tồn tại → trả về `{ loai: "LienKet" }`, không tạo bản ghi mới. Nếu chưa → tạo `SinhVien` + tài khoản ControlHub → trả về `{ loai: "TaoMoi" }`. |
| 4 | Sinh viên nhận email thông báo. | Nếu SV mới: nhận mật khẩu tạm, đổi mật khẩu lần đầu. Nếu SV đã tồn tại: nhận thông báo liên kết Issuer mới. |

### 3.2. Nhập hàng loạt từ Excel

1. Issuer tải template Excel mẫu từ hệ thống.
2. Điền dữ liệu: **Họ tên**, **CCCD**, **Email**. Số điện thoại có thể điền thêm — dùng cho tài khoản ControlHub, không lưu vào `SinhVien` domain.
3. Upload file. Hệ thống validate từng dòng:
   - CCCD không để trống, đúng định dạng 12 số (bắt đầu bằng `0`).
   - Email đúng định dạng.
   - Họ tên không để trống.
   - Nếu CCCD đã tồn tại → đánh dấu "liên kết" thay vì "tạo mới".
4. Hệ thống hiển thị preview: số dòng tạo mới / số dòng liên kết / số dòng lỗi.
5. Issuer xác nhận import. Hệ thống xử lý hàng loạt và gửi email cho từng sinh viên.

### 3.3. Cập nhật thông tin sinh viên

Issuer có thể cập nhật các trường: **Họ tên**, **Email** của sinh viên. Số điện thoại được quản lý bởi ControlHub Identity, không thuộc domain ChainDegree.

> Domain method: `SinhVien.CapNhatThongTinSinhVien(ten, email)` — validate tên không trống, email đúng định dạng.

### ~~3.4. Hủy liên kết sinh viên~~ *(đã loại bỏ)*

> **Quyết định thiết kế:** Domain không track explicit link CSDT↔SinhVien. Quan hệ tồn tại implicit qua `BangCap.CoSoDaoTaoCapId`. Issuer nhận dạng sinh viên bằng CCCD và có thể cấp bằng cho bất kỳ CCCD nào — không cần "liên kết" trước. Soft delete SinhVien nếu cần sẽ được xử lý ở Application layer dựa trên việc không còn bằng nào đang hoạt động.

---

## 4. Quy trình cấp bằng / chứng chỉ (Issue Credential)

### 4.1. Điều kiện tiên quyết

- Tài khoản Issuer đang hoạt động.
- CSDT có thể cấp bằng cho SV nếu CSDT đó biết **CCCD** của SV (không yêu cầu SV phải thuộc CSDT đó).
- Sinh viên chưa có bằng cùng loại (`LoaiBangCap`) và cùng lĩnh vực (`LinhVuc`) đang hiệu lực.

### 4.2. Luồng chính

| Bước | Hành động | Kết quả |
|---|---|---|
| 1 | Issuer chọn sinh viên cần cấp bằng. | Form nhập liệu hiển thị. |
| 2 | Nhập thông tin bằng cấp: Tên bằng, Loại bằng (`LoaiBangCap`), Lĩnh vực (`LinhVuc`), Điểm, Ngày cấp, Ngày hết hạn (tùy chọn). | Dữ liệu được validate. |
| 3 | Upload file PDF bằng cấp gốc (nếu có). | File được lưu vào storage, URL gán vào trường `File`. |
| 4 | **[Tự động]** Hệ thống tạo `Salt` ngẫu nhiên và tính `credentialHash = keccak256(dữ liệu chuẩn hóa + salt)`. | `MaBamXacThuc` được tính. `TrangThaiBangCap = Nhap`. |
| 5 | Issuer xác nhận cấp bằng. | Bản ghi `BangCap` ghi vào SQL Server. Tạo Outbox event. |
| 6 | **[Tự động]** Outbox worker gọi smart contract `issueCredential(credentialHash, holderAddress)`. | `TrangThaiBlockchain = ChoDuyet`. Transaction broadcast lên Besu. |
| 7 | **[Tự động]** Transaction được mine. | `MaBamGiaoDich = txHash`. `TrangThaiBlockchain = XacNhan`. `TrangThaiBangCap = DaXacNhan`. Sinh viên nhận thông báo. |

### 4.3. Luồng thay thế / Xử lý lỗi

- **Bước 6 thất bại (Besu node không phản hồi):** Outbox event giữ trạng thái `Pending`. Worker retry tối đa 5 lần, delay 30s. `TrangThaiBlockchain = ChoDuyet` — SV không thể chia sẻ bằng cho đến khi blockchain confirm.
- **Hash trùng (bằng cấp đã tồn tại trên chain):** Smart contract revert, hệ thống báo lỗi. `TrangThaiBlockchain = ThatBai`.

---

## 5. Quy trình hủy bằng (Cancel Credential)

Hủy bằng là **hủy bỏ vĩnh viễn** — bằng cấp không còn tồn tại trong hệ thống, không thể khôi phục.

### 5.1. Điều kiện tiên quyết

- Bằng cấp do Issuer này cấp.
- Chỉ Issuer đã cấp bằng đó mới có quyền hủy.

### 5.2. Luồng chính

| Bước | Hành động | Kết quả |
|---|---|---|
| 1 | Issuer vào danh sách bằng cấp → chọn bằng → nhấn **Hủy bằng**. | Dialog xác nhận hiển thị. |
| 2 | Issuer chọn `LyDoHuy` (enum) + nhập `GhiChuHuy` (tùy chọn). | Lý do được ghi nhận. |
| 3 | **[Tự động]** Hệ thống cập nhật SQL: `TrangThaiBangCap = DaHuy`, lưu `LyDoHuy` + `GhiChuHuy`. | Bằng bị đánh dấu hủy. |
| 4 | **[Tự động]** Nếu bằng đã lên blockchain: gọi smart contract `revokeCredential(credentialHash)`. | Blockchain state: revoked. |
| 5 | Sinh viên nhận thông báo bằng đã bị hủy. | — |

### 5.3. Phân loại lý do hủy (`LyDoHuy`)

| Giá trị enum | Mô tả | Ảnh hưởng UyTin |
|---|---|---|
| `LoiNhapLieu` | Nhập sai thông tin (tên, điểm, ngày cấp...) | −5 điểm |
| `NhapTrungLap` | Cấp trùng bằng cho cùng SV | −5 điểm |
| `YeuCauCuaSinhVien` | SV yêu cầu hủy | 0 điểm |
| `YeuCauCuaCSDT` | CSDT tự hủy vì lý do nội bộ | 0 điểm |
| `Khac` | Lý do khác — **bắt buộc nhập `GhiChuHuy`**, không được để trống | 0 điểm |

> **Ràng buộc:** Khi `LyDoHuy = Khac`, trường `GhiChuHuy` là bắt buộc. Hệ thống từ chối xử lý nếu `GhiChuHuy` null hoặc rỗng.

---

## 6. Quy trình thu hồi bằng (Revoke Credential)

Thu hồi bằng là **tạm ngưng hiệu lực** — bằng vẫn tồn tại, trạng thái = DaThuHoi trên blockchain. **Có thể khôi phục** (xem mục 7).

### 6.1. Điều kiện tiên quyết

- Bằng cấp đang ở trạng thái `DaXacNhan` và đã được confirm trên blockchain.
- Chỉ Issuer đã cấp bằng đó mới có quyền thu hồi (smart contract kiểm tra `msg.sender`).

### 6.2. Luồng chính

| Bước | Hành động | Kết quả |
|---|---|---|
| 1 | Issuer vào danh sách bằng cấp → chọn bằng → nhấn **Thu hồi**. | Dialog xác nhận hiển thị. |
| 2 | Issuer chọn `LyDoThuHoi` (enum) + nhập `GhiChuThuHoi` (tùy chọn). | Lý do được ghi nhận. |
| 3 | **[Tự động]** Hệ thống cập nhật SQL: `TrangThaiBangCap = DaThuHoi`, lưu `LyDoThuHoi` + `GhiChuThuHoi`. | — |
| 4 | **[Tự động]** Gọi smart contract `revokeCredential(credentialHash)`. | Blockchain: `revoked = true`. |
| 5 | Điểm uy tín Issuer bị trừ theo loại lý do (xem bảng 6.3). | — |
| 6 | Sinh viên nhận thông báo bằng đã bị thu hồi. | — |

### 6.3. Phân loại lý do thu hồi (`LyDoThuHoi`) và ảnh hưởng điểm uy tín

| Giá trị enum | Mô tả | Ảnh hưởng UyTin |
|---|---|---|
| `ViPhamHocThuat` | SV vi phạm quy chế học thuật sau khi cấp bằng | 0 điểm |
| `ViPhamDaoDuc` | SV vi phạm đạo đức nghiêm trọng | 0 điểm |
| `BangGia` | Xác nhận bằng giả / gian lận | **−200 điểm** |
| `GianLanXacNhan` | Gian lận trong quá trình xác nhận bằng | **−200 điểm** |
| `ThayDoiQuyDinh` | Thay đổi quy định cấp bằng của tổ chức | −5 điểm |
| `QuyetDinhPhapLy` | Quyết định từ cơ quan pháp luật | 0 điểm |
| `Khac` | Lý do khác — **bắt buộc nhập `GhiChuThuHoi`**, không được để trống | 0 điểm |

> **Ràng buộc:** Khi `LyDoThuHoi = Khac`, trường `GhiChuThuHoi` là bắt buộc. Hệ thống từ chối xử lý nếu `GhiChuThuHoi` null hoặc rỗng.

---

## 7. Quy trình khôi phục bằng đã thu hồi (Restore Credential)

### 7.1. Điều kiện tiên quyết

- Bằng cấp đang ở trạng thái `DaThuHoi`.
- Chỉ Issuer đã cấp bằng đó mới có quyền khôi phục.

### 7.2. Luồng chính

| Bước | Hành động | Kết quả |
|---|---|---|
| 1 | Issuer vào danh sách bằng đã thu hồi → chọn bằng → nhấn **Khôi phục**. | Dialog xác nhận hiển thị. |
| 2 | Issuer chọn `LyDoKhoiPhuc` (enum) + nhập `GhiChuKhoiPhuc`. Nếu `LyDoKhoiPhuc = Khac`, `GhiChuKhoiPhuc` là **bắt buộc**. | — |
| 3 | **[Tự động]** Hệ thống ghi nhận `LyDoKhoiPhuc` + `GhiChuKhoiPhuc` vào bản ghi cũ (giữ nguyên trạng thái `DaThuHoi`). Tạo bản ghi `BangCap` mới với dữ liệu cũ + `Salt` mới → tính `credentialHash` mới. | Bản cũ: vẫn `DaThuHoi` trên blockchain (lịch sử kiểm toán). Hash mới được tạo cho bản mới. |
| 4 | **[Tự động]** Gọi smart contract `issueCredential(newCredentialHash, holderAddress)`. | Bản mới lên blockchain. Bản cũ vẫn revoked trên chain (lịch sử). |
| 5 | Bản mới: `TrangThaiBangCap = ChuaXacNhan`, chờ Outbox confirm. Sau khi blockchain xác nhận: `TrangThaiBangCap = DaXacNhan`. Sinh viên nhận thông báo. | — |

> **Ràng buộc:** Khi `LyDoKhoiPhuc = Khac`, trường `GhiChuKhoiPhuc` là bắt buộc. Hệ thống từ chối xử lý nếu `GhiChuKhoiPhuc` null hoặc rỗng.

> **Lưu ý:** Khôi phục thực chất là cấp lại bản mới (hash mới) trên blockchain. Bản cũ (đã revoke) vẫn tồn tại trên chain như lịch sử kiểm toán — trạng thái `DaThuHoi` của bản cũ không thay đổi. Lý do và ghi chú khôi phục lưu tại SQL Server, không ghi lên blockchain.

---

## 8. Quy trình cập nhật bằng cấp (Update Credential)

### 8.1. Nguyên tắc

`credentialHash` không được phép thay đổi sau khi tạo. Khi CSDT cần sửa thông tin bằng cấp, hệ thống tự động thực hiện: **thu hồi bản cũ (ghi lý do) + cấp lại bản mới (hash mới)** trong cùng 1 thao tác.

### 8.2. Luồng chính

| Bước | Hành động | Kết quả |
|---|---|---|
| 1 | Issuer chọn bằng cần sửa → nhấn **Cập nhật**. | Form hiển thị thông tin hiện tại, cho phép chỉnh sửa. |
| 2 | Issuer sửa thông tin cần thay đổi. | — |
| 3 | **[Tự động]** Hệ thống thu hồi bản cũ trên blockchain: `revokeCredential(oldHash)` + ghi lý do "Cập nhật thông tin". | Bản cũ: `TrangThaiBangCap = DaThuHoi`, `LyDoThuHoi = ThayDoiQuyDinh`. |
| 4 | **[Tự động]** Hệ thống tạo bản mới: `Salt` mới → `credentialHash` mới → `issueCredential(newHash, holderAddress)`. | Bản mới: `TrangThaiBangCap = DaXacNhan`. |
| 5 | Sinh viên nhận thông báo bằng đã được cập nhật. | — |

---

## 9. Quy trình xác minh bằng (Verify Credential)

Bất kỳ ai cũng có thể xác minh bằng cấp (giao diện công khai). Tuy nhiên, `NhatKyXacMinh` chỉ được ghi khi Nhà tuyển dụng đã đăng nhập thực hiện xác minh.

### 9.1. Xác minh qua QR Code / Link

| Bước | Hành động | Kết quả |
|---|---|---|
| 1 | Sinh viên chia sẻ QR code hoặc link xác minh. | — |
| 2 | Người xác minh scan QR / mở link. | Màn hình xác minh hiển thị. |
| 3 | **[Tự động]** Hệ thống query smart contract `getCredential(credentialHash)`. | Blockchain trả về: `{ exists, revoked, issuerAddress, timestamp }`. |
| 4 | Hệ thống hiển thị kết quả. | ✅ HỢP LỆ / ❌ ĐÃ THU HỒI / ⚠ KHÔNG TỒN TẠI / 🚫 GIAN LẬN |
| 5 | **[Nếu NTD đã đăng nhập]** Ghi `NhatKyXacMinh`: NTD nào, bằng nào, thời gian, `KetQuaXacMinh`, `MaBamXacMinh`. | NhatKyXacMinh chỉ ghi cho NTD đã đăng nhập. |
| 6 | **[Nếu HỢP LỆ]** Điểm uy tín Issuer +1. | UyTinToChuc.SoLuongXacMinhHopLe tăng. |

### 9.2. Xác minh thủ công bằng Hash

Người xác minh nhập trực tiếp `credentialHash` vào ô tìm kiếm → tương tự bước 3–6 ở mục 9.1.

### 9.3. Quyền xem thông tin chi tiết

| Thông tin | NTD đã đăng nhập | Công khai |
|---|---|---|
| Kết quả HỢP LỆ / THU HỒI / KHÔNG TỒN TẠI | ✓ | ✓ |
| Tên sinh viên, tên Issuer, loại bằng, ngày cấp | ✓ | ✓ |
| `txHash` và link Blockchain Explorer | ✓ | ✓ |
| Lịch sử các lần xác minh trước | ✓ | ✗ |
| Điểm chi tiết, link PDF gốc | ✓ | ✗ |

---

## 10. Quy trình báo cáo gian lận (Report Fraud)

### 10.1. Ai có thể báo cáo

- **Sinh viên:** Báo cáo gian lận trong bằng cấp của bản thân (ví dụ: phát hiện bằng bị cấp sai hoặc ai đó sử dụng bằng của mình).
- **Nhà tuyển dụng:** Báo cáo gian lận khi phát hiện bằng cấp có dấu hiệu giả mạo trong quá trình tuyển dụng.

### 10.2. Luồng chính

| Bước | Hành động | Kết quả |
|---|---|---|
| 1 | SV hoặc NTD chọn bằng cấp cần báo cáo → nhấn **Báo cáo gian lận**. | Form báo cáo hiển thị. |
| 2 | Nhập lý do (`LyDo`), ghi chú chi tiết (`GhiChu`). | — |
| 3 | **[Tự động]** Hệ thống tạo `BaoCaoGianLan` với `TrangThaiBaoCao = ChoXuLy`. | Admin nhận thông báo. |
| 4 | Admin xem xét báo cáo. Cập nhật `TrangThaiBaoCao = DangXuLy`. | — |
| 5a | **[Xác nhận gian lận]** Admin yêu cầu Issuer thu hồi bằng. `TrangThaiBaoCao = DaXuLy`. | UyTin Issuer −200. |
| 5b | **[Không gian lận]** `TrangThaiBaoCao = TuChoi`. | Không ảnh hưởng. |

### 10.3. Ảnh hưởng đến UyTinToChuc

Mỗi báo cáo gian lận được xác nhận (DaXuLy) sẽ tăng `SoLuongBangCapBiBaoCaoGianLan` trên `UyTinToChuc` của Issuer đã cấp bằng đó.

---

## 11. Quy trình tuyển dụng hỗ trợ bởi AI

### 11.1. Đăng tin tuyển dụng

Verifier tạo `ThongTinTuyenDung`: Tên vị trí, Lĩnh vực (`LinhVuc`), Mô tả công việc, Thời hạn ứng tuyển.

Tin tuyển dụng được hiển thị công khai cho Holder xem và ứng tuyển.

### 11.2. Quy trình ứng tuyển của sinh viên

| Bước | Hành động | Kết quả |
|---|---|---|
| 1 | Sinh viên xem danh sách `ThongTinTuyenDung`. | — |
| 2 | Sinh viên chọn bằng cấp muốn đính kèm → nhấn **Ứng tuyển**. | `HoSoUngTuyen` được tạo với `TrangThaiUngTuyen = ChoXem`. `BangCapUngTuyen` được tạo cho mỗi bằng đính kèm. |
| 3 | **[Tự động]** Hệ thống xác minh từng bằng trên blockchain trước khi cho phép nộp. | Chỉ bằng `TrangThaiBangCap = DaXacNhan` mới được đính kèm. |
| 4 | Hồ sơ được gửi đến Verifier. | — |

### 11.3. Quản lý hồ sơ ứng tuyển (khi còn ChoXem)

Khi hồ sơ ở trạng thái `ChoXem` (NTD chưa xem), sinh viên có thể:
- **Thêm bằng cấp:** `ThemBangCapUngTuyen()` — đính kèm thêm bằng hợp lệ.
- **Xóa bằng cấp:** `XoaBangCapUngTuyen()` — bỏ bằng không phù hợp.
- **Thu hồi hồ sơ:** `ThuHoiHoSoUngTuyen()` — hủy toàn bộ hồ sơ. `TrangThaiUngTuyen = DaThuHoi`.

> Khi NTD đã xem (`DaXem`), SV không thể thay đổi hồ sơ nữa.

### 11.4. AI phân tích độ phù hợp

NTD có thể chạy phân tích AI nhiều lần cho cùng 1 `ThongTinTuyenDung`. Mỗi lần tạo 1 `KetQuaPhanTich` mới (lịch sử phân tích).

| Thuộc tính | Ý nghĩa |
|---|---|
| `PhanTramPhuHop` | Điểm % phù hợp. Tính dựa trên: lĩnh vực, loại bằng, điểm số, UyTin của Issuer. |
| `KetLuan` (JSON) | Phân tích chi tiết: điểm mạnh, điểm thiếu, đề xuất cho Verifier. |
| `ThoiGianPhanTich` | Thời điểm chạy phân tích. |

### 11.5. NTD duyệt hồ sơ

NTD xem danh sách hồ sơ + kết quả AI gợi ý → quyết định:
- `DaXem` → NTD đã mở xem hồ sơ.
- `ChapNhan` → NTD chấp nhận ứng viên.
- `TuChoi` → NTD từ chối ứng viên.

Kết quả AI chỉ mang tính tham khảo — quyết định cuối thuộc về Verifier.

---

## 12. Phụ lục: Trạng thái & ràng buộc hệ thống

### 12.1. Vòng đời trạng thái YeuCauDangKy

| Trạng thái | Mô tả |
|---|---|
| `Nhap` | Tổ chức đang điền form, chưa nộp chính thức. |
| `DaGui` | Đã nộp hồ sơ, chờ Admin xem xét. |
| `XacNhan` | Admin phê duyệt. Hệ thống tạo CSDT/NTD. |
| `TuChoi` | Admin từ chối. Tổ chức có thể nộp hồ sơ mới. |

### 12.2. Vòng đời trạng thái BangCap

> **Lưu ý:** Tên enum trong domain code là `ChuaXacNhan` (không phải `Nhap`).

| TrangThaiBangCap | TrangThaiBlockchain | Mô tả |
|---|---|---|
| `ChuaXacNhan` | `ChoDuyet` | Bằng vừa tạo, đang chờ ghi blockchain. |
| `DaXacNhan` | `XacNhan` | Bằng đã lên blockchain, đang hiệu lực. |
| `DaThuHoi` | `XacNhan` | Bằng bị thu hồi trên blockchain. Có thể khôi phục. |
| `DaHuy` | `XacNhan` hoặc `N/A` | Bằng bị hủy vĩnh viễn. Không khôi phục. |
| `ChuaXacNhan` | `ThatBai` | Ghi blockchain thất bại. Cần retry. |

### 12.3. Vòng đời TrangThaiXacMinhGiayPhep

| Trạng thái | Mô tả |
|---|---|
| `ChoXacMinh` | Giấy phép vừa upload, chờ Admin xem xét. |
| `DaXacMinh` | Admin đã xác nhận hợp lệ. |
| `TuChoi` | Admin từ chối. Tổ chức có thể upload lại. |
| `TaiLenLai` | Tổ chức upload giấy phép mới thay thế → chuyển về `ChoXacMinh`. |

### 12.4. Cơ chế điểm uy tín (UyTinToChuc)

| Sự kiện | Issuer | Verifier |
|---|---|---|
| Khởi tạo khi được Admin phê duyệt | `+50 × số giấy phép đã nộp` (tối thiểu 100 với 2 giấy bắt buộc) | `+50 × số giấy phép đã nộp` (tối thiểu 50 với 1 giấy bắt buộc) |
| Mỗi bằng cấp được cấp thành công lên blockchain | +2 | N/A |
| Mỗi lần NTD xác minh bằng hợp lệ | +1 | N/A |
| Hủy bằng do lỗi nhập liệu / trùng lặp | −5 | N/A |
| Thu hồi bằng do thay đổi quy định | −5 | N/A |
| Bằng bị xác nhận gian lận (Admin `XacNhanGianLan`) | **−200** | N/A |

> UyTin là tín hiệu tin cậy: AI sử dụng để phân tích mức độ tin cậy của bằng cấp, NTD tham khảo để đánh giá CSDT. UyTin **không** ảnh hưởng đến quyền cấp bằng hay trạng thái hoạt động của tổ chức.

### 12.5. Xếp hạng uy tín (HangUyTin)

| Hạng | Điều kiện |
|---|---|
| `ChuaCoGiayPhep` | Chưa upload giấy phép |
| `Dong` | Điểm < 100 |
| `Bac` | Điểm 100–299 |
| `Vang` | Điểm 300–499 |
| `DaCoGiayPhep` | Điểm ≥ 500 |

### 12.6. Ràng buộc nghiệp vụ quan trọng

- **Consistency đảm bảo:** Mọi ghi blockchain đều thông qua Outbox Pattern. Không có trường hợp SQL thành công mà blockchain bị bỏ qua.
- **Bất biến của hash:** `credentialHash` không được phép thay đổi sau khi tạo. Muốn sửa → thu hồi bản cũ + cấp lại bản mới (hash mới).
- **Không xóa cứng bằng cấp:** Bằng chỉ được soft delete hoặc thu hồi. Dữ liệu lịch sử luôn được giữ lại.
- **Phân quyền smart contract:** `authorizedIssuers[msg.sender]` — chỉ Issuer đã whitelist mới gọi được `issueCredential()` / `revokeCredential()`.
- **Sinh viên thuộc nhiều CSDT:** Nhận biết qua CCCD. Không có explicit link CSDT↔SinhVien — quan hệ tồn tại implicit qua `BangCap.CoSoDaoTaoCapId`. Mỗi CSDT nhận dạng sinh viên bằng CCCD và có thể cấp bằng độc lập.
- **Admin chỉ duyệt và cấp quyền:** Admin không tạm khóa tổ chức. Không có trạng thái Suspended.

---

## 13. Ghi chú — Spec vs Codebase (cần xem xét triển khai)

Mục này ghi lại các điểm **đặc tả thiết kế** chưa được phản ánh đầy đủ trong domain code hiện tại. Nhóm cần quyết định xem các mục này có nằm trong phạm vi sprint hiện tại không.

### 13.1. Domain code thiếu so với spec

| # | Vấn đề | Vị trí | Ghi chú |
|---|--------|--------|---------|
| 1 | `LyDoTuChoi` chỉ có 4 giá trị trong code (`GiayToKhongHopLe`, `ThongTinKhongChinhXac`, `KhongDuGiayTo`, `Khac`), trong khi class diagram thiết kế 7 giá trị | `Domain/QuanLyToChuc/Enums/LyDoTuChoi.cs` | Cần bổ sung 3 giá trị: `ThongTinToChucKhongKhopVoiGiayTo`, `GiayPhepHetHan`, `ToChucDangTrongDanhSachDen` |
| 2 | `NhaTuyenDung` không có `UyTinToChuc` ValueObject — chỉ có `HangUyTin` enum thuần | `Domain/TuyenDung/Aggregates/NhaTuyenDung.cs` | `UyTinToChuc` với đầy đủ điểm số và lịch sử cần được thêm vào, tương tự như `CoSoDaoTao` |
| 3 | `NhaTuyenDung.BaoCaoGianLanBangCap()` nhận `string lyDo` thay vì `LyDoBaoCaoGianLan` enum; method không tạo aggregate `BaoCaoGianLan` mà chỉ raise event | `Domain/TuyenDung/Aggregates/NhaTuyenDung.cs` | Cần sửa kiểu tham số và logic tạo aggregate |
| 4 | `NhaTuyenDung.CapNhatThongTinNhaTuyenDung(ten, diaChi)` chỉ nhận 2 tham số — không lưu `sdt`, `email`, `website`, `logo`, `moTa`, `maSoThue` | `Domain/TuyenDung/Aggregates/NhaTuyenDung.cs` | Cần quyết định: các trường này có thuộc domain không hay nằm ở Application/Infrastructure layer? |
| 5 | `TaiLenLaiGiayPhep` chỉ được implement cho CSDT (`GiayPhepCSDT`), không có cho NTD (`GiayPhepNhaTuyenDung`) | `Domain/QuanLyToChuc/Aggregates/YeuCauDangKy.cs` | Nếu NTD cần nộp lại giấy phép sau khi bị từ chối, cần bổ sung method tương ứng |
| 6 | `YeuCauDangKy` domain không lưu `Email`, `SDT`, `DiaChi` của tổ chức — các trường này có trong ERD nhưng không có trong aggregate | `Domain/QuanLyToChuc/Aggregates/YeuCauDangKy.cs` | Các trường này có thể lấy từ tài khoản ControlHub Identity. Cần làm rõ có cần lưu redundant trong `YeuCauDangKy` không |

### 13.2. ERD/Diagram chưa phản ánh đúng codebase

| # | Vấn đề | Vị trí | Ghi chú |
|---|--------|--------|---------|
| 1 | ERD có bảng `CSDTSinhVien` (junction table explicit) — đã loại bỏ theo quyết định thiết kế | `docs/diagram/erd.mmd` | Xóa bảng này khỏi ERD |
| 2 | ERD `BaoCaoGianLan.LyDo` kiểu `string` — domain dùng `LyDoBaoCaoGianLan` enum | `docs/diagram/erd.mmd` | Cập nhật kiểu sang enum |
| 3 | ERD `YeuCauDangKy` có các cột `Email`, `SDT`, `DiaChi` — domain code không lưu | `docs/diagram/erd.mmd` | Quyết định bỏ hay giữ tùy theo item 13.1.6 |
| 4 | ERD `YeuCauDangKy` thiếu `GhiChuDuyet`, `ThoiGianNop`, `ThoiGianXetDuyet` | `docs/diagram/erd.mmd` | Bổ sung vào ERD |
| 5 | Class diagram `CoSoDaoTao` vẫn có method `HuyLienKetSinhVien()` — đã loại bỏ | `docs/diagram/class.mmd` | Xóa method này |
| 6 | Class diagram `SinhVien` có `TaoMaQR()`, `ChiaSeDuongDan()` — đây là presentation concern, không phải domain behavior | `docs/diagram/class.mmd` | Xem xét chuyển sang Application/API layer |

### 13.3. Các tính năng trong spec chưa có trong codebase

| # | Tính năng | Ghi chú |
|---|-----------|---------|
| 1 | **Ký số giấy phép (ChuKySo)** — `ThongTinChuKySo` là ValueObject có trong code nhưng chưa có logic tích hợp ký số thực sự | Cần xác định flow ký số: CSDT ký file trước khi upload, hay hệ thống ký? |
| 2 | **Outbox Pattern** — đề cập trong spec nhưng chưa có Infrastructure implementation | Cần implement `OutboxMessage`, `OutboxProcessor` |
| 3 | **AI Reranker / JD Parsing** — module tuyển dụng AI chưa có domain code tương ứng | Có thể nằm trong Application/Infrastructure layer |
| 4 | **Webhook thông báo** — spec đề cập notify CSDT/NTD khi admin duyệt/từ chối | Domain event đã có; cần Infrastructure handler gửi webhook/email |