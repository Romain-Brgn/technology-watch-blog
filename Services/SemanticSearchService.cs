using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using TechnologyWatchBlog.Data;
using TechnologyWatchBlog.Models;

namespace TechnologyWatchBlog.Services
{
    public class SemanticSearchService
    {
        private readonly AppDbContext _context;
        private readonly OpenAiEmbeddingService _embeddingService;

        public SemanticSearchService(AppDbContext context, OpenAiEmbeddingService embeddingService)
        {
            _context = context;
            _embeddingService = embeddingService;
        }

        public async Task<List<Article>> SearchAsync(string question, int limit = 3)
        {
            var questionEmbedding = await _embeddingService.GenerateQueryEmbeddingAsync(question);

            if (questionEmbedding.Length == 0)
                return new List<Article>();

            var cutoffDate = DateTime.UtcNow.AddDays(-30);

            var articles = await _context.Articles
                .Where(a => a.Embedding != null &&
                            (a.PublishedAt ?? a.CreatedAt) >= cutoffDate)
                .ToListAsync();

            var rankedArticles = articles
                .Select(article =>
                {
                    var articleEmbedding = JsonSerializer.Deserialize<float[]>(article.Embedding!);

                    var score = articleEmbedding == null
                        ? 0
                        : CosineSimilarity(questionEmbedding, articleEmbedding);

                    return new
                    {
                        Article = article,
                        Score = score
                    };
                })
                .OrderByDescending(x => x.Score)
                .Take(limit)
                .Select(x => x.Article)
                .ToList();

            return rankedArticles;
        }

        private double CosineSimilarity(float[] vectorA, float[] vectorB)
        {
            if (vectorA.Length != vectorB.Length)
                return 0;

            double dot = 0;
            double normA = 0;
            double normB = 0;

            for (int i = 0; i < vectorA.Length; i++)
            {
                dot += vectorA[i] * vectorB[i];
                normA += vectorA[i] * vectorA[i];
                normB += vectorB[i] * vectorB[i];
            }

            if (normA == 0 || normB == 0)
                return 0;

            return dot / (Math.Sqrt(normA) * Math.Sqrt(normB));
        }
    }
}