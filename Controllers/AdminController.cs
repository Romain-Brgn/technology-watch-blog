using Microsoft.AspNetCore.Mvc;
using TechnologyWatchBlog.Services;

namespace TechnologyWatchBlog.Controllers
{
    public class AdminController : Controller
    {
        private readonly RssImportService _rssImportService;
        private readonly OpenAiEmbeddingService _embeddingService;

        public AdminController(RssImportService rssImportService, OpenAiEmbeddingService embeddingService)
        {
            _rssImportService = rssImportService;
            _embeddingService = embeddingService;
        }

        [HttpGet]

        public async Task<IActionResult> GenerateEmbeddings()
        {
            int count = await _embeddingService.GenerateEmbeddingsAsync(200);

            return Content($"{count} embeddings générés.");
        }
        public async Task<IActionResult> ImportAll()
        {
            int total = await _rssImportService.ImportMultipleFeedsAsync();

            return Content($"{total} article(s) importé(s) depuis tous les flux.");
        }

    }
}