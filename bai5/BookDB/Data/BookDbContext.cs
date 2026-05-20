using BookDB.Models;
using Microsoft.EntityFrameworkCore;

namespace BookDB.Data
{
    public class BookDbContext : DbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Category
            modelBuilder.Entity<Category>()
                .HasKey(c => c.Id);
            
            modelBuilder.Entity<Category>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Configure Book
            modelBuilder.Entity<Book>()
                .HasKey(b => b.Id);
            
            modelBuilder.Entity<Book>()
                .Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<Book>()
                .Property(b => b.Author)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Book>()
                .Property(b => b.Price)
                .HasPrecision(18, 2);

            // Configure relationship
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed initial data
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Công Nghệ Thông Tin", Description = "Sách về lập trình và IT" },
                new Category { Id = 2, Name = "Văn Học", Description = "Sách văn học" },
                new Category { Id = 3, Name = "Kinh Tế", Description = "Sách về kinh tế học" }
            );

            modelBuilder.Entity<Book>().HasData(
                new Book 
                { 
                    Id = 1, 
                    Title = "Học Lập Trình C#", 
                    Author = "Nguyễn Văn A", 
                    Description = "Sách hướng dẫn học C# từ cơ bản đến nâng cao",
                    Price = 250000,
                    Quantity = 10,
                    PublicationDate = new DateTime(2023, 1, 15),
                    CategoryId = 1
                },
                new Book 
                { 
                    Id = 2, 
                    Title = "Lập Trình Web với ASP.NET Core", 
                    Author = "Trần Văn B", 
                    Description = "Hướng dẫn lập trình web sử dụng ASP.NET Core",
                    Price = 320000,
                    Quantity = 15,
                    PublicationDate = new DateTime(2023, 6, 20),
                    CategoryId = 1
                },
                new Book 
                { 
                    Id = 3, 
                    Title = "Dạo Đức Kinh", 
                    Author = "Lão Tử", 
                    Description = "Kinh điển triết học Trung Quốc",
                    Price = 150000,
                    Quantity = 20,
                    PublicationDate = new DateTime(2022, 3, 10),
                    CategoryId = 2
                }
            );
        }
    }
}
