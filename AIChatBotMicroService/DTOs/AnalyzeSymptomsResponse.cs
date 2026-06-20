using AIChatBotMicroService.DTOs.AIChatBotMicroService.DTOs;

namespace AIChatBotMicroService.DTOs
{
    public class AnalyzeSymptomsResponse
    {
        public PatientInfoDto Patient { get; set; } = null!;

        public AiAnalysisDto Analysis { get; set; } = null!;

        public List<DoctorDto> RecommendedDoctors { get; set; } = [];
    }
}
