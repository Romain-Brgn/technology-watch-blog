using OpenAI.Chat;
using TechnologyWatchBlog.Models;
using System.Text;

namespace TechnologyWatchBlog.Services
{
    public class OpenAiChatService
    {
        private readonly ChatClient _client;

        public OpenAiChatService(IConfiguration config)
        {
            var apiKey = config["OpenAI:ApiKey"];
            _client = new ChatClient("gpt-4o-mini", apiKey);
        }

        public async Task<string> AskAsync(string question, List<Article> articles)
        {
            var context = BuildContext(articles);

            var prompt = $"""
            Tu es un assistant spécialisé en veille technologique.

            Réponds à la question en te basant UNIQUEMENT sur les articles fournis.

            Question :
            {question}

            Articles :
            {context}

            Règles :
            - Sois clair et structuré
            - Résume les informations importantes
            - Ne fabrique rien
            - Si tu ne sais pas, dis-le
            """;

            var response = await _client.CompleteChatAsync(prompt);

            return response.Value.Content[0].Text;
        }

        private string BuildContext(List<Article> articles)
        {
            var sb = new StringBuilder();

            foreach (var article in articles)
            {
                sb.AppendLine($"Titre: {article.Title}");
                sb.AppendLine($"Contenu: {article.CleanContent.Substring(0, Math.Min(2000, article.CleanContent.Length))}");
                sb.AppendLine("----");
            }

            return sb.ToString();
        }
    }
}