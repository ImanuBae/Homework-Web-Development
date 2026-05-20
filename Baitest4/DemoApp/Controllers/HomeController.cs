using Microsoft.AspNetCore.Mvc;

namespace DemoApp.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Guide()
    {
        return View();
    }
}
