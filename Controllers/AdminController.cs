using Microsoft.AspNetCore.Mvc;
using TechnologyWatchBlog.Services;

namespace TechnologyWatchBlog.Controllers
{
    public class AdminController : Controller
    {
        private readonly RssImportService _rssImportService;

        public AdminController(RssImportService rssImportService)
        {
            _rssImportService = rssImportService;
        }

        [HttpGet]
        public async Task<IActionResult> ImportAll()
        {
            int total = await _rssImportService.ImportMultipleFeedsAsync();

            return Content($"{total} article(s) importé(s) depuis tous les flux.");
        }
    }
}