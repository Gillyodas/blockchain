# Danh sách Chức năng Hệ thống — ChainDegree

> **Phiên bản:** 1.0 | **Ngày:** 2026-04-06

---

## 1. Đăng ký & Xác thực Tổ chức

| Mã | Chức năng | Mô tả | Vai trò |
|----|-----------|-------|---------|
| F-01 | Đăng ký làm Cơ sở đào tạo | Tổ chức nộp yêu cầu trở thành Issuer với thông tin tổ chức và giấy phép bắt buộc (Giấy phép hoạt động giáo dục + Quyết định thành lập trường). Qua 2 bước: tạo yêu cầu → tải giấy phép → nộp hồ sơ. | Account |
| F-02 | Đăng ký làm Nhà tuyển dụng | Tổ chức nộp yêu cầu trở thành Verifier với thông tin công ty và giấy phép kinh doanh. Quy trình tương tự F-01. | Account |
| F-03 | Tải lên giấy phép tổ chức | Upload file PDF giấy tờ đăng ký, hệ thống tự động verify chữ ký số (PKI) và cập nhật trạng thái. | Account |
| F-04 | Tải lại giấy phép bị từ chối | Khi giấy phép bị Admin từ chối, tổ chức được phép upload lại file mới cho cùng loại giấy phép đó. | Account |
| F-05 | Xác nhận nộp hồ sơ | Tổ chức chính thức gửi hồ sơ lên Admin xét duyệt sau khi đã upload đủ giấy tờ bắt buộc. | Account |
| F-06 | Admin phê duyệt đăng ký | Admin xem xét và phê duyệt hồ sơ — hệ thống tự động tạo CSDT hoặc NTD, gán Role, khởi tạo điểm uy tín. | Admin |
| F-07 | Admin từ chối đăng ký | Admin từ chối hồ sơ với lý do cụ thể — tổ chức có thể tải lại giấy phép và nộp lại. | Admin |

---

## 2. Quản lý Sinh viên (Holder)

| Mã | Chức năng | Mô tả | Vai trò |
|----|-----------|-------|---------|
| F-08 | Tạo sinh viên mới | CSDT tạo tài khoản sinh viên bằng CCCD + email. Nếu CCCD đã có trong hệ thống (SV thuộc CSDT khác), hệ thống liên kết chứ không tạo mới. | Issuer |
| F-09 | Nhập sinh viên hàng loạt từ Excel | Upload file Excel theo template — hệ thống validate và trả về preview (số tạo mới, số liên kết, các lỗi). Cần xác nhận ở bước 2 mới thực sự lưu. | Issuer |
| F-10 | Xác nhận import hàng loạt | Bước 2 của F-09 — xác nhận đồng ý sau khi xem preview. | Issuer |
| F-11 | Tải template Excel nhập sinh viên | Tải file template Excel chuẩn để nhập đúng định dạng. | Issuer |
| F-12 | Cập nhật thông tin sinh viên | Cập nhật Họ tên và Email của sinh viên. CCCD không thể thay đổi. Số điện thoại quản lý ở tầng Identity. | Issuer |

---

## 3. Quản lý Bằng cấp

| Mã | Chức năng | Mô tả | Vai trò |
|----|-----------|-------|---------|
| F-13 | Cấp bằng cho sinh viên | CSDT cấp bằng/chứng chỉ mới cho SV. Hệ thống kiểm tra không trùng loại + lĩnh vực đang hiệu lực. Sau khi lưu SQL, tự động ghi hash lên blockchain qua Outbox pattern. | Issuer |
| F-14 | Cập nhật bằng cấp | Sửa thông tin bằng — tự động thu hồi bản cũ (lý do: ThayDoiQuyDinh) và tạo bằng mới với hash mới trên blockchain. | Issuer |
| F-15 | Hủy bằng vĩnh viễn | Hủy bằng không thể khôi phục (DaHuy). Áp dụng khi lỗi nhập liệu, nhập trùng, hoặc theo yêu cầu. Ảnh hưởng điểm uy tín nếu lý do là lỗi nhập liệu. | Issuer |
| F-16 | Thu hồi bằng tạm thời | Thu hồi bằng — có thể khôi phục sau (DaThuHoi). Gọi `revokeCredential` trên blockchain. Ảnh hưởng nặng điểm uy tín nếu lý do gian lận/bằng giả. | Issuer |
| F-17 | Khôi phục bằng đã thu hồi | Tạo bằng mới với dữ liệu cũ + salt mới → hash mới. Bản cũ vẫn revoked trên chain (lịch sử kiểm toán). | Issuer |

---

## 4. Xác minh Bằng cấp

| Mã | Chức năng | Mô tả | Vai trò |
|----|-----------|-------|---------|
| F-18 | Xác minh bằng cấp (công khai) | Tra cứu bằng hash hoặc bangCapId. Hệ thống query smart contract để xác nhận tính hợp lệ. Không cần đăng nhập. | Công khai |
| F-19 | Xem chi tiết bằng khi xác minh | Verifier đã đăng nhập thấy thêm: điểm số, file PDF, lịch sử xác minh. Hệ thống ghi nhật ký xác minh và cộng điểm uy tín cho Issuer (+1). | Verifier |

---

## 5. Báo cáo Gian lận

| Mã | Chức năng | Mô tả | Vai trò |
|----|-----------|-------|---------|
| F-20 | Báo cáo bằng giả / gian lận | SV hoặc NTD báo cáo bằng cấp nghi ngờ gian lận với lý do cụ thể. Báo cáo tạo ở trạng thái ChoXuLy. | Holder / Verifier |
| F-21 | Admin tiếp nhận báo cáo | Admin tiếp nhận báo cáo để bắt đầu điều tra (ChoXuLy → DangXuLy). | Admin |
| F-22 | Admin xác nhận gian lận | Admin xác nhận báo cáo là gian lận — CSDT bị trừ 200 điểm uy tín, domain event được raise để thu hồi bằng liên quan. | Admin |
| F-23 | Admin từ chối báo cáo | Admin kết luận báo cáo không có cơ sở — đóng báo cáo mà không ảnh hưởng uy tín CSDT. | Admin |

---

## 6. Tuyển dụng AI

| Mã | Chức năng | Mô tả | Vai trò |
|----|-----------|-------|---------|
| F-24 | Cập nhật thông tin Nhà tuyển dụng | NTD cập nhật hồ sơ công ty: tên, địa chỉ, website, logo, mô tả. | Verifier |
| F-25 | Thêm giấy phép cho NTD | NTD đã được duyệt bổ sung thêm giấy tờ chứng nhận hoạt động. | Verifier |
| F-26 | Tạo tin tuyển dụng | NTD đăng tin tuyển dụng với tên vị trí, mô tả, lĩnh vực, và hạn ứng tuyển. | Verifier |
| F-27 | Cập nhật tin tuyển dụng | NTD sửa nội dung tin đang đăng. | Verifier |
| F-28 | Xóa tin tuyển dụng | Soft delete — đóng tin tuyển dụng. | Verifier |
| F-29 | Xem danh sách tin tuyển dụng | Tất cả người dùng (kể cả không đăng nhập) xem tin đang mở, hỗ trợ lọc theo lĩnh vực và tìm kiếm theo từ khóa. | Công khai |
| F-30 | Nộp hồ sơ ứng tuyển | SV chọn bằng cấp đính kèm và nộp hồ sơ cho vị trí tuyển dụng. Hệ thống xác minh bằng trên blockchain trước khi chấp nhận. | Holder |
| F-31 | Thêm bằng cấp vào hồ sơ | SV bổ sung bằng cấp vào hồ sơ đã nộp, trong khi NTD chưa xem. | Holder |
| F-32 | Xóa bằng cấp khỏi hồ sơ | SV rút bằng cấp ra khỏi hồ sơ đang chờ NTD xem. | Holder |
| F-33 | Thu hồi hồ sơ ứng tuyển | SV rút toàn bộ hồ sơ (chỉ khi NTD chưa xem). | Holder |
| F-34 | NTD cập nhật trạng thái hồ sơ | NTD đánh dấu hồ sơ là DaXem / ChapNhan / TuChoi. Sau khi DaXem, SV không thể chỉnh sửa hồ sơ nữa. | Verifier |
| F-35 | AI phân tích độ phù hợp | NTD chạy phân tích AI cho một hồ sơ ứng tuyển — trả về % phù hợp, điểm mạnh, điểm thiếu, và đề xuất. Có thể chạy nhiều lần, mỗi lần tạo bản phân tích mới. | Verifier |

---

## 7. Hệ thống Uy tín Tổ chức

| Mã | Chức năng | Mô tả |
|----|-----------|-------|
| F-36 | Khởi tạo điểm uy tín | Khi CSDT / NTD được Admin duyệt, điểm uy tín khởi tạo = 50 × số giấy phép đã nộp. |
| F-37 | Cộng điểm uy tín | +2 khi cấp bằng thành công lên blockchain; +1 khi Verifier xác minh bằng hợp lệ. |
| F-38 | Trừ điểm uy tín | −5 khi hủy/thu hồi bằng vì lỗi nhập liệu; −200 khi Admin xác nhận gian lận. |
| F-39 | Xếp hạng uy tín | Tự động tính hạng dựa trên điểm: ChuaCoGiayPhep / Dong / Bac / Vang / DaCoGiayPhep. |

---

## 8. Blockchain & Xác thực Hash

| Mã | Chức năng | Mô tả |
|----|-----------|-------|
| F-40 | Ghi hash lên blockchain | Sau khi cấp bằng, Outbox event kích hoạt gọi `issueCredential(credentialHash, holderAddress)` trên Hyperledger Besu. |
| F-41 | Thu hồi hash trên blockchain | Khi thu hồi / hủy bằng (sau khi đã lên chain), gọi `revokeCredential(credentialHash)`. |
| F-42 | Tái phát hành hash | Khi khôi phục bằng hoặc cập nhật bằng, tạo Salt mới → hash mới → gọi lại `issueCredential` với hash mới. |
| F-43 | Verify hash on-chain | Khi xác minh, hệ thống tính lại hash từ dữ liệu SQL + salt, so sánh với giá trị trên smart contract. |

---

## Tóm tắt số lượng

| Nhóm | Số chức năng |
|------|:------------:|
| Đăng ký & Xác thực Tổ chức | 7 |
| Quản lý Sinh viên | 5 |
| Quản lý Bằng cấp | 5 |
| Xác minh Bằng cấp | 2 |
| Báo cáo Gian lận | 4 |
| Tuyển dụng AI | 12 |
| Hệ thống Uy tín | 4 |
| Blockchain & Hash | 4 |
| **Tổng** | **43** |
