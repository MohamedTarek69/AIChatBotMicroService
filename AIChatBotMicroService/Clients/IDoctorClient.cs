using AIChatBotMicroService.DTOs.AIChatBotMicroService.DTOs;

namespace AIChatBotMicroService.Clients
{
    public interface IDoctorClient
    {
        Task<List<DoctorDto>> GetBySpecialtyAsync(string specialty, string token);
    }
}
