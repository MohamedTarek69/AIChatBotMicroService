namespace AIChatBotMicroService.Providers
{
    using OpenAI.Chat;

    public class OpenAiProvider
        : IAIProvider
    {
        private readonly IConfiguration
            _configuration;

        public OpenAiProvider(
            IConfiguration configuration)
        {
            _configuration =
                configuration;
        }

        public async Task<string>
            AnalyzeAsync(string prompt)
        {
            var apiKey =
                _configuration
                ["OpenAI:ApiKey"];

            var client =
                new ChatClient(
                    "gpt-4o-mini",
                    apiKey);

            var response =
                await client
                    .CompleteChatAsync(
                        prompt);

            return response
                .Value
                .Content[0]
                .Text;
        }
    }
}
