//using Microsoft.AspNetCore.Mvc;
//using Mscc.GenerativeAI;

//namespace AIChatBotMicroService.Controllers
//{
//    [ApiController]
//    [Route("api/gemini")]
//    public class GeminiDebugController : ControllerBase
//    {
//        private readonly IConfiguration _configuration;

//        public GeminiDebugController(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }

//        [HttpGet("models")]
//        public async Task<IActionResult> GetModels()
//        {
//            var apiKey = _configuration["Gemini:ApiKey"];

//            var googleAI = new GoogleAI(apiKey);

//            var models = await googleAI.ListModelsAsync();

//            var result = models.Select(m => new
//            {
//                m.Name
//            });

//            return Ok(result);
//        }
//    }
//}