using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TechnologyWatchBlog.Models;
using TechnologyWatchBlog.Data;
using Microsoft.EntityFrameworkCore;

namespace TechnologyWatchBlog.Controllers;

public class HomeController : Controller
{

    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index()
        {
            ViewBag.ArticleCount = await _context.Articles.CountAsync();
            return View();
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
