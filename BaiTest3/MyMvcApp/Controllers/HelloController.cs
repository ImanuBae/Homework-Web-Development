using Microsoft.AspNetCore.Mvc;

namespace MyMvcApp.Controllers;

public class HelloController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
