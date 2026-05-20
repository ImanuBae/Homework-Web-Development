# Entity Framework Core - Code First Implementation Guide

## 📖 Giới Thiệu

Dự án BookDB sử dụng **Entity Framework Core 8.0** với **Code First Approach**. Điều này có nghĩa:

1. **Bạn viết code trước** (Models trong C#)
2. **EF Core tạo database** từ code đó
3. **Migrations theo dõi các thay đổi**

---

## 🏗️ Code First Architecture

```
┌─────────────────────────────────────────┐
│         Your C# Models (Code)          │
│  (Book.cs, Category.cs)                │
└──────────────────┬──────────────────────┘
                   │
                   ▼
┌─────────────────────────────────────────┐
│      DbContext (BookDbContext.cs)      │
│  - Kết nối Models với Database         │
│  - Config relationships                │
│  - Seed data                           │
└──────────────────┬──────────────────────┘
                   │
                   ▼
┌─────────────────────────────────────────┐
│      Migrations (Auto Generated)       │
│  - InitialCreate.cs                    │
│  - Track schema changes                │
└──────────────────┬──────────────────────┘
                   │
                   ▼
┌─────────────────────────────────────────┐
│      SQL Server Database                │
│  - Categories table                    │
│  - Books table                         │
│  - Foreign Keys & Indexes              │
└─────────────────────────────────────────┘
```

---

## 1️⃣ Models - Định Nghĩa Dữ Liệu

### Category Model (`Models/Category.cs`)

```csharp
public class Category
{
    public int Id { get; set; }  // Primary Key
    public string Name { get; set; }
    public string Description { get; set; }

    // Navigation property - một danh mục có nhiều sách
    public ICollection<Book> Books { get; set; }
}
```

**Tương ứng với SQL:**
```sql
CREATE TABLE Categories (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX)
);
```

### Book Model (`Models/Book.cs`)

```csharp
public class Book
{
    public int Id { get; set; }  // Primary Key
    public string Title { get; set; }
    public string Author { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public DateTime PublicationDate { get; set; }
    public string? ImageUrl { get; set; }
    
    // Foreign Key
    public int CategoryId { get; set; }
    
    // Navigation property - một sách thuộc một danh mục
    public Category? Category { get; set; }
}
```

**Tương ứng với SQL:**
```sql
CREATE TABLE Books (
    Id INT PRIMARY KEY IDENTITY,
    Title NVARCHAR(200) NOT NULL,
    Author NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX),
    Price DECIMAL(18,2),
    Quantity INT,
    PublicationDate DATETIME2,
    ImageUrl NVARCHAR(MAX),
    CategoryId INT NOT NULL,
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id) ON DELETE CASCADE
);
```

---

## 2️⃣ DbContext - Trung Tâm Dữ Liệu

### BookDbContext (`Data/BookDbContext.cs`)

```csharp
public class BookDbContext : DbContext
{
    // Constructor nhận options
    public BookDbContext(DbContextOptions<BookDbContext> options) 
        : base(options) { }

    // DbSets đại diện cho các bảng
    public DbSet<Category> Categories { get; set; }
    public DbSet<Book> Books { get; set; }

    // OnModelCreating - Config mô hình dữ liệu
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Config Category
        modelBuilder.Entity<Category>()
            .HasKey(c => c.Id);  // Set Primary Key
        
        modelBuilder.Entity<Category>()
            .Property(c => c.Name)
            .IsRequired()           // NOT NULL
            .HasMaxLength(100);     // VARCHAR(100)

        // Config relationship: 1 Category - Many Books
        modelBuilder.Entity<Book>()
            .HasOne(b => b.Category)
            .WithMany(c => c.Books)
            .HasForeignKey(b => b.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);  // Xóa danh mục -> xóa sách

        // Seed data
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "IT", Description = "..." },
            new Category { Id = 2, Name = "Literature", Description = "..." }
        );
    }
}
```

**Các phương thức quan trọng:**
- `DbSet<T>` - Truy cập bảng (như SELECT * FROM)
- `SaveChanges()` - Lưu thay đổi vào database
- `OnModelCreating()` - Config mô hình và relationships

---

## 3️⃣ Dependency Injection - Kết Nối DbContext

### Program.cs - Đăng Ký DbContext

```csharp
// Thêm DbContext vào DI Container
builder.Services.AddDbContext<BookDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);
```

**Điều này cho phép:**
- Controller nhận DbContext qua constructor
- ASP.NET Core tự động quản lý lifecycle
- Connection pooling tự động

---

## 4️⃣ Migrations - Theo Dõi Thay Đổi

### Tạo Migration

```powershell
dotnet ef migrations add InitialCreate
```

**Tạo ra file:**
- `Migrations/20240520000000_InitialCreate.cs` - Tạo bảng (UP)
- `Migrations/20240520000000_InitialCreate.Designer.cs` - Metadata
- `Migrations/BookDbContextModelSnapshot.cs` - Current state

### Nội Dung Migration

```csharp
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // CREATE TABLE Categories
        migrationBuilder.CreateTable(
            name: "Categories",
            columns: table => new
            {
                Id = table.Column<int>(),
                Name = table.Column<string>(maxLength: 100),
                Description = table.Column<string>()
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Categories", x => x.Id);
            });

        // CREATE TABLE Books
        migrationBuilder.CreateTable(
            name: "Books",
            columns: table => new
            {
                // ... columns ...
                CategoryId = table.Column<int>()
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Books", x => x.Id);
                table.ForeignKey(
                    name: "FK_Books_Categories_CategoryId",
                    column: x => x.CategoryId,
                    principalTable: "Categories",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // DROP TABLE Books
        migrationBuilder.DropTable(name: "Books");
        
        // DROP TABLE Categories
        migrationBuilder.DropTable(name: "Categories");
    }
}
```

### Áp Dụng Migration

```powershell
dotnet ef database update
```

**Điều này:**
1. Chạy `Up()` method của tất cả pending migrations
2. Tạo database nếu chưa tồn tại
3. Tạo các bảng theo migration
4. Cập nhật `__EFMigrationsHistory` table
5. Chạy seed data

---

## 5️⃣ CRUD Operations trong Controller

### Create (Thêm)

```csharp
[HttpPost]
public async Task<IActionResult> Create(Book book)
{
    _context.Add(book);              // Thêm vào DbContext
    await _context.SaveChangesAsync(); // INSERT vào database
    return RedirectToAction(nameof(Index));
}
```

**Thực tế SQL:**
```sql
INSERT INTO Books (Title, Author, Price, CategoryId, ...)
VALUES (@Title, @Author, @Price, @CategoryId, ...)
```

### Read (Đọc)

```csharp
// Lấy tất cả sách
var books = await _context.Books
    .Include(b => b.Category)  // JOIN với Category
    .ToListAsync();

// Lấy một sách theo ID
var book = await _context.Books
    .Include(b => b.Category)
    .FirstOrDefaultAsync(b => b.Id == id);

// Lọc theo danh mục
var books = await _context.Books
    .Where(b => b.CategoryId == categoryId)
    .ToListAsync();
```

**SQL được generate:**
```sql
-- Lấy tất cả
SELECT b.*, c.* FROM Books b
LEFT JOIN Categories c ON b.CategoryId = c.Id

-- Lọc theo danh mục
SELECT * FROM Books WHERE CategoryId = @categoryId
```

### Update (Sửa)

```csharp
[HttpPost]
public async Task<IActionResult> Edit(int id, Book book)
{
    book.Id = id;
    _context.Update(book);              // Mark as modified
    await _context.SaveChangesAsync();  // UPDATE database
    return RedirectToAction(nameof(Index));
}
```

**SQL:**
```sql
UPDATE Books SET Title = @Title, Author = @Author, ... 
WHERE Id = @Id
```

### Delete (Xóa)

```csharp
[HttpPost]
public async Task<IActionResult> DeleteConfirmed(int id)
{
    var book = await _context.Books.FindAsync(id);
    _context.Books.Remove(book);         // Mark for deletion
    await _context.SaveChangesAsync();   // DELETE from database
    return RedirectToAction(nameof(Index));
}
```

**SQL:**
```sql
DELETE FROM Books WHERE Id = @Id
```

---

## 6️⃣ Relationships - Mối Quan Hệ Dữ Liệu

### One-to-Many (1:N)
1 Category → Many Books

```csharp
// Category có nhiều Books
public ICollection<Book> Books { get; set; }

// Book thuộc một Category
public int CategoryId { get; set; }
public Category? Category { get; set; }

// Config trong OnModelCreating
modelBuilder.Entity<Book>()
    .HasOne(b => b.Category)
    .WithMany(c => c.Books)
    .HasForeignKey(b => b.CategoryId)
    .OnDelete(DeleteBehavior.Cascade);
```

**SQL:**
```sql
ALTER TABLE Books
ADD CONSTRAINT FK_Books_Categories_CategoryId
FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
ON DELETE CASCADE;
```

---

## 7️⃣ Advanced Features

### Include - Eager Loading
```csharp
// Load Category cùng Book
var books = await _context.Books
    .Include(b => b.Category)
    .ToListAsync();
```

### Where - Filtering
```csharp
var expensiveBooks = await _context.Books
    .Where(b => b.Price > 100000)
    .ToListAsync();
```

### Select - Projection
```csharp
var titles = await _context.Books
    .Select(b => new { b.Title, b.Author })
    .ToListAsync();
```

### OrderBy - Sorting
```csharp
var sortedBooks = await _context.Books
    .OrderByDescending(b => b.Price)
    .ToListAsync();
```

### Async Operations
```csharp
// Async methods
var books = await _context.Books.ToListAsync();
var book = await _context.Books.FindAsync(id);
await _context.SaveChangesAsync();
```

---

## 8️⃣ Workflow: Thay Đổi Schema

### Scenario: Thêm column "Rating"

**Bước 1: Sửa Model**
```csharp
public class Book
{
    // ... existing properties ...
    public int Rating { get; set; } = 5;  // Thêm property mới
}
```

**Bước 2: Tạo Migration**
```powershell
dotnet ef migrations add AddRatingToBook
```

**Bước 3: Xem Migration**
```csharp
migrationBuilder.AddColumn<int>(
    name: "Rating",
    table: "Books",
    type: "int",
    nullable: false,
    defaultValue: 5);  // Giá trị mặc định cho dữ liệu hiện tại
```

**Bước 4: Áp Dụng**
```powershell
dotnet ef database update
```

**Bước 5: Sử Dụng**
```csharp
var book = new Book 
{ 
    Title = "...", 
    Rating = 4  // Sử dụng property mới 
};
```

---

## 🔧 Lệnh Đông Dùng

| Lệnh | Mô Tả |
|------|-------|
| `dotnet ef migrations add Name` | Tạo migration mới |
| `dotnet ef database update` | Áp dụng migrations |
| `dotnet ef migrations list` | Xem danh sách migrations |
| `dotnet ef migrations remove` | Xóa migration cuối |
| `dotnet ef database drop` | Xóa database |
| `dotnet ef dbcontext info` | Xem thông tin DbContext |
| `dotnet ef migrations script` | Tạo SQL script |

---

## 📚 Best Practices

✅ **Nên làm:**
- Luôn dùng `async/await` cho database operations
- Dùng `.Include()` để tránh N+1 queries
- Validate data trước khi save
- Dùng migrations cho mọi schema changes
- Commit migrations vào version control

❌ **Không nên làm:**
- Trực tiếp edit migration files
- Xóa database và tạo lại thay vì dùng migrations
- Lưu password trong code (dùng User Secrets)
- Quên thêm `await` cho async operations
- N+1 queries (load Category cho mỗi Book)

---

## 🎓 Tài Liệu Thêm

- [EF Core Documentation](https://learn.microsoft.com/en-us/ef/core/)
- [Code First Migrations](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
- [Relationships](https://learn.microsoft.com/en-us/ef/core/modeling/relationships/)
- [Shadow Properties & Annotations](https://learn.microsoft.com/en-us/ef/core/modeling/)

---

**Happy coding with Entity Framework Core! 🚀**
