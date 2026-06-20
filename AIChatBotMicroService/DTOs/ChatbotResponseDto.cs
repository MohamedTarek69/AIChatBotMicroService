using AIChatBotMicroService.DTOs.AIChatBotMicroService.DTOs;

namespace AIChatBotMicroService.DTOs
{
    public class ChatbotResponseDto
    {
        public PatientInfoDto Patient { get; set; } = default!;

        public string PreliminaryAnalysis { get; set; } = string.Empty;

        public string Specialty { get; set; } = string.Empty;

        public string RiskLevel { get; set; } = string.Empty;

        public List<string> Tips { get; set; } = [];

        public List<DoctorDto> RecommendedDoctors { get; set; } = [];
    }
}
