namespace AIChatBotMicroService.DTOs
{
    public class ChatMessageDto
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Symptoms { get; set; } = string.Empty;

        public string AIResponse { get; set; } = string.Empty;

        public string Specialty { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
