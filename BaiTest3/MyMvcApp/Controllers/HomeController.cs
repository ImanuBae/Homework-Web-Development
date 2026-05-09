using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Models;

namespace MyMvcApp.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult BaiTap2()
    {
        var sanpham = new SanPhamViewModel
        {
            TenSanPham = "Laptop Lenovo Legion 5 Pro 16IAH7H",
            GiaBan = 25000000,
            AnhMoTa = "/images/images.jpg"
        };

        return View(sanpham);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
