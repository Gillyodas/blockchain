# Contract API — ChainDegree

> **Phiên bản:** 1.0 | **Ngày:** 2026-04-06
> **Nguồn tổng hợp:** `PhanTichAPI.docx`, `hopdongAPI.docx`, `DacTaQuyTrinhNghiepVu.md`, domain code
> **Lưu ý:** File này là nguồn sự thật. Các enum và tên trường lấy từ domain code C#, không phải từ tài liệu cũ (có thể chứa giá trị lỗi thời).

---

## Quy ước chung

| Mục | Giá trị |
|-----|---------|
| Base URL | `https://<host>/api/v1` |
| Content-Type mặc định | `application/json` |
| Auth | Bearer Token (JWT) — trừ endpoint ghi rõ "Không bắt buộc" |
| Ngày giờ | ISO 8601, UTC — ví dụ `2025-06-01T00:00:00Z` |
| Lỗi | `{ "code": "string", "message": "string" }` |

**Vai trò (Role):**
- **Admin** — quản trị hệ thống
- **Issuer** — Cơ sở đào tạo đã được duyệt
- **Verifier** — Nhà tuyển dụng đã được duyệt
- **Holder** — Sinh viên (tài khoản do Issuer tạo)
- **Account** — tài khoản bất kỳ đã xác thực email

---

## Module 1: Đăng ký & Xác thực Tổ chức

### 1.1 Đăng ký trở thành Cơ sở đào tạo

**`POST /yeu-cau-dang-ky/co-so-dao-tao`**

- **Auth:** Account
- **Điều kiện:** Chưa là Issuer và chưa có yêu cầu đang chờ duyệt
- **Ghi chú nguồn dữ liệu:** `email` và `sdt` mặc định lấy từ hồ sơ tài khoản đang đăng nhập. Chỉ cần cung cấp tường minh nếu email/sdt của tổ chức **khác** với tài khoản (ví dụ: đăng ký qua hợp đồng hợp tác do admin hỗ trợ).

**Request Body:**

| Trường | Kiểu | Bắt buộc | Mô tả |
|--------|------|:--------:|-------|
| tenToChuc | string | ✅ | Tên tổ chức |
| diaChi | string | ✅ | Địa chỉ tổ chức |
| maSoTruong | string | ❌ | Mã số trường (nếu có) |
| website | string | ❌ | Website tổ chức |
| email | string | ❌ | Email liên hệ tổ chức — mặc định lấy từ tài khoản |
| sdt | string | ❌ | Số điện thoại tổ chức — mặc định lấy từ tài khoản |

**Responses:**

| Status | Trường hợp | Dữ liệu trả về |
|--------|-----------|----------------|
| 201 Created | Tạo yêu cầu thành công | `{ yeuCauDangKyId: guid }` |
| 400 Bad Request | Thiếu trường bắt buộc | Error |
| 401 Unauthorized | Chưa đăng nhập | — |
| 409 Conflict | Đã có yêu cầu đang chờ hoặc đã là Issuer | Error |

---

### 1.2 Đăng ký trở thành Nhà tuyển dụng

**`POST /yeu-cau-dang-ky/nha-tuyen-dung`**

- **Auth:** Account
- **Điều kiện:** Chưa là Verifier và chưa có yêu cầu đang chờ duyệt
- **Ghi chú nguồn dữ liệu:** `email` và `sdt` mặc định lấy từ hồ sơ tài khoản đang đăng nhập. Chỉ cần cung cấp tường minh nếu khác với tài khoản.

**Request Body:**

| Trường | Kiểu | Bắt buộc | Mô tả |
|--------|------|:--------:|-------|
| tenToChuc | string | ✅ | Tên công ty |
| diaChi | string | ✅ | Địa chỉ công ty |
| linhVucHoatDong | string | ✅ | Lĩnh vực hoạt động |
| email | string | ❌ | Email liên hệ tổ chức — mặc định lấy từ tài khoản |
| sdt | string | ❌ | Số điện thoại tổ chức — mặc định lấy từ tài khoản |

**Responses:**

| Status | Trường hợp | Dữ liệu trả về |
|--------|-----------|----------------|
| 201 Created | Tạo yêu cầu thành công | `{ yeuCauDangKyId: guid }` |
| 400 Bad Request | Thiếu trường bắt buộc | Error |
| 401 Unauthorized | Chưa đăng nhập | — |
| 409 Conflict | Đã có yêu cầu đang chờ hoặc đã là Verifier | Error |

---

### 1.3 Tải lên giấy phép cho yêu cầu đăng ký

**`POST /yeu-cau-dang-ky/{id}/giay-phep`**

- **Auth:** Account (chủ sở hữu YeuCauDangKy)
- **Content-Type:** `multipart/form-data`
- **Ghi chú:** CSDT cần tối thiểu 2 giấy phép bắt buộc (`GiayPhepHoatDongGiaoDuc` + `QuyetDinhThanhLapTruong`). NTD cần tối thiểu `GiayPhepDangKyKinhDoanh`.

**Path Parameters:**

| Tham số | Kiểu | Mô tả |
|---------|------|-------|
| id | guid | ID của YeuCauDangKy |

**Request Body (form-data):**

| Trường | Kiểu | Bắt buộc | Mô tả |
|--------|------|:--------:|-------|
| file | file (PDF) | ✅ | File giấy tờ PDF (hỗ trợ chữ ký số) |
| kieuGiayPhep | int | ✅ | Xem enum `KieuGiayPhepCSDT` hoặc `KieuGiayPhepNTD` |
| ngayHetHan | datetime | ✅ (CSDT) / ❌ (NTD) | Ngày hết hiệu lực của giấy phép — bắt buộc với CSDT, không dùng với NTD |

**Responses:**

| Status | Trường hợp | Dữ liệu trả về |
|--------|-----------|----------------|
| 201 Created | Upload thành công | `{ giayPhepId: guid, trangThaiXacMinh: "ChoXacMinh", thongTinChuKySo: { coChuKySo: bool, hopLe: bool, nhaCungCap: string, nhaCungCapDuocTinTuong: bool, ngayHetHan: datetime?, xacMinhLuc: datetime } }` |
| 400 Bad Request | File không hợp lệ hoặc sai trạng thái yêu cầu | Error |
| 403 Forbidden | Không phải chủ sở hữu | Error |
| 404 Not Found | YeuCauDangKy không tồn tại | Error |

---

### 1.4 Tải lại giấy phép bị từ chối

**`PUT /yeu-cau-dang-ky/{id}/giay-phep/{giayPhepId}`**

- **Auth:** Account (chủ sở hữu YeuCauDangKy)
- **Content-Type:** `multipart/form-data`
- **Điều kiện:** `TrangThaiXacMinhGiayPhep == TuChoi`

**Path Parameters:**

| Tham số | Kiểu | Mô tả |
|---------|------|-------|
| id | guid | ID của YeuCauDangKy |
| giayPhepId | guid | ID của giấy phép bị từ chối |

**Request Body (form-data):**

| Trường | Kiểu | Bắt buộc | Mô tả |
|--------|------|:--------:|-------|
| file | file (PDF) | ✅ | File giấy phép mới |
| kieuGiayPhep | int | ✅ | Loại giấy phép (phải khớp với giấy phép gốc) |

**Responses:**

| Status | Trường hợp | Dữ liệu trả về |
|--------|-----------|----------------|
| 200 OK | Tải lại thành công, hệ thống tự verify | `{ trangThaiXacMinh: "ChoXacMinh", thongTinChuKySo: { ... } }` |
| 400 Bad Request | Trạng thái giấy phép không phải TuChoi | Error |
| 403 Forbidden | Không phải chủ sở hữu | Error |
| 404 Not Found | Giấy phép hoặc YeuCauDangKy không tồn tại | Error |

---

### 1.5 Xác nhận nộp hồ sơ đăng ký

**`POST /yeu-cau-dang-ky/{id}/xac-nhan-nop`**

- **Auth:** Account (chủ sở hữu YeuCauDangKy)
- **Điều kiện:** Đã upload đủ giấy tờ bắt buộc; trạng thái yêu cầu là `Nhap`

**Path Parameters:**

| Tham số | Kiểu | Mô tả |
|---------|------|-------|
| id | guid | ID của YeuCauDangKy |

**Request Body:** Không có

**Responses:**

| Status | Trường hợp | Dữ liệu trả về |
|--------|-----------|----------------|
| 200 OK | Nộp hồ sơ thành công | `{ trangThai: "DaGui" }` |
| 400 Bad Request | Chưa đủ giấy tờ bắt buộc hoặc trạng thái không hợp lệ | Error |
| 403 Forbidden | Không phải chủ sở hữu | Error |
| 404 Not Found | YeuCauDangKy không tồn tại | Error |

---

### 1.6 Admin phê duyệt hồ sơ đăng ký

**`POST /admin/yeu-cau-dang-ky/{id}/duyet`**

- **Auth:** Admin
- **Điều kiện:** Trạng thái yêu cầu là `DaGui`
- **Hệ thống tự động:** Tạo `CoSoDaoTao` hoặc `NhaTuyenDung`; gắn Role; khởi tạo `UyTinToChuc`; gửi email thông báo

**Path Parameters:**

| Tham số | Kiểu | Mô tả |
|---------|------|-------|
| id | guid | ID của YeuCauDangKy |

**Request Body:**

| Trường | Kiểu | Bắt buộc | Mô tả |
|--------|------|:--------:|-------|
| ghiChu | string | ❌ | Ghi chú của Admin |

**Responses:**

| Status | Trường hợp | Dữ liệu trả về |
|--------|-----------|----------------|
| 200 OK | Phê duyệt thành công | `{ toChucId: guid, trangThai: "XacNhan" }` |
| 400 Bad Request | Trạng thái không phải DaGui | Error |
| 404 Not Found | YeuCauDangKy không tồn tại | Error |

---

### 1.7 Admin từ chối hồ sơ đăng ký

**`POST /admin/yeu-cau-dang-ky/{id}/tu-choi`**

- **Auth:** Admin
- **Điều kiện:** Trạng thái yêu cầu là `DaGui`

**Path Parameters:**

| Tham số | Kiểu | Mô tả |
|---------|------|-------|
| id | guid | ID của YeuCauDangKy |

**Request Body:**

| Trường | Kiểu | Bắt buộc | Mô tả |
|--------|------|:--------:|-------|
| lyDoTuChoi | int (enum `LyDoTuChoi`) | ✅ | Xem bảng enum |
| ghiChuTuChoi | string | ✅ | Ghi chú lý do từ chối |

**Responses:**

| Status | Trường hợp | Dữ liệu trả về |
|--------|-----------|----------------|
| 200 OK | Từ chối thành công | `{ trangThai: "TuChoi" }` |
| 400 Bad Request | Thiếu ghiChuTuChoi hoặc trạng thái không phải DaGui | Error |
| 404 Not Found | YeuCauDangKy không tồn tại | Error |

---

## Module 2: Quản lý Sinh viên

> Sinh viên không tự đăng ký — tài khoản do Issuer tạo.

### 2.1 Tạo sinh viên mới

**`POST /co-so-dao-tao/{csdt_id}/sinh-vien`**

- **Auth:** Issuer (chủ sở hữu CSDT)

**Path Parameters:**

| Tham số | Kiểu | Mô tả |
|---------|------|-------|
| csdt_id | guid | ID của CoSoDaoTao |

**Request Body:**

| Trường | Kiểu | Bắt buộc | Mô tả |
|--------|------|:--------:|-------|
| ten | string | ✅ | Họ tên sinh viên |
| cccd | string | ✅ | Căn cước công dân 12 số (bắt đầu bằng 0) |
| email | string | ✅ | Email sinh viên |
| diaChiViSinhVien | string | ✅ | Địa chỉ ví blockchain của sinh viên (do ControlHub cấp) |

**Responses:**

| Status | Trường hợp | Dữ liệu trả về |
|--------|-----------|----------------|
| 201 Created | Tạo SV mới (CCCD chưa có trong hệ thống) | `{ sinhVienId: guid, loai: "TaoMoi" }` |
| 200 OK | CCCD đã tồn tại — liên kết SV với CSDT này | `{ sinhVienId: guid, loai: "LienKet" }` |
| 400 Bad Request | CCCD sai định dạng, Email sai định dạng, thiếu trường | Error |
| 403 Forbidden | Không phải Issuer của CSDT này | Error |

---

### 2.2 Nhập sinh viên hàng loạt từ Excel

**`POST /co-so-dao-tao/{csdt_id}/sinh-vien/import`**

- **Auth:** Issuer
- **Content-Type:** `multipart/form-data`
- **Ghi chú:** Bước 1 — validate + trả về preview. Không lưu ngay.

**Request Body (form-data):**

| Trường | Kiểu | Bắt buộc | Mô tả |
|--------|------|:--------:|-------|
| file | file (.xlsx) | ✅ | File Excel theo template hệ thống |

**Responses:**

| Status | Trường hợp | Dữ liệu trả về |
|--------|-----------|----------------|
| 200 OK | Validate thành công | `{ importId: string, soTaoMoi: int, soLienKet: int, soLoi: int, chiTietLoi: [{ dong: int, lyDo: string }] }` |
| 400 Bad Request | File sai format | Error |

---

### 2.3 Xác nhận import hàng loạt

**`POST /co-so-dao-tao/{csdt_id}/sinh-vien/import/xac-nhan`**

- **Auth:** Issuer
- **Ghi chú:** Bước 2 — xác nhận sau khi xem preview.

**Request Body:**

| Trường | Kiểu | Bắt buộc | Mô tả |
|--------|------|:--------:|-------|
| importId | string | ✅ | ID phiên import đã preview (server cấp ở bước 1) |

**Responses:**

| Status | Trường hợp | Dữ liệu trả về |
|--------|-----------|----------------|
| 200 OK | Import thành công | `{ soTaoMoi: int, soLienKet: int }` |
| 400 Bad Request | importId không hợp lệ hoặc hết hạn | Error |

---

### 2.4 Tải template Excel

**`GET /co-so-dao-tao/sinh-vien/template`**

- **Auth:** Issuer

**Responses:**

| Status | Trường hợp | Dữ liệu trả về |
|--------|-----------|----------------|
| 200 OK | Tải thành công | File `.xlsx` (binary download) |

---

### 2.5 Cập nhật thông tin sinh viên

**`PUT /co-so-dao-tao/{csdt_id}/sinh-vien/{sv_id}`**

- **Auth:** Issuer (quản lý SV này)
- **Ghi chú:** Chỉ cập nhật được `Ten` và `Email`. CCCD không thể thay đổi. `SoDienThoai` quản lý ở tầng Identity (ngoài domain).

**Path Parameters:**

| Tham số | Kiểu | Mô tả |
|---------|------|-------|
| csdt_id | guid | ID của CoSoDaoTao |
| sv_id | guid | ID của SinhVien |

**Request Body:**

| Trường | Kiểu | Bắt buộc | Mô tả |
|--------|------|:--------:|-------|
| ten | string | ✅ | Họ tên mới |
| email | string | ✅ | Email mới |

**Responses:**

| Status | Trường hợp | Dữ liệu trả về |
|--------|-----------|----------------|
| 200 OK | Cập nhật thành công | `{ sinhVienId: guid }` |
| 400 Bad Request | Dữ liệu không hợp lệ | Error |
| 403 Forbidden | Issuer không quản lý SV này | Error |
| 404 Not Found | SV không tồn tại | Error |

---

## Module 3: Quản lý Bằng cấp

### 3.1 Cấp bằng cho sinh viên

**`POST /co-so-dao-tao/{csdt_id}/bang-cap`**

- **Auth:** Issuer (chủ sở hữu CSDT)
- **Content-Type:** `multipart/form-data`
- **Điều kiện:** SV chưa có bằng cùng `LoaiBangCap` + cùng `LinhVucId` đang ở trạng thái `DaXacNhan`
- **Hệ thống tự động:** Tạo Salt; tính `credentialHash = keccak256(data + salt)`; ghi SQL → Outbox event → gọi `issueCredential(credentialHash, holderAddress)` trên smart contract. Khi blockchain confirm: `TrangThaiBangCap = DaXacNhan`, `TrangThaiBlockchain = XacNhan`, `MaBamGiaoDich = txHash`.

**Path Parameters:**

| Tham số | Kiểu | Mô tả |
|---------|------|-------|
| csdt_id | guid | ID của CoSoDaoTao |

**Request Body (form-data):**

| Trường | Kiểu | Bắt buộc | Mô tả |
|--------|------|:--------:|-------|
| sinhVienId | guid | ✅ | ID sinh viên được cấp |
| ten | string | ✅ | Tên bằng cấp |
| loaiBangCap | int (enum `LoaiBangCap`) | ✅ | Xem bảng enum |
| linhVucId | guid | ✅ | ID lĩnh vực |
| diem | double | ✅ | Điểm số (≥ 0) |
| ngayCap | datetime | ✅ | Ngày cấp bằng |
| ngayHetHan | datetime | ❌ | Ngày hết hạn (để trống nếu không có) |
| file | file (PDF) | ❌ | File PDF bằng cấp gốc |
| link | string | ❌ | URL tham chiếu ngoài |

**Responses:**

| Status | Trường hợp | Dữ liệu trả về |
|--------|-----------|----------------|
| 201 Created | Cấp bằng thành công (đang xử lý blockchain) | `{ bangCapId: guid, trangThaiBangCap: "Nhap", trangThaiBlockchain: "ChoDuyet", maBamXacThuc: string }` |
| 400 Bad Request | Điểm < 0, thiếu trường, ngày không hợp lệ | Error |
| 403 Forbidden | Không phải Issuer của CSDT | Error |
| 409 Conflict | SV đã có bằng cùng loại + lĩnh vực đang hiệu lực | Error |

---

### 3.2 Cập nhật bằng cấp

**`PUT /co-so-dao-tao/{csdt_id}/bang-cap/{bc_id}`**

- **Auth:** Issuer (đã cấp bằng này)
- **Content-Type:** `multipart/form-data`
- **Hệ thống tự động:** Thu hồi bản cũ (`LyDoThuHoi = ThayDoiQuyDinh`) → cấp lại bản mới với Salt mới và `credentialHash` mới.

**Path Parameters:**

| Tham số | Kiểu | Mô tả |
|---------|------|-------|
| csdt_id | guid | ID của CoSoDaoTao |
| bc_id | guid | ID bằng cấp cần cập nhật |

**Request Body (form-data):**

| Trường | Kiểu | Bắt buộc | Mô tả |
|--------|------|:--------:|-------|
| ten | string | ✅ | Tên bằng cấp mới |
| loaiBangCap | int (enum `LoaiBangCap`) | ✅ | Loại bằng mới |
| linhVucId | guid | ✅ | Lĩnh vực mới |
| diem | double | ✅ | Điểm mới |
| ngayCap | datetime | ✅ | Ngày cấp mới |
| ngayHetHan | datetime | ❌ | Ngày hết hạn mới |
| file | file (PDF) | ❌ | File PDF mới |
| link | string | ❌ | URL tham chiếu mới |

**Responses:**

| Status | Trường hợp | Dữ liệu trả về |
|--------|-----------|----------------|
| 202 Accepted | Yêu cầu hợp lệ, đang xử lý blockchain | `{ bangCapMoiId: guid, bangCapCuId: guid, trangThaiBlockchain: "ChoDuyet" }` |
| 400 Bad Request | Dữ liệu không hợp lệ | Error |
| 403 Forbidden | Không phải Issuer đã cấp bằng này | Error |
| 404 Not Found | Bằng không tồn tại | Error |

---

### 3.3 Hủy bằng (vĩnh viễn)

**`POST /co-so-dao-tao/{csdt_id}/bang-cap/{bc_id}/huy`**

- **Auth:** Issuer (đã cấp bằng này)
- **Hệ thống tự động:** `TrangThaiBangCap = DaHuy`. Nếu đã lên blockchain → gọi `revokeCredential(credentialHash)`.

**Request Body:**

| Trường | Kiểu | Bắt buộc | Mô tả |
|--------|------|:--------:|-------|
| lyDoHuy | int (enum `LyDoHuy`) | ✅ | Xem bảng enum |
| ghiChuHuy | string | ❌ (✅ nếu `Khac`) | Bắt buộc khi `lyDoHuy = Khac` |

**Responses:**

| Status | Trường hợp | Dữ liệu trả về |
|--------|-----------|----------------|
| 200 OK | Hủy thành công | `{ bangCapId: guid, trangThai: "DaHuy" }` |
| 400 Bad Request | Thiếu `ghiChuHuy` khi `lyDoHuy = Khac` | Error |
| 403 Forbidden | Không phải Issuer đã cấp | Error |
| 404 Not Found | Bằng không tồn tại | Error |

---

### 3.4 Thu hồi bằng (có thể khôi phục)

**`POST /co-so-dao-tao/{csdt_id}/bang-cap/{bc_id}/thu-hoi`**

- **Auth:** Issuer (đã cấp bằng này)
- **Điều kiện:** Bằng đang ở trạng thái `DaXacNhan` và đã confirm trên blockchain
- **Hệ thống tự động:** Gọi `revokeCredential(credentialHash)`. Ảnh hưởng điểm uy tín: `BangGia / GianLanXacNhan` → −200 điểm; `ThayDoiQuyDinh` → −5 điểm.

**Request Body:**

| Trường | Kiểu | Bắt buộc | Mô tả |
|--------|------|:--------:|-------|
| lyDoThuHoi | int (enum `LyDoThuHoi`) | ✅ | Xem bảng enum |
| ghiChuThuHoi | string | ❌ (✅ nếu `Khac`) | Ghi chú thu hồi; bắt buộc khi `lyDoThuHoi = Khac` |

**Responses:**

| Status | Trường hợp | Dữ liệu trả về |
|--------|-----------|----------------|
| 200 OK | Thu hồi thành công | `{ bangCapId: guid, trangThai: "DaThuHoi" }` |
| 400 Bad Request | Bằng không ở trạng thái `DaXacNhan`, thiếu `ghiChuThuHoi` | Error |
| 403 Forbidden | Không phải Issuer đã cấp | Error |
| 404 Not Found | Bằng không tồn tại | Error |

---

### 3.5 Khôi phục bằng đã thu hồi

**`POST /co-so-dao-tao/{csdt_id}/bang-cap/{bc_id}/khoi-phuc`**

- **Auth:** Issuer (đã cấp bằng này)
- **Điều kiện:** Bằng đang ở trạng thái `DaThuHoi`
- **Hệ thống tự động:** Tạo `BangCap` mới với dữ liệu cũ + Salt mới → `credentialHash` mới → gọi `issueCredential(newHash, holderAddress)`. Bản cũ vẫn revoked trên chain (lịch sử kiểm toán).

**Request Body:**

| Trường | Kiểu | Bắt buộc | Mô tả |
|--------|------|:--------:|-------|
| lyDoKhoiPhuc | int (enum `LyDoKhoiPhuc`) | ✅ | Xem bảng enum |
| ghiChuKhoiPhuc | string | ✅ | Ghi chú khôi phục |

**Responses:**

| Status | Trường hợp | Dữ liệu trả về |
|--------|-----------|----------------|
| 202 Accepted | Khôi phục hợp lệ, đang xử lý blockchain | `{ bangCapMoiId: guid, trangThaiBangCap: "ChuaXacNhan", trangThaiBlockchain: "ChoDuyet" }` |
| 400 Bad Request | Bằng không ở trạng thái `DaThuHoi` | Error |
| 403 Forbidden | Không phải Issuer đã cấp | Error |
| 404 Not Found | Bằng không tồn tại | Error |

---

## Module 4: Xác minh Bằng cấp

### 4.1 Xác minh bằng cấp

**`GET /xac-minh`**

- **Auth:** Không bắt buộc. Nếu đã đăng nhập là Verifier → hệ thống ghi `NhatKyXacMinh` và cộng điểm uy tín cho Issuer.
- **Ghi chú:** Cung cấp `hash` hoặc `bangCapId`, không bắt buộc cả hai.

**Query Parameters:**

| Tham số | Kiểu | Bắt buộc | Mô tả |
|---------|------|:--------:|-------|
| hash | string | ❌ | credentialHash (keccak256) |
| bangCapId | guid | ❌ | ID bằng cấp (qua QR / link chia sẻ) |

> Phải có ít nhất một trong hai: `hash` hoặc `bangCapId`.

**Responses:**

| Status | Trường hợp | Dữ liệu trả về |
|--------|-----------|----------------|
| 200 OK | Xác minh hoàn tất | Xem cấu trúc JSON bên dưới |
| 400 Bad Request | Không có `hash` lẫn `bangCapId` | Error |

**Response Body (200 OK):**

```json
{
  "ketQuaXacMinh": "HopLe | KhongHopLe | DaThuHoi | KhongTonTai | GianLan",
  "tenSinhVien": "string",
  "tenIssuer": "string",
  "loaiBangCap": "string",
  "ngayCap": "datetime",
  "txHash": "string",
  "blockchainExplorerUrl": "string",

  // Chỉ khi Verifier đã đăng nhập:
  "diem": "double",
  "filePdf": "string (URL)",
  "lichSuXacMinh": [
    {
      "thoiGianXacMinh": "datetime",
      "ketQua": "string"
    }
  ]
}
```

**Phân quyền kết quả:**
- Công khai: `ketQuaXacMinh`, `tenSinhVien`, `tenIssuer`, `loaiBangCap`, `ngayCap`, `txHash`, `blockchainExplorerUrl`
- Verifier đã đăng nhập: thêm `diem`, `filePdf`, `lichSuXacMinh`

---

## Module 5: Báo cáo Gian lận

### 5.1 Tạo báo cáo gian lận

**`POST /bao-cao-gian-lan`**

- **Auth:** Holder (SinhVien) hoặc Verifier (NhaTuyenDung)

**Request Body:**

| Trường | Kiểu | Bắt buộc | Mô tả |
|--------|------|:--------:|-------|
| bangCapId | guid | ✅ | ID bằng cấp cần báo cáo |
| lyDo | int (enum `LyDoBaoCaoGianLan`) | ✅ | Xem bảng enum |
| ghiChu | string | ❌ (✅ nếu `Khac`) | Ghi chú chi tiết; bắt buộc khi `lyDo = Khac` |

**Responses:**

| Status | Trường hợp | Dữ liệu trả về |
|--------|-----------|----------------|
| 201 Created | Báo cáo được tạo | `{ baoCaoId: guid, trangThai: "ChoXuLy" }` |
| 400 Bad Request | Thiếu trường, `bangCapId` không tồn tại, hoặc `lyDo = Khac` mà thiếu `ghiChu` | Error |
| 403 Forbidden | Role không hợp lệ | Error |

---

### 5.2 Admin tiếp nhận báo cáo

**`POST /admin/bao-cao-gian-lan/{id}/tiep-nhan`**

- **Auth:** Admin
- **Điều kiện:** `TrangThaiBaoCao == ChoXuLy`

**Path Parameters:**

| Tham số | Kiểu | Mô tả |
|---------|------|-------|
| id | guid | ID của BaoCaoGianLan |

**Request Body:** Không có

**Responses:**

| Status | Trường hợp | Dữ liệu trả về |
|--------|-----------|----------------|
| 200 OK | Tiếp nhận thành công | `{ baoCaoId: guid, trangThai: "DangXuLy" }` |
| 400 Bad Request | Trạng thái không phải `ChoXuLy` | Error |
| 404 Not Found | BaoCaoGianLan không tồn tại | Error |

---

### 5.3 Admin xác nhận gian lận

**`POST /admin/bao-cao-gian-lan/{id}/xac-nhan-gian-lan`**

- **Auth:** Admin
- **Điều kiện:** `TrangThaiBaoCao == DangXuLy`
- **Hệ thống tự động:** `UyTinToChuc` của Issuer −200 điểm; `SoLuongBangCapBiBaoCaoGianLan` tăng; raise `GianLanXacNhanDomainEvent`.

**Path Parameters:**

| Tham số | Kiểu | Mô tả |
|---------|------|-------|
| id | guid | ID của BaoCaoGianLan |

**Request Body:** Không có

**Responses:**

| Status | Trường hợp | Dữ liệu trả về |
|--------|-----------|----------------|
| 200 OK | Xác nhận gian lận thành công | `{ baoCaoId: guid, trangThai: "DaXuLy" }` |
| 400 Bad Request | Trạng thái không phải `DangXuLy` | Error |
| 404 Not Found | BaoCaoGianLan không tồn tại | Error |

---

### 5.4 Admin từ chối báo cáo

**`POST /admin/bao-cao-gian-lan/{id}/tu-choi`**

- **Auth:** Admin
- **Điều kiện:** `TrangThaiBaoCao == DangXuLy`

**Path Parameters:**

| Tham số | Kiểu | Mô tả |
|---------|------|-------|
| id | guid | ID của BaoCaoGianLan |

**Request Body:** Không có

**Responses:**

| Status | Trường hợp | Dữ liệu trả về |
|--------|-----------|----------------|
| 200 OK | Từ chối báo cáo thành công | `{ baoCaoId: guid, trangThai: "TuChoi" }` |
| 400 Bad Request | Trạng thái không phải `DangXuLy` | Error |
| 404 Not Found | BaoCaoGianLan không tồn tại | Error |

---

## Module 6: Tuyển dụng AI

### 6.1 Cập nhật thông tin Nhà tuyển dụng

**`PUT /nha-tuyen-dung/{id}`**

- **Auth:** Verifier (chủ sở hữu)

**Path Parameters:**

| Tham số | Kiểu | Mô tả |
|---------|------|-------|
| id | guid | ID của NhaTuyenDung |

**Request Body:**

| Trường | Kiểu | Bắt buộc | Mô tả |
|--------|------|:--------:|-------|
| ten | string | ✅ | Tên công ty |
| diaChi | string | ❌ | Địa chỉ |
| sdt | string | ❌ | Số điện thoại |
| email | string | ❌ | Email liên hệ |
| website | string | ❌ | Website |
| logo | string | ❌ | URL logo |
| moTa | string | ❌ | Mô tả công ty |
| maSoThue | string | ❌ | Mã số thuế |

**Responses:**

| Status | Trường hợp | Dữ liệu trả về |
|--------|-----------|----------------|
| 200 OK | Cập nhật thành công | `{ nhaTuyenDungId: guid }` |
| 400 Bad Request | `ten` để trống | Error |
| 403 Forbidden | Không phải chủ sở hữu | Error |

---

### 6.2 Thêm giấy phép cho Nhà tuyển dụng

**`POST /nha-tuyen-dung/{id}/giay-phep`**

- **Auth:** Verifier (chủ sở hữu)
- **Content-Type:** `multipart/form-data`
- **Ghi chú:** Dành cho NTD đã được Admin duyệt muốn bổ sung giấy tờ.

**Request Body (form-data):**

| Trường | Kiểu | Bắt buộc | Mô tả |
|--------|------|:--------:|-------|
| file | file (PDF) | ✅ | File giấy phép |
| kieuGiayPhep | int (enum `KieuGiayPhepNTD`) | ✅ | Xem bảng enum |

**Responses:**

| Status | Trường hợp | Dữ liệu trả về |
|--------|-----------|----------------|
| 201 Created | Thêm thành công | `{ giayPhepId: guid }` |
| 400 Bad Request | File không hợp lệ | Error |
| 403 Forbidden | Không phải chủ sở hữu | Error |

---

### 6.3 Tạo tin tuyển dụng

**`POST /nha-tuyen-dung/{ntd_id}/tin-tuyen-dung`**

- **Auth:** Verifier

**Path Parameters:**

| Tham số | Kiểu | Mô tả |
|---------|------|-------|
| ntd_id | guid | ID của NhaTuyenDung |

**Request Body:**

| Trường | Kiểu | Bắt buộc | Mô tả |
|--------|------|:--------:|-------|
| ten | string | ✅ | Tên vị trí tuyển dụng |
| moTa | string | ✅ | Mô tả công việc |
| linhVucId | guid | ✅ | ID lĩnh vực |
| thoiHanUngTuyen | datetime | ✅ | Hạn cuối ứng tuyển (phải lớn hơn thời điểm hiện tại) |

**Responses:**

| Status | Trường hợp | Dữ liệu trả về |
|--------|-----------|----------------|
| 201 Created | Tạo tin thành công | `{ thongTinTuyenDungId: guid }` |
| 400 Bad Request | `ten` / `moTa` trống, `thoiHanUngTuyen` ≤ hiện tại | Error |
| 403 Forbidden | Không phải chủ sở hữu NTD | Error |

---

### 6.4 Cập nhật tin tuyển dụng

**`PUT /nha-tuyen-dung/{ntd_id}/tin-tuyen-dung/{tttd_id}`**

- **Auth:** Verifier

**Path Parameters:**

| Tham số | Kiểu | Mô tả |
|---------|------|-------|
| ntd_id | guid | ID của NhaTuyenDung |
| tttd_id | guid | ID của ThongTinTuyenDung |

**Request Body:**

| Trường | Kiểu | Bắt buộc | Mô tả |
|--------|------|:--------:|-------|
| ten | string | ✅ | Tên vị trí |
| moTa | string | ✅ | Mô tả |
| linhVucId | guid | ✅ | Lĩnh vực |
| thoiHanUngTuyen | datetime | ✅ | Hạn ứng tuyển mới |

**Responses:**

| Status | Trường hợp | Dữ liệu trả về |
|--------|-----------|----------------|
| 200 OK | Cập nhật thành công | `{ thongTinTuyenDungId: guid }` |
| 400 Bad Request | Dữ liệu không hợp lệ | Error |
| 403 Forbidden | Không phải chủ sở hữu | Error |
| 404 Not Found | Tin không tồn tại | Error |

---

### 6.5 Xóa tin tuyển dụng

**`DELETE /nha-tuyen-dung/{ntd_id}/tin-tuyen-dung/{tttd_id}`**

- **Auth:** Verifier
- **Ghi chú:** Soft delete — đặt `ThoiGianXoa`, không xóa khỏi DB.

**Responses:**

| Status | Trường hợp | Dữ liệu trả về |
|--------|-----------|----------------|
| 200 OK | Xóa thành công | `{ thongTinTuyenDungId: guid }` |
| 403 Forbidden | Không phải chủ sở hữu | Error |
| 404 Not Found | Tin không tồn tại | Error |

---

### 6.6 Xem danh sách tin tuyển dụng

**`GET /tin-tuyen-dung`**

- **Auth:** Không bắt buộc (công khai)

**Query Parameters:**

| Tham số | Kiểu | Bắt buộc | Mô tả |
|---------|------|:--------:|-------|
| linhVucId | guid | ❌ | Lọc theo lĩnh vực |
| tuKhoa | string | ❌ | Từ khóa tìm kiếm |
| trang | int | ❌ | Số trang (mặc định 1) |
| soLuong | int | ❌ | Số bản ghi mỗi trang (mặc định 20) |

**Responses:**

| Status | Trường hợp | Dữ liệu trả về |
|--------|-----------|----------------|
| 200 OK | Lấy danh sách thành công | `{ items: [{ id, ten, moTa, linhVuc, thoiHanUngTuyen, nhaTuyenDung: { ten, logo } }], totalCount, trang, soLuong }` |

---

### 6.7 Nộp hồ sơ ứng tuyển

**`POST /ho-so-ung-tuyen`**

- **Auth:** Holder
- **Điều kiện:** Chưa nộp hồ sơ cho tin này; tất cả bằng cấp đính kèm phải ở trạng thái `DaXacNhan`
- **Hệ thống tự động:** Xác minh từng bằng trên blockchain. Tạo `BangCapUngTuyen` cho từng bằng đính kèm.

**Request Body:**

| Trường | Kiểu | Bắt buộc | Mô tả |
|--------|------|:--------:|-------|
| thongTinTuyenDungId | guid | ✅ | ID tin tuyển dụng |
| bangCapIds | guid[] | ✅ | Danh sách ID bằng cấp đính kèm |

**Responses:**

| Status | Trường hợp | Dữ liệu trả về |
|--------|-----------|----------------|
| 201 Created | Nộp hồ sơ thành công | `{ hoSoUngTuyenId: guid, trangThai: "ChoXem" }` |
| 400 Bad Request | Bằng không hợp lệ, `bangCapIds` rỗng, `thongTinTuyenDungId` không hợp lệ | Error |
| 403 Forbidden | Không phải Holder | Error |
| 409 Conflict | Đã nộp hồ sơ cho tin này | Error |

---

### 6.8 Thêm bằng cấp vào hồ sơ

**`POST /ho-so-ung-tuyen/{id}/bang-cap`**

- **Auth:** Holder (chủ sở hữu hồ sơ)
- **Điều kiện:** `TrangThaiUngTuyen == ChoXem`

**Request Body:**

| Trường | Kiểu | Bắt buộc | Mô tả |
|--------|------|:--------:|-------|
| bangCapId | guid | ✅ | ID bằng cấp muốn thêm |

**Responses:**

| Status | Trường hợp | Dữ liệu trả về |
|--------|-----------|----------------|
| 200 OK | Thêm thành công | `{ hoSoUngTuyenId: guid }` |
| 400 Bad Request | Hồ sơ đã được NTD xem (`DaXem`), bằng không hợp lệ | Error |
| 403 Forbidden | Không phải chủ sở hữu hồ sơ | Error |

---

### 6.9 Xóa bằng cấp khỏi hồ sơ

**`DELETE /ho-so-ung-tuyen/{id}/bang-cap/{bc_id}`**

- **Auth:** Holder (chủ sở hữu hồ sơ)
- **Điều kiện:** `TrangThaiUngTuyen == ChoXem`

**Responses:**

| Status | Trường hợp | Dữ liệu trả về |
|--------|-----------|----------------|
| 200 OK | Xóa thành công | `{ hoSoUngTuyenId: guid }` |
| 400 Bad Request | Hồ sơ đã được NTD xem (`DaXem`) | Error |
| 403 Forbidden | Không phải chủ sở hữu | Error |

---

### 6.10 Thu hồi hồ sơ ứng tuyển

**`POST /ho-so-ung-tuyen/{id}/thu-hoi`**

- **Auth:** Holder (chủ sở hữu hồ sơ)
- **Điều kiện:** `TrangThaiUngTuyen == ChoXem`

**Request Body:** Không có

**Responses:**

| Status | Trường hợp | Dữ liệu trả về |
|--------|-----------|----------------|
| 200 OK | Thu hồi thành công | `{ hoSoUngTuyenId: guid, trangThai: "DaThuHoi" }` |
| 400 Bad Request | Hồ sơ không ở trạng thái `ChoXem` | Error |
| 403 Forbidden | Không phải chủ sở hữu | Error |

---

### 6.11 Nhà tuyển dụng cập nhật trạng thái hồ sơ

**`PUT /nha-tuyen-dung/{ntd_id}/ho-so-ung-tuyen/{id}/trang-thai`**

- **Auth:** Verifier (chủ sở hữu NTD tương ứng)

**Request Body:**

| Trường | Kiểu | Bắt buộc | Mô tả |
|--------|------|:--------:|-------|
| trangThai | int (enum `TrangThaiUngTuyen`) | ✅ | Chỉ được đặt: `DaXem=1`, `ChapNhan=2`, `TuChoi=3` |

**Responses:**

| Status | Trường hợp | Dữ liệu trả về |
|--------|-----------|----------------|
| 200 OK | Cập nhật thành công | `{ hoSoUngTuyenId: guid, trangThai: string }` |
| 400 Bad Request | `TrangThai` không hợp lệ | Error |
| 403 Forbidden | NTD không sở hữu tin tuyển dụng này | Error |
| 404 Not Found | Hồ sơ không tồn tại | Error |

---

### 6.12 AI phân tích độ phù hợp hồ sơ

**`POST /nha-tuyen-dung/{ntd_id}/tin-tuyen-dung/{tttd_id}/ho-so/{hoSoId}/phan-tich`**

- **Auth:** Verifier
- **Ghi chú:** Có thể chạy nhiều lần. Mỗi lần tạo `KetQuaPhanTich` mới (lưu lịch sử). Kết quả AI chỉ mang tính tham khảo.
- **AI phân tích dựa trên:** lĩnh vực, loại bằng, điểm số, `UyTinToChuc` của Issuer.

**Request Body:** Không có (AI tự lấy dữ liệu từ `HoSoUngTuyen` và `ThongTinTuyenDung`)

**Responses:**

| Status | Trường hợp | Dữ liệu trả về |
|--------|-----------|----------------|
| 201 Created | Phân tích hoàn tất | `{ ketQuaPhanTichId: guid, phanTramPhuHop: double (0–100), ketLuan: { diemManh: string[], diemThieu: string[], deXuat: string }, thoiGianPhanTich: datetime }` |
| 400 Bad Request | Hồ sơ hoặc tin tuyển dụng không hợp lệ | Error |
| 403 Forbidden | Không phải chủ sở hữu NTD | Error |
| 404 Not Found | HoSo hoặc ThongTinTuyenDung không tồn tại | Error |

---

## Phụ lục: Enum Reference

### LoaiBangCap

| Giá trị | Tên |
|:-------:|-----|
| 0 | CaoDang |
| 1 | CuNhan |
| 2 | KySu |
| 3 | ThacSi |
| 4 | TienSi |
| 5 | ChungChiNgoaiNgu |
| 6 | ChungChiTinHoc |
| 7 | ChungChiNgheNghiep |
| 8 | BangDiem |
| 9 | GiayChungNhanTotNghiepTam |
| 10 | ChungNhanHoanThanhKhoaHoc |
| 11 | ChungNhanThucTap |
| 12 | GiayKhenThuong |

---

### TrangThaiBangCap

| Giá trị | Tên |
|:-------:|-----|
| 0 | ChuaXacNhan |
| 1 | DaXacNhan |
| 2 | DaThuHoi |
| 3 | DaHuy |

---

### TrangThaiBlockchain

| Giá trị | Tên |
|:-------:|-----|
| 0 | ChoDuyet |
| 1 | XacNhan |
| 2 | ThatBai |

---

### LyDoHuy

| Giá trị | Tên | Ảnh hưởng UyTin |
|:-------:|-----|:---------------:|
| 0 | LoiNhapLieu | −5 |
| 1 | NhapTrungLap | −5 |
| 2 | YeuCauCuaSinhVien | 0 |
| 3 | YeuCauCuaCoSoDaoTao | 0 |
| 99 | Khac | 0 |

---

### LyDoThuHoi

| Giá trị | Tên | Ảnh hưởng UyTin |
|:-------:|-----|:---------------:|
| 0 | ViPhamHocThuat | 0 |
| 1 | ViPhamDaoDuc | 0 |
| 2 | BangGia | −200 |
| 3 | GianLanXacNhan | −200 |
| 4 | ThayDoiQuyDinh | −5 |
| 5 | QuyetDinhPhapLy | 0 |
| 99 | Khac | 0 |

---

### LyDoKhoiPhuc

| Giá trị | Tên |
|:-------:|-----|
| 0 | SuaLoiNhapLieu |
| 1 | SinhVienKhongConViPham |
| 2 | DaoHanPhucHoi |
| 3 | XacNhanKhongGianLan |
| 4 | QuyDinhDuocCapNhat |
| 5 | QuyetDinhPhapLyBiHuy |
| 99 | Khac |

---

### KieuGiayPhepCSDT

| Giá trị | Tên | Bắt buộc |
|:-------:|-----|:--------:|
| 0 | GiayPhepHoatDongGiaoDuc | ✅ |
| 1 | QuyetDinhThanhLapTruong | ✅ |
| 2 | GiayCongNhanNganhDaoTao | ❌ |
| 3 | XacNhanDangKyVoiBoGDDT | ❌ |

---

### KieuGiayPhepNTD

| Giá trị | Tên | Bắt buộc |
|:-------:|-----|:--------:|
| 0 | GiayPhepDangKyKinhDoanh | ✅ |
| 1 | MaSoThue | ❌ |
| 2 | CongVanXacNhanBoPhanHR | ❌ |

---

### LyDoTuChoi (Admin từ chối đăng ký)

| Giá trị | Tên |
|:-------:|-----|
| 0 | GiayToKhongHopLe |
| 1 | ThongTinKhongChinhXac |
| 2 | KhongDuGiayTo |
| 3 | ToChucDaTonTai |
| 4 | ChuKySoKhongHopLe |
| 99 | Khac |

---

### TrangThaiYeuCauDangKy

| Giá trị | Tên |
|:-------:|-----|
| 0 | Nhap |
| 1 | DaGui |
| 2 | XacNhan |
| 3 | TuChoi |

---

### TrangThaiXacMinhGiayPhep

| Giá trị | Tên |
|:-------:|-----|
| 0 | ChoXacMinh |
| 1 | DaXacMinh |
| 2 | TuChoi |
| 3 | TaiLenLai |

---

### LyDoBaoCaoGianLan

| Giá trị | Tên |
|:-------:|-----|
| 0 | GiaMaoBangCap |
| 1 | ThongTinSai |
| 2 | SuDungTraiPhep |
| 3 | GianLanXacNhan |
| 99 | Khac |

---

### TrangThaiBaoCao

| Giá trị | Tên |
|:-------:|-----|
| 0 | ChoXuLy |
| 1 | DangXuLy |
| 2 | DaXuLy |
| 3 | TuChoi |

---

### TrangThaiUngTuyen

| Giá trị | Tên |
|:-------:|-----|
| 0 | ChoXem |
| 1 | DaXem |
| 2 | ChapNhan |
| 3 | TuChoi |
| 4 | DaThuHoi |

---

### KetQuaXacMinh

| Giá trị | Tên |
|:-------:|-----|
| 0 | HopLe |
| 1 | KhongHopLe |
| 2 | DaThuHoi |
| 3 | KhongTonTai |
| 4 | GianLan |

---

### HangUyTin

| Giá trị | Tên | Điều kiện |
|:-------:|-----|-----------|
| 0 | ChuaCoGiayPhep | Chưa upload giấy phép (`SoLuongGiayPhep == 0`) |
| 1 | Dong | Điểm < 100 |
| 2 | Bac | Điểm 100–299 |
| 3 | Vang | Điểm 300–499 |
| 4 | DaCoGiayPhep | Điểm ≥ 500 |

> **Điểm uy tín ban đầu khi được duyệt:** +50 × số giấy phép đã nộp.
> Mỗi bằng cấp ghi blockchain thành công: +2. Xác minh hợp lệ bởi NTD: +1.

---
### Domain code gaps (cần cập nhật domain)

| Gap | Mô tả |
|-----|-------|
| `NhaTuyenDung` thiếu business fields | ERD/domain không có `sdt`, `email`, `website`, `logo`, `moTa`, `maSoThue` — cần thêm nếu muốn hỗ trợ profile đầy đủ |
| **Bug** `HoSoUngTuyen.Create()` là `internal` không có aggregate root wrap | Application layer không gọi được. Cần thêm method factory vào aggregate (tương tự `TaoBangCapChoSinhVien`, `TaoSinhVien`). F-30 không implement được nếu không fix |
| `CoSoDaoTao` thiếu method để trừ điểm khi xác nhận gian lận | `UyTinToChuc.TruDiemBangCapGianLan()` tồn tại trong ValueObject nhưng không có public method trên `CoSoDaoTao` gọi nó. `UyTin` có `private set` nên Application layer không cập nhật trực tiếp được. Domain event handler F-22 không có entry point |
| `CoSoDaoTao` thiếu method để cộng điểm khi Verifier xác minh hợp lệ | `UyTinToChuc.CongDiemXacMinhHopLe()` tồn tại nhưng không có wrapper trên `CoSoDaoTao`. F-37 (+1 xác minh hợp lệ) không có entry point trong aggregate |
| `CoSoDaoTao` thiếu `YeuCauDangKyId` trong domain model | `YeuCauDangKyId` tồn tại ở Infrastructure (shadow property trong `CoSoDaoTaoConfiguration`) nhưng không có trong domain aggregate — Application layer phải set qua EF shadow property khi handle `CoSoDaoTaoApprovedEvent` |
