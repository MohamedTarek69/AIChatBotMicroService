using AIChatBotMicroService.DTOs;

namespace AIChatBotMicroService.Clients
{
    public interface IPatientClient
    {
        Task<ReturnedPatientDetailsDto>GetPatientDetailsAsync(Guid identityUserId,string token);

    }
}
