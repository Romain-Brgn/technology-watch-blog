using System;

namespace TechnologyWatchBlog.Models
{
    public class Article
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string CleanContent { get; set; } = string.Empty;

        public string Excerpt { get; set; } = string.Empty;

        public string SourceUrl { get; set; } = string.Empty;

        public string SourceName { get; set; } = string.Empty;

        public string? Author { get; set; }

        public string? Tags { get; set; }

        public string? ImageUrl { get; set; }

        public string? Embedding { get; set; }

        public DateTime? EmbeddedAt { get; set; }

        public DateTime? PublishedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}