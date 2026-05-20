using Microsoft.AspNetCore.Mvc;
using DemoApp.Models;
using System.Security.Cryptography;
using System.Text;

namespace DemoApp.Controllers;

public class AccountController : Controller
{
    // Lưu trữ user tạm thời (trong thực tế dùng database)
    private static List<User> Users = new();

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(RegisterViewModel model)
    {
        // Kiểm tra xem ModelState có hợp lệ không (tự động do Data Annotation)
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Kiểm tra username đã tồn tại chưa
        if (Users.Any(u => u.Username == model.Username))
        {
            ModelState.AddModelError("Username", "Tên đăng nhập đã được sử dụng");
            return View(model);
        }

        // Kiểm tra email đã tồn tại chưa
        if (Users.Any(u => u.Email == model.Email))
        {
            ModelState.AddModelError("Email", "Email đã được đăng ký");
            return View(model);
        }

        // Tạo user mới
        var newUser = new User
        {
            Id = Users.Count + 1,
            Username = model.Username,
            Email = model.Email,
            FullName = model.FullName,
            PasswordHash = HashPassword(model.Password), // Mã hóa mật khẩu
            CreatedDate = DateTime.Now
        };

        Users.Add(newUser);

        TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        // Kiểm tra validation
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Tìm user theo username
        var user = Users.FirstOrDefault(u => u.Username == model.Username);

        if (user == null || !VerifyPassword(model.Password, user.PasswordHash))
        {
            ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
            return View(model);
        }

        // Đăng nhập thành công
        HttpContext.Session.SetString("Username", user.Username);
        HttpContext.Session.SetString("UserId", user.Id.ToString());
        HttpContext.Session.SetString("FullName", user.FullName);

        TempData["SuccessMessage"] = $"Chào mừng {user.FullName}!";
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        TempData["SuccessMessage"] = "Đã đăng xuất thành công.";
        return RedirectToAction("Index", "Home");
    }

    // Hàm mã hóa mật khẩu bằng SHA256
    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }

    // Hàm xác minh mật khẩu
    private bool VerifyPassword(string password, string hash)
    {
        var hashOfInput = HashPassword(password);
        return hashOfInput == hash;
    }
}
