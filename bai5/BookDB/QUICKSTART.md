# Hướng Dẫn Nhanh Bắt Đầu BookDB

## 🚀 Chạy Ứng Dụng

### Bước 1: Mở Terminal
Mở PowerShell hoặc Command Prompt trong thư mục `BookDB`

### Bước 2: Khôi Phục Packages
```powershell
dotnet restore
```

### Bước 3: Tạo Database từ Code First
```powershell
dotnet ef database update
```

Lệnh này sẽ:
- Tạo initial migration
- Tạo SQL Server database tên "BookDB"
- Tạo các bảng Categories và Books
- Thêm dữ liệu mẫu

### Bước 4: Chạy Ứng Dụng
```powershell
dotnet run
```

Ứng dụng sẽ chạy tại: `https://localhost:7001`

---

## 📋 Các Lệnh Entity Framework Core

### Tạo Migration Mới (khi thay đổi Models)
```powershell
dotnet ef migrations add NameOfMigration
```
Ví dụ:
```powershell
dotnet ef migrations add AddPriceColumn
```

### Xem Danh Sách Migrations
```powershell
dotnet ef migrations list
```

### Cập Nhật Database
```powershell
dotnet ef database update
```

### Xóa Migration Cuối Cùng
```powershell
dotnet ef migrations remove
```

### Rollback Database đến Migration Cụ Thể
```powershell
dotnet ef database update MigrationName
```

---

## 🌐 Các Route Chính

| URL | Mô Tả |
|-----|-------|
| `/` | Trang chủ - hiển thị danh sách sách |
| `/home/bycategory/{id}` | Xem sách theo danh mục |
| `/book` | Quản lý sách (danh sách) |
| `/book/create` | Thêm sách mới |
| `/book/edit/{id}` | Sửa thông tin sách |
| `/book/delete/{id}` | Xóa sách |
| `/category` | Quản lý danh mục |
| `/category/create` | Thêm danh mục |
| `/category/edit/{id}` | Sửa danh mục |
| `/category/delete/{id}` | Xóa danh mục |

---

## 🗄️ Cấu Hình Connection String

### Nếu dùng SQL Server Local
`appsettings.json`:
```json
"ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BookDB;Trusted_Connection=true;"
}
```

### Nếu dùng SQL Server trên máy khác
```json
"ConnectionStrings": {
    "DefaultConnection": "Server=IPAddress;Database=BookDB;User Id=sa;Password=Password123;"
}
```

---

## ⚙️ Cấu Trúc Dự Án

```
BookDB/
├── Models/
│   ├── Book.cs          ← Định nghĩa bảng Books
│   ├── Category.cs      ← Định nghĩa bảng Categories
│   └── ErrorViewModel.cs
├── Controllers/
│   ├── HomeController.cs      ← Hiển thị sách
│   ├── BookController.cs      ← Quản lý sách (CRUD)
│   └── CategoryController.cs  ← Quản lý danh mục (CRUD)
├── Views/               ← Giao diện (Razor)
│   ├── Home/
│   ├── Book/
│   ├── Category/
│   └── Shared/
├── Data/
│   └── BookDbContext.cs ← Kết nối database & config EF
├── Migrations/          ← Database migrations (tự động tạo)
├── wwwroot/            ← CSS, JS, images
├── appsettings.json    ← Cấu hình kết nối database
└── Program.cs          ← Entry point
```

---

## 🔍 Code First Workflow

```
1. Tạo Models (Book.cs, Category.cs)
         ↓
2. Tạo DbContext (BookDbContext.cs)
         ↓
3. Chạy: dotnet ef migrations add Initial
         ↓
4. Xem migration file được tạo
         ↓
5. Chạy: dotnet ef database update
         ↓
6. Database được tạo từ models!
```

---

## 🆘 Gỡ Rối

### Lỗi: Package dependency version conflict
```powershell
dotnet clean
dotnet restore
```

### Lỗi: Database connection failed
- Kiểm tra SQL Server đang chạy
- Kiểm tra connection string trong `appsettings.json`

### Muốn xóa database và tạo lại
```powershell
dotnet ef database drop
dotnet ef database update
```

---

## 📚 Học Thêm

- Model: `Models/Book.cs` - Xem cách định nghĩa entity
- DbContext: `Data/BookDbContext.cs` - Xem cách config relationships
- Controller: `Controllers/BookController.cs` - Xem CRUD operations
- View: `Views/Book/Index.cshtml` - Xem cách display dữ liệu

---

**Happy Coding! 🎉**
