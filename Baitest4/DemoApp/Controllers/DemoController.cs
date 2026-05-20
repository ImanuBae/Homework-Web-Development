using Microsoft.AspNetCore.Mvc;
using DemoApp.Models;

namespace DemoApp.Controllers;

public class DemoController : Controller
{
    public IActionResult Index()
    {
        var actors = new List<Actor>
        {
            new Actor { Name = "Phương Anh Đào", Height = 160, Role = "Mai" },
            new Actor { Name = "Tuấn Trần", Height = 170, Role = "Dương (Sâu)" },
            new Actor { Name = "Trần Thành", Height = 150, Role = "Ông Hoàng" }
        };

        return View(actors);
    }
}
