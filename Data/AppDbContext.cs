using Microsoft.EntityFrameworkCore;
using TechnologyWatchBlog.Models;

namespace TechnologyWatchBlog.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Article> Articles { get; set; }
    }
}