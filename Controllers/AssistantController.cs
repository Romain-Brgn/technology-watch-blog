using Microsoft.AspNetCore.Mvc;
using TechnologyWatchBlog.Services;

namespace TechnologyWatchBlog.Controllers
{
    public class AssistantController : Controller
    {
        private readonly SemanticSearchService _semanticSearchService;
        private readonly OpenAiChatService _chatService;

        public AssistantController(
            SemanticSearchService semanticSearchService,
            OpenAiChatService chatService)
        {
            _semanticSearchService = semanticSearchService;
            _chatService = chatService;
        }

        [HttpGet]
        public async Task<IActionResult> Ask(string question)
        {
            if (string.IsNullOrWhiteSpace(question))
                return BadRequest("Question manquante.");

            var articles = await _semanticSearchService.SearchAsync(question, 5);

            var answer = await _chatService.AskAsync(question, articles);

            var sources = articles.Select(a => new
            {
                a.Title,
                Url = Url.Action("Details", "Articles", new { slug = a.Slug }, Request.Scheme)
            });

            return Json(new
            {
                answer,
                sources
            });
        }
    }
}