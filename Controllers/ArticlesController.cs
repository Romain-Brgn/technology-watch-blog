using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechnologyWatchBlog.Data;

namespace TechnologyWatchBlog.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly AppDbContext _context;

        public ArticlesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var articles = await _context.Articles
                .OrderByDescending(a => a.PublishedAt ?? a.CreatedAt)
                .ToListAsync();

            return View(articles);
        }
        public async Task<IActionResult> Details(string slug)
        {
            if (string.IsNullOrEmpty(slug))
                return NotFound();

            var article = await _context.Articles
                .FirstOrDefaultAsync(a => a.Slug == slug);

            if (article == null)
                return NotFound();

            return View(article);
        }
    }
}