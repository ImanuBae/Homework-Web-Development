using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Models;

namespace MyMvcApp.Controllers;

public class StudentController : Controller
{
    private static readonly List<StudentViewModel> RegisteredStudents = new();
    private static readonly string[] Majors = new[] { "CNPM", "HTTT", "ANM", "TTNT", "MMT" };

    public IActionResult Index()
    {
        ViewBag.Majors = Majors;
        return View(new StudentViewModel());
    }

    [HttpPost]
    public IActionResult Index(StudentViewModel model)
    {
        ViewBag.Majors = Majors;

        if (string.IsNullOrWhiteSpace(model.ChuyenNganh) || !Majors.Contains(model.ChuyenNganh))
        {
            ModelState.AddModelError(nameof(model.ChuyenNganh), "Vui lòng chọn chuyên ngành.");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        RegisteredStudents.Add(model);
        ViewBag.CountSameMajor = RegisteredStudents.Count(s => s.ChuyenNganh == model.ChuyenNganh);
        return View("ShowKQ", model);
    }
}
