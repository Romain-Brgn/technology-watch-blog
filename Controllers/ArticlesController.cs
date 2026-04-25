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

        public async Task<IActionResult> Index(List<string>? tags, string? sort)
        {
            var query = _context.Articles.AsQueryable();

            if (tags != null && tags.Any())
            {
                foreach (var selectedTag in tags)
                {
                    query = query.Where(a =>
                        a.Tags != null &&
                        a.Tags.ToLower().Contains(selectedTag.ToLower())
                    );
                }
            }

            query = sort switch
            {
                "old" => query.OrderBy(a => a.PublishedAt ?? a.CreatedAt),
                _ => query.OrderByDescending(a => a.PublishedAt ?? a.CreatedAt)
            };

            var articles = await query.ToListAsync();

            var allTags = await _context.Articles
                .Where(a => a.Tags != null && a.Tags != "")
                .Select(a => a.Tags!)
                .ToListAsync();

            ViewBag.AllTags = new List<string>
            {
                "data",
                "ai",
                "agents",
                "dotnet",
                "csharp",
                "python",
                "php",
                "javascript",
                "node",
                "llm"
            };

            ViewBag.CurrentTags = tags ?? new List<string>();
            ViewBag.CurrentSort = sort;

            return View(articles);
        }

        public async Task<IActionResult> Details(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
                return NotFound();

            var article = await _context.Articles
                .FirstOrDefaultAsync(a => a.Slug == slug);

            if (article == null)
                return NotFound();

            return View(article);
        }
    }
}