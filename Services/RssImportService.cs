using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Xml;
using TechnologyWatchBlog.Data;
using TechnologyWatchBlog.Models;

namespace TechnologyWatchBlog.Services
{
    public class RssImportService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<RssImportService> _logger;

        public RssImportService(AppDbContext context, ILogger<RssImportService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<int> ImportMultipleFeedsAsync()
        {
            var feeds = new List<string>
            {
                "https://dev.to/feed/tag/ai",
                "https://dev.to/feed/tag/javascript",
                "https://dev.to/feed/tag/python",
                "https://dev.to/feed/tag/dotnet",
                "https://dev.to/feed/tag/csharp",
                "https://dev.to/feed/tag/java",
                "https://dev.to/feed/tag/agents",
                "https://dev.to/feed/tag/cybersecurity",
                "https://dev.to/feed/tag/vulnerability",
                "https://dev.to/feed/tag/automation",
                "https://dev.to/feed/tag/llm",
                "https://dev.to/feed/tag/azure",
                "https://dev.to/feed/tag/deeplearning",
                "https://dev.to/feed/tag/machinelearning",
                "https://dev.to/feed/tag/computerscience",
                "https://dev.to/feed/tag/dataengineering",
                "https://dev.to/feed/tag/backend",
                "https://dev.to/feed/tag/node",
                "https://dev.to/feed/tag/claude",
                "https://dev.to/feed/tag/anthropic",
                "https://dev.to/feed/tag/google"
            };

            int totalImported = 0;

            foreach (var feedUrl in feeds)
            {
                try
                {
                    _logger.LogInformation("Importing: {Url}", feedUrl);
                    int count = await ImportFromUrlAsync(feedUrl);
                    totalImported += count;
                    _logger.LogInformation("Imported {Count} article(s) from {Url}", count, feedUrl);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error importing from {Url}", feedUrl);
                }
            }

            return totalImported;
        }

        public async Task<int> ImportFromUrlAsync(string feedUrl)
        {
            using var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(30);

            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0 Safari/537.36"
            );

            httpClient.DefaultRequestHeaders.Accept.ParseAdd(
                "application/rss+xml, application/xml, text/xml, */*"
            );

            var xml = await httpClient.GetStringAsync(feedUrl);

            using var stringReader = new StringReader(xml);
            using var reader = XmlReader.Create(stringReader);

            var feed = SyndicationFeed.Load(reader);

            if (feed == null)
                return 0;

            int importedCount = 0;

            foreach (var item in feed.Items)
            {
                var sourceUrl = item.Links.FirstOrDefault()?.Uri?.ToString();

                if (string.IsNullOrWhiteSpace(sourceUrl))
                    continue;

                bool alreadyExists = _context.Articles.Any(a => a.SourceUrl == sourceUrl);

                if (alreadyExists)
                    continue;

                string title = item.Title?.Text ?? "Sans titre";
                string contentHtml = GetContentFromItem(item);
                string cleanContent = StripHtml(contentHtml);
                string excerpt = BuildExcerptFromCleanText(cleanContent);
                string author = item.Authors.FirstOrDefault()?.Name ?? string.Empty;
                string tags = string.Join(", ", item.Categories.Select(c => c.Name));

                var article = new Article
                {
                    Title = title,
                    Slug = GenerateSlug(title),
                    Content = contentHtml,
                    CleanContent = cleanContent,
                    Excerpt = excerpt,
                    SourceUrl = sourceUrl,
                    SourceName = "dev.to",
                    Author = author,
                    Tags = tags,
                    ImageUrl = null,
                    PublishedAt = item.PublishDate != DateTimeOffset.MinValue ? item.PublishDate.UtcDateTime : null,
                    UpdatedAt = item.LastUpdatedTime != DateTimeOffset.MinValue ? item.LastUpdatedTime.UtcDateTime : null,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Articles.Add(article);
                importedCount++;
            }

            await _context.SaveChangesAsync();
            return importedCount;
        }

        private string GetContentFromItem(SyndicationItem item)
        {
            if (item.Summary != null)
                return item.Summary.Text ?? string.Empty;

            return string.Empty;
        }

        private string BuildExcerptFromCleanText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            return text.Length > 220
                ? text.Substring(0, 220) + "..."
                : text;
        }

        private string StripHtml(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return string.Empty;

            string text = Regex.Replace(html, "<.*?>", " ");
            text = System.Net.WebUtility.HtmlDecode(text);
            text = Regex.Replace(text, @"\s+", " ");

            return text.Trim();
        }

        private string GenerateSlug(string text)
        {
            string slug = text.ToLower().Trim();
            slug = System.Net.WebUtility.HtmlDecode(slug);
            slug = Regex.Replace(slug, @"\s+", "-");
            slug = Regex.Replace(slug, @"[^a-z0-9\-]", "");
            slug = Regex.Replace(slug, @"-+", "-");

            return slug.Trim('-');
        }
    }
}