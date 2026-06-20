namespace AIChatBotMicroService.DTOs
{
    namespace AIChatBotMicroService.DTOs
    {
        public class DoctorDto
        {
            public Guid Id { get; set; }

            public string Name { get; set; } = string.Empty;

            public string Specialty { get; set; } = string.Empty;

            public List<string> Clinics { get; set; } = new();
        }
    }
}
