using Microsoft.Extensions.DependencyInjection;

namespace TechnologyWatchBlog.Services
{
    public class RssBackgroundImportService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<RssBackgroundImportService> _logger;

        public RssBackgroundImportService(
            IServiceProvider serviceProvider,
            ILogger<RssBackgroundImportService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("RSS background import service started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();

                    var rssImportService = scope.ServiceProvider
                        .GetRequiredService<RssImportService>();

                    int imported = await rssImportService.ImportMultipleFeedsAsync();

                    _logger.LogInformation(
                        "RSS background import completed. {Count} article(s) imported.",
                        imported
                    );
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during RSS background import.");
                }

                await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
            }
        }
    }
}