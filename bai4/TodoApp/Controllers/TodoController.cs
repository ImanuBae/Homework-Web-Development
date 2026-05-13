using Microsoft.AspNetCore.Mvc;
using TodoApp.Models;

namespace TodoApp.Controllers;

public class TodoController : Controller
{
    private static List<TodoItem> _todos = new()
    {
        new TodoItem { Id = 1, TenCongViec = "Đi chợ",       HoanThanh = false },
        new TodoItem { Id = 2, TenCongViec = "Chơi thể thao", HoanThanh = false },
        new TodoItem { Id = 3, TenCongViec = "Chơi game",     HoanThanh = false },
        new TodoItem { Id = 4, TenCongViec = "Học bài",       HoanThanh = true  },
    };
    private static int _nextId = 5;

    public IActionResult Index() => View(_todos);

    public IActionResult Details(int id)
    {
        var item = _todos.FirstOrDefault(t => t.Id == id);
        if (item == null) return NotFound();
        return View(item);
    }

    [HttpGet]
    public IActionResult Create() => View(new TodoItem());

    [HttpPost]
    public IActionResult Create(TodoItem item)
    {
        if (!ModelState.IsValid) return View(item);
        item.Id = _nextId++;
        _todos.Add(item);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var item = _todos.FirstOrDefault(t => t.Id == id);
        if (item == null) return NotFound();
        return View(item);
    }

    [HttpPost]
    public IActionResult Edit(TodoItem updated)
    {
        if (!ModelState.IsValid) return View(updated);
        var item = _todos.FirstOrDefault(t => t.Id == updated.Id);
        if (item == null) return NotFound();
        item.TenCongViec = updated.TenCongViec;
        item.HoanThanh   = updated.HoanThanh;
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var item = _todos.FirstOrDefault(t => t.Id == id);
        if (item != null) _todos.Remove(item);
        return RedirectToAction(nameof(Index));
    }
}
