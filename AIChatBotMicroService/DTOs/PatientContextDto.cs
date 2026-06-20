namespace AIChatBotMicroService.DTOs
{
    public class PatientContextDto
    {
        public List<string> MedicalHistory { get; set; }
            = new();

        public List<string> Allergies { get; set; }
            = new();

        public int Age { get; set; }

        public string Gender { get; set; } = null!;
    }
}
