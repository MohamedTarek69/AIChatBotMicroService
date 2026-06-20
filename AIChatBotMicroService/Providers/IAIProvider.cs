namespace AIChatBotMicroService.Providers
{
    public interface IAIProvider
    {
        Task<string> AnalyzeAsync(
            string prompt);
    }
}
