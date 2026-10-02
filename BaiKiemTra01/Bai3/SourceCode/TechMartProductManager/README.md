# TechMart Product Manager

Ứng dụng quản lý danh mục thiết bị công nghệ bằng C# Windows Forms (.NET 8).

## Mở và chạy
1. Giải nén file ZIP.
2. Mở `TechMartProductManager.csproj` bằng Visual Studio 2022.
3. Nếu Visual Studio yêu cầu cài workload, cài **.NET desktop development**.
4. Chọn cấu hình `Debug` và nhấn `F5`.

## Chức năng
- Giao diện co giãn theo cửa sổ, chia khung nhập liệu và bảng dữ liệu.
- Thêm, cập nhật, xóa sản phẩm.
- Kiểm tra tên, đơn giá và số lượng bằng ErrorProvider.
- Chọn ảnh sản phẩm.
- Tìm kiếm tên sản phẩm theo thời gian thực.
- Xuất CSV qua SaveFileDialog; File menu có Ctrl+E và Ctrl+X.
- BindingList<Product>, BindingSource, DataGridView tự định nghĩa cột.
- StatusStrip hiển thị tổng số sản phẩm.

## Lưu ý
Dữ liệu được lưu trong bộ nhớ khi ứng dụng đang chạy; đóng ứng dụng sẽ xóa danh sách đã nhập. Ảnh chỉ lưu đường dẫn cục bộ, nên ảnh có thể không hiện nếu chuyển máy hoặc di chuyển file ảnh.
