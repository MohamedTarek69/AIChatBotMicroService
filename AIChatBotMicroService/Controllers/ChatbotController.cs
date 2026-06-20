using AIChatBotMicroService.DTOs;
using AIChatBotMicroService.Providers;
using AIChatBotMicroService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AIChatBotMicroService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/chatbot")]
    public class ChatbotController
    : ControllerBase
    {
        private readonly ChatbotService _service;
        private readonly IAIProvider _aiProvider;
        private readonly IConfiguration _configuration;
        private readonly IChatHistoryService _chatHistoryService;

        public ChatbotController(
            ChatbotService service,
            IAIProvider aIProvider,
            IConfiguration configuration,
            IChatHistoryService chatHistoryService)
        {
            _service = service;
            _aiProvider = aIProvider;
            _configuration = configuration;
            _chatHistoryService = chatHistoryService;
        }

        [HttpPost("analyze")]
        public async Task<IActionResult>
            Analyze(
                AnalyzeSymptomsRequest request)
        {
            var result =
                await _service
                    .AnalyzeSymptomsAsync(
                        request);

            return Ok(result);
        }

        [HttpGet("test-groq")]
        public async Task<IActionResult> TestGroq()
        {
            var result = await _aiProvider.AnalyzeAsync("Hello");

            return Ok(result);
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var userId =
                User.FindFirst(
                    ClaimTypes.NameIdentifier)?
                    .Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var history =
                await _chatHistoryService
                    .GetChatHistoryAsync(
                        Guid.Parse(userId));

            return Ok(history);
        }
    }
}
