using AIChatBotMicroService.DTOs;
using System.Net.Http.Headers;

namespace AIChatBotMicroService.Clients
{
    public class PatientClient : IPatientClient
    {
        private readonly HttpClient _httpClient;

        public PatientClient(
            HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ReturnedPatientDetailsDto>
GetPatientDetailsAsync(
    Guid identityUserId,
    string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new UnauthorizedAccessException(
                    "Authorization token is missing");
            }

            var jwt = token.Replace("Bearer ", "");

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    jwt);

            var response =
                await _httpClient.GetAsync(
                    $"Patiant/DetailsByIdentityUserId/{identityUserId}");

            response.EnsureSuccessStatusCode();

            return await response.Content
                .ReadFromJsonAsync<ReturnedPatientDetailsDto>();
        }
    }
}
