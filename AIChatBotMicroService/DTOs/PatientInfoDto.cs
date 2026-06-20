namespace AIChatBotMicroService.DTOs
{
    public class PatientInfoDto
    {
        public int Age { get; set; }

        public string Gender { get; set; } = string.Empty;

        public List<string> MedicalHistory { get; set; } = [];

        public List<string> Allergies { get; set; } = [];

        public string Symptoms { get; set; } = string.Empty;
    }
}
