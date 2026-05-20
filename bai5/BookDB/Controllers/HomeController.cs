using BookDB.Data;
using BookDB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BookDB.Controllers
{
    public class HomeController : Controller
    {
        private readonly BookDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(BookDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var books = await _context.Books.Include(b => b.Category).ToListAsync();
            ViewBag.Categories = await _context.Categories.Include(c => c.Books).ToListAsync();
            return View(books);
        }

        public async Task<IActionResult> ByCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            var books = await _context.Books
                .Where(b => b.CategoryId == id)
                .Include(b => b.Category)
                .ToListAsync();

            ViewBag.Categories = await _context.Categories.Include(c => c.Books).ToListAsync();
            ViewBag.CategoryName = category.Name;
            return View("Index", books);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var book = await _context.Books
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (book == null)
                return NotFound();

            return View(book);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
