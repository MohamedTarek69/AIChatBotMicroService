using AIChatBotMicroService.DTOs;
using AIChatBotMicroService.DTOs.AIChatBotMicroService.DTOs;
using System.Net.Http.Headers;

namespace AIChatBotMicroService.Clients
{
    public class DoctorClient : IDoctorClient
    {
        private readonly HttpClient _httpClient;

        public DoctorClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<DoctorDto>> GetBySpecialtyAsync(
            string specialty,
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

            var url =
                $"doctors/BySpecialty/{Uri.EscapeDataString(specialty)}";

            Console.WriteLine($"Calling: {_httpClient.BaseAddress}{url}");
            Console.WriteLine($"Specialty = {specialty}");

            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            return await response.Content
                       .ReadFromJsonAsync<List<DoctorDto>>()
                   ?? new List<DoctorDto>();
        }
    }
}