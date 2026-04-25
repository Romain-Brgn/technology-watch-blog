using OpenAI.Embeddings;
using TechnologyWatchBlog.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace TechnologyWatchBlog.Services
{
    public class OpenAiEmbeddingService
    {
        private readonly AppDbContext _context;
        private readonly EmbeddingClient _client;
        private readonly ILogger<EmbeddingBackgroundService> _logger;

        public OpenAiEmbeddingService(IConfiguration config, AppDbContext context,ILogger<EmbeddingBackgroundService> logger)
        {
            _context = context;
            _logger = logger;

            var apiKey = config["OpenAI:ApiKey"];
            _client = new EmbeddingClient("text-embedding-3-small", apiKey);
        }

        public async Task<int> GenerateEmbeddingsAsync(int limit = 20)
        {
            var articles = await _context.Articles
                .Where(a => a.Embedding == null)
                .OrderByDescending(a => a.PublishedAt ?? a.CreatedAt)
                .Take(limit)
                .ToListAsync();

            int count = 0;

            foreach (var article in articles)
            {
                try
                {
                    var text = article.CleanContent;

                    if (string.IsNullOrWhiteSpace(text))
                        continue;

                    if (text.Length > 20000)
                        text = text.Substring(0, 20000);

                    var result = await _client.GenerateEmbeddingAsync(text);

                    float[] vector = result.Value.ToFloats().ToArray();

                    article.Embedding = JsonSerializer.Serialize(vector);
                    article.EmbeddedAt = DateTime.UtcNow;

                    count++;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Embedding error");
                }
            }

            await _context.SaveChangesAsync();
            return count;
        }
        public async Task<float[]> GenerateQueryEmbeddingAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            return Array.Empty<float>();

            var result = await _client.GenerateEmbeddingAsync(text);

            return result.Value.ToFloats().ToArray();
        }
    }
}