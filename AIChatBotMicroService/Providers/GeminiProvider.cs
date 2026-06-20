using System.Text;
using System.Text.Json;

namespace AIChatBotMicroService.Providers
{
    public class GeminiProvider : IAIProvider
    {
        private readonly IConfiguration _configuration;

        public GeminiProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> AnalyzeAsync(string prompt)
        {
            var apiKey = _configuration["Gemini:ApiKey"];

            Console.WriteLine("========== GEMINI DEBUG ==========");

            if (string.IsNullOrEmpty(apiKey))
            {
                throw new Exception("Gemini ApiKey is NULL or Empty");
            }

            Console.WriteLine(
                $"API Key: {apiKey.Substring(0, Math.Min(10, apiKey.Length))}...");

            using var client = new HttpClient();

            var url =
                $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={apiKey}";

            Console.WriteLine($"URL: {url}");

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new
                            {
                                text = prompt
                            }
                        }
                    }
                }
            };

            var json =
                JsonSerializer.Serialize(requestBody);

            Console.WriteLine("Request Body:");
            Console.WriteLine(json);

            var response =
                await client.PostAsync(
                    url,
                    new StringContent(
                        json,
                        Encoding.UTF8,
                        "application/json"));

            var responseContent =
                await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Status Code: {(int)response.StatusCode}");
            Console.WriteLine($"Status: {response.StatusCode}");

            Console.WriteLine("Response Content:");
            Console.WriteLine(responseContent);

            Console.WriteLine("========== END DEBUG ==========");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"Gemini Error ({response.StatusCode}):\n{responseContent}");
            }

            using var document =
                JsonDocument.Parse(responseContent);

            return document
                       .RootElement
                       .GetProperty("candidates")[0]
                       .GetProperty("content")
                       .GetProperty("parts")[0]
                       .GetProperty("text")
                       .GetString()
                   ?? "No response";
        }
    }
}