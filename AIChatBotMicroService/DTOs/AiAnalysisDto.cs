namespace AIChatBotMicroService.DTOs
{
    public class AiAnalysisDto
    {
        public string PreliminaryAnalysis { get; set; } = string.Empty;

        public string Specialty { get; set; } = string.Empty;

        public string RiskLevel { get; set; } = string.Empty;

        public List<string> Tips { get; set; } = [];
    }
}
