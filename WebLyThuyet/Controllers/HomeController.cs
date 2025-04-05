using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebLyThuyet.Models;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context; // Đổi DbContext thành ApplicationDbContext

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int? categoryId, string searchTerm)
    {
        var products = _context.Products
            .Include(p => p.Category)
            .AsQueryable();

        if (categoryId.HasValue)
        {
            products = products.Where(p => p.CategoryId == categoryId.Value);
        }

        if (!string.IsNullOrEmpty(searchTerm))
        {
            products = products.Where(p =>
                p.Name.Contains(searchTerm) ||
                (p.Category != null && p.Category.Name.Contains(searchTerm))
            );
        }

        ViewBag.Categories = await _context.Categories.ToListAsync();
        ViewBag.SearchTerm = searchTerm;

        return View(await products.ToListAsync());
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

    [HttpGet]
    public async Task<IActionResult> SearchSuggestions(string term)
    {
        if (string.IsNullOrWhiteSpace(term))
            return Json(new List<object>());

        var query = _context.Products
            .Include(p => p.Category)
            .AsQueryable();

        term = term.ToLower();

        query = query.Where(p =>
            EF.Functions.Like(p.Name.ToLower(), $"%{term}%") ||
            (p.Category != null && EF.Functions.Like(p.Category.Name.ToLower(), $"%{term}%"))
        );

        var suggestions = await query.Select(p => new
        {
            p.Id,
            p.Name,
            p.Price,
            p.ImageUrl
        })
        .Take(5)
        .ToListAsync();

        return Json(suggestions);
    }

}

