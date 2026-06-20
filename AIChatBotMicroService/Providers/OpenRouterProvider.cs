using System.Text;
using System.Text.Json;

namespace AIChatBotMicroService.Providers
{
    public class OpenRouterProvider : IAIProvider
    {
        private readonly IConfiguration _configuration;

        public OpenRouterProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> AnalyzeAsync(string prompt)
        {
            var apiKey = _configuration["OpenRouter:ApiKey"];

            using var client = new HttpClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                $"Bearer {apiKey}");

            var requestBody = new
            {
                model = "deepseek/deepseek-chat-v3-0324:free",

                messages = new[]
                {
                    new
                    {
                        role = "user",
                        content = prompt
                    }
                }
            };

            var json = JsonSerializer.Serialize(requestBody);

            var response = await client.PostAsync(
                "https://openrouter.ai/api/v1/chat/completions",
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json"));

            var responseContent =
                await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"OpenRouter Error:\n{responseContent}");
            }

            using var document =
                JsonDocument.Parse(responseContent);

            return document
                .RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString()
                ?? "No response";
        }
    }
}