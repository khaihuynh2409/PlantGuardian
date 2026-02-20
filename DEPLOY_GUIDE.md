# Hướng dẫn Deploy PlantGuardian

Tất cả code đã được chuẩn bị sẵn sàng để lên Cloud và build thành App chạy thật. Hãy làm theo các bước sau:

## 1. Deploy Backend (API & AI) lên Railway.app
Dự án của bạn là một "monorepo" – có 2 thành phần hệ thống (API và AI) nằm trong 2 thư mục riêng biệt. Không sao cả, Railway hỗ trợ rất tốt nhưng bạn cần thiết lập "Root Directory".

**LƯU Ý TRƯỚC TIÊN:** Mình vừa sửa lại file `PlantGuardian.API/Dockerfile` để tương thích hoàn toàn với Railway. Bạn hãy **Commit và Push** đoạn code mới lên GitHub trước khi thao tác tiếp trên Railway nhé!

1. Đăng ký tài khoản tại [Railway.app](https://railway.app/) (đăng nhập bằng GitHub).
2. Tạo 1 repository trên GitHub và push toàn bộ source code thư mục `PlantGuardian`. Mình thấy bạn vừa làm xong.
3. Trên Railway Dashboard:
   - Bấm **New Project** -> **Deploy from GitHub repo**.
   - Chọn repo `PlantGuardian`.
   - **QUAN TRỌNG:** Ngay sau khi chọn, Railway sẽ tự động dò tìm code ở thư mục gốc (root) và **sẽ BÁO LỖI (Build failed in 10 seconds)** như trong ảnh bạn vừa chụp. **ĐÂY LÀ ĐIỀU BÌNH THƯỜNG**, đừng lo lắng!

4. Thiết lập cho thư mục API:
   - Bấm vào cái ô (Service) báo lỗi màu đỏ trên màn hình Railway.
   - Chuyển sang tab mũi tên cờ lê **Settings** -> Cuộn xuống mục **Deploy** -> Tìm ô **Root Directory**.
   - Gõ chính xác `/PlantGuardian.API` vào đó (bấm dấu tick để lưu).
   - Railway sẽ tự động nhận diện lại Dockerfile và bắt đầu Build. Nếu nó không tự build, bạn hãy bấm nút **Deploy** (góc trên bên phải).
   - Đợi khoảng 1-2 phút cho thẻ báo "Active" màu xanh lá.
   - Chuyển tab **Settings** -> **Networking** -> Bấm **Generate Domain** để lấy URL (VD: `https://plantguardian-api-production.up.railway.app`).

5. Tạo Service thứ 2 cho thư mục AI:
   - Quay lại trang Dashboard chính của Dự án Railway này.
   - Bấm nút **+ Create** (hoặc nút + ở góc) -> Chọn **GitHub Repo** -> Chọn lại repo `PlantGuardian`.
   - Nó cũng sẽ lại chạy lỗi trong vài giây đầu. Kệ nó.
   - Bạn bấm vào nó -> Chuyển sang **Settings** -> Gõ vào **Root Directory**: `/PlantGuardian.AI`
   - Chờ báo "Active" -> Qua tab Networking tạo Domain để lấy URL thứ 2.

## 2. Cập nhật URL vào Mobile App
Khi có được 2 URL từ Railway, hãy mở file `AppConfig.cs` (nằm trong thư mục `PlantGuardian.Mobile`):

```csharp
public static class AppConfig
{
    // NHỚ GIỮ LẠI CHỮ /api/ Ở CUỐI CHO ĐƯỜNG DẪN API !!!
    public const string ApiBaseUrl = "CÁC_ĐỊA_CHỈ_TỪ_RAILWAY_CỦA_API/api/";
    
    // ĐƯỜNG DẪN AI CỨ ĐỂ TRỐNG THÊM DẤU / LÀ ĐƯỢC
    public const string AiBaseUrl = "CÁC_ĐỊA_CHỈ_TỪ_RAILWAY_CỦA_AI/";
}
```

*Ví dụ chuẩn xác:*
- `ApiBaseUrl = "https://plantguardian-api-production.up.railway.app/api/";`
- `AiBaseUrl = "https://plantguardian-ai-production.up.railway.app/";`

## 3. Build APK
Mở Terminal / PowerShell tại thư mục thư mục `PlantGuardian.Mobile` và chạy lệnh sau:

```bash
dotnet publish -f net8.0-android -c Release
```

Sau khi chạy xong, file APK hoàn chỉnh sẽ nằm trong đường dẫn:
`PlantGuardian.Mobile\bin\Release\net10.0-android\publish\`

Bạn hãy chép file có chữ `*-Signed.apk` vào điện thoại Android và cài đặt. Thưởng thức PlantGuardian ở bất kì đâu!
