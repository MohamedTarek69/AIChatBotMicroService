namespace AIChatBotMicroService.DTOs
{
    public class MedicalRecordDto
    {
        public int Id { get; set; }

        public string Diagnosis { get; set; }
            = null!;

        public string Notes { get; set; }
            = null!;
    }
}
