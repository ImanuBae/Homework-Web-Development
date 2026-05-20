# BookDB - Ứng Dụng Quản Lý Sách

Ứng dụng ASP.NET Core MVC sử dụng Entity Framework Core với Code First Approach để quản lý sách và danh mục sách.

## Công Nghệ Sử Dụng

- **Framework**: ASP.NET Core 10.0
- **Database**: SQL Server
- **ORM**: Entity Framework Core 8.0
- **Frontend**: Bootstrap 5.3

## Tính Năng

### 1. Quản Lý Sách (CRUD)
- Thêm sách mới
- Xem danh sách sách
- Sửa thông tin sách
- Xóa sách

### 2. Quản Lý Danh Mục (CRUD)
- Thêm danh mục sách
- Xem danh sách danh mục
- Sửa thông tin danh mục
- Xóa danh mục

### 3. Hiển Thị Sách
- Xem tất cả sách trên trang chủ
- Xem sách theo danh mục
- Xem chi tiết sách

## Cấu Trúc Dự Án

```
BookDB/
├── Models/               # Các model (Entity)
│   ├── Book.cs
│   ├── Category.cs
│   └── ErrorViewModel.cs
├── Controllers/          # Các controller
│   ├── HomeController.cs
│   ├── BookController.cs
│   └── CategoryController.cs
├── Views/               # Các view (Razor)
│   ├── Home/
│   ├── Book/
│   ├── Category/
│   └── Shared/
├── Data/                # DbContext
│   └── BookDbContext.cs
├── Properties/          # Cấu hình ứng dụng
├── wwwroot/            # Static files
└── Program.cs          # Entry point
```

## Cài Đặt và Chạy

### 1. Điều Kiện Tiên Quyết
- .NET 10.0 SDK
- SQL Server (hoặc SQL Server Express)
- Visual Studio Code hoặc Visual Studio

### 2. Các Bước Cài Đặt

#### Cách 1: Dùng dotnet CLI

1. Mở Terminal trong thư mục `BookDB`

2. Khôi phục các package:
```bash
dotnet restore
```

3. Chạy các migration để tạo cơ sở dữ liệu:
```bash
dotnet ef database update
```

4. Chạy ứng dụng:
```bash
dotnet run
```

5. Mở trình duyệt tại: `https://localhost:7001`

#### Cách 2: Dùng Visual Studio

1. Mở file `BookDB.csproj` trong Visual Studio
2. Bây giờ hãy chuột phải trên project > "Manage User Secrets" để cấu hình connection string (nếu cần)
3. Mở Package Manager Console (Tools > NuGet Package Manager > Package Manager Console)
4. Chạy lệnh: `Update-Database`
5. Nhấn F5 để chạy ứng dụng

## Cơ Sở Dữ Liệu

### Kết Nối
Connection String được định nghĩa trong `appsettings.json`:
```json
"ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BookDB;Trusted_Connection=true;"
}
```

### Thay Đổi Connection String
Nếu bạn muốn dùng SQL Server khác, hãy chỉnh sửa trong `appsettings.json`:
```json
"DefaultConnection": "Server=YourServerName;Database=BookDB;User Id=sa;Password=YourPassword;"
```

### Các Bảng Dữ Liệu

**Categories Table:**
- Id (int, Primary Key)
- Name (string, 100 characters)
- Description (string)

**Books Table:**
- Id (int, Primary Key)
- Title (string, 200 characters)
- Author (string, 100 characters)
- Description (string)
- Price (decimal)
- Quantity (int)
- PublicationDate (datetime)
- ImageUrl (string, nullable)
- CategoryId (int, Foreign Key)

## Entity Framework Core - Code First

Dự án sử dụng **Code First Approach** với Entity Framework Core:

1. **Models được định nghĩa trước**: Các class trong thư mục `Models/` định nghĩa cấu trúc dữ liệu
2. **DbContext**: Lớp `BookDbContext` trong `Data/BookDbContext.cs` đại diện cho cơ sở dữ liệu
3. **Migrations**: Các migration được tạo từ models
4. **Database**: Cơ sở dữ liệu được tạo từ migrations

### Tạo Migration Mới
Khi bạn thay đổi models, hãy tạo migration mới:
```bash
dotnet ef migrations add MigrationName
```

### Cập Nhật Database
Áp dụng migrations vào database:
```bash
dotnet ef database update
```

### Xem Danh Sách Migrations
```bash
dotnet ef migrations list
```

## Dữ Liệu Mẫu

Dự án đã tạo sẵn dữ liệu mẫu:

**Danh Mục:**
1. Công Nghệ Thông Tin
2. Văn Học
3. Kinh Tế

**Sách:**
1. Học Lập Trình C# - Nguyễn Văn A (250.000 đ)
2. Lập Trình Web với ASP.NET Core - Trần Văn B (320.000 đ)
3. Dạo Đức Kinh - Lão Tử (150.000 đ)

## Hướng Dẫn Sử Dụng

### Trang Chủ
- Hiển thị danh sách tất cả sách
- Có danh sách danh mục bên trái
- Nhấp vào danh mục để lọc sách

### Quản Lý Sách
- Xem tất cả sách: `/book`
- Thêm sách: `/book/create`
- Sửa sách: `/book/edit/{id}`
- Xóa sách: `/book/delete/{id}`

### Quản Lý Danh Mục
- Xem tất cả danh mục: `/category`
- Thêm danh mục: `/category/create`
- Sửa danh mục: `/category/edit/{id}`
- Xóa danh mục: `/category/delete/{id}`

## Lỗi Thường Gặp

### Lỗi: "Cannot open database"
**Giải pháp**: Kiểm tra connection string và SQL Server đang chạy

### Lỗi: "No database provider configured"
**Giải pháp**: Chạy `dotnet ef database update` để tạo database

### Lỗi: Migration pending
**Giải pháp**: 
```bash
dotnet ef database update
```

## Tài Liệu Tham Khảo

- [Entity Framework Core Documentation](https://learn.microsoft.com/en-us/ef/core/)
- [ASP.NET Core Documentation](https://learn.microsoft.com/en-us/aspnet/core/)
- [Code First Migrations](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/)

## Tác Giả

Bài Tập Số 5 - Lập Trình Web

---

**Lưu ý**: Đây là dự án học tập, vui lòng không sử dụng trong môi trường production mà không có thêm các tính năng bảo mật cần thiết.
