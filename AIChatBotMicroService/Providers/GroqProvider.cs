using System.Text;
using System.Text.Json;
using AIChatBotMicroService.Providers;

namespace AIChatBotMicroService.Providers
{
    public class GroqProvider : IAIProvider
    {
        private readonly IConfiguration _configuration;

        public GroqProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> AnalyzeAsync(string prompt)
        {
            var apiKey = _configuration["Groq:ApiKey"];

            using var client = new HttpClient();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                $"Bearer {apiKey}");

            var requestBody = new
            {
                model = "llama-3.3-70b-versatile",

                messages = new[]
                {
                    new
                    {
                        role = "user",
                        content = prompt
                    }
                },

                temperature = 0.3
            };

            var json = JsonSerializer.Serialize(requestBody);

            var response = await client.PostAsync(
                "https://api.groq.com/openai/v1/chat/completions",
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json"));

            var responseContent =
                await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"Groq Error:\n{responseContent}");
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