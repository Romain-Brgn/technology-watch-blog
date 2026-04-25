using Microsoft.Extensions.DependencyInjection;

namespace TechnologyWatchBlog.Services
{
    public class EmbeddingBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EmbeddingBackgroundService> _logger;

        public EmbeddingBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<EmbeddingBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Embedding background service started.");
        
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
        
                try
                {
                    using var scope = _serviceProvider.CreateScope();
        
                    var embeddingService = scope.ServiceProvider
                        .GetRequiredService<OpenAiEmbeddingService>();
        
                    int count = await embeddingService.GenerateEmbeddingsAsync(100);
        
                    _logger.LogInformation("{Count} embeddings generated.", count);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during embedding generation.");
                }
            }
        }
    }
}