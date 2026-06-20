using AIChatBotMicroService.Clients;
using AIChatBotMicroService.DTOs;
using AIChatBotMicroService.DTOs.AIChatBotMicroService.DTOs;
using AIChatBotMicroService.Providers;
using System.Security.Claims;
using System.Text;
using static Google.Apis.Requests.BatchRequest;

namespace AIChatBotMicroService.Services
{
    public class ChatbotService
    {
        private readonly IPatientClient _patientClient;
        private readonly IDoctorClient _doctorClient;
        private readonly IAIProvider _aiProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IChatHistoryService _chatHistoryService;

        public ChatbotService(
            IPatientClient patientClient,
            IDoctorClient doctorClient,
            IAIProvider aiProvider,
            IHttpContextAccessor httpContextAccessor,
            IChatHistoryService chatHistoryService)
        {
            _patientClient = patientClient;
            _doctorClient = doctorClient;
            _aiProvider = aiProvider;
            _httpContextAccessor = httpContextAccessor;
            _chatHistoryService = chatHistoryService;
        }

        public async Task<string> AnalyzeSymptomsAsync(
            AnalyzeSymptomsRequest request)
        {
            var token =
                _httpContextAccessor.HttpContext?
                .Request.Headers["Authorization"]
                .ToString();

            var userId =
                _httpContextAccessor.HttpContext?
                .User
                .FindFirst(ClaimTypes.NameIdentifier)?
                .Value;

            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("Invalid Token");

            var identityUserId = Guid.Parse(userId);

            var patient =
                await _patientClient.GetPatientDetailsAsync(
                    identityUserId,
                    token);

            var age =
                DateTime.Today.Year -
                patient.DateOfBirth.Year;

            if (patient.DateOfBirth >
                DateTime.Today.AddYears(-age))
            {
                age--;
            }

            var medicalHistory =
                patient.MedicalRecords
                .Select(x => x.Diagnosis)
                .ToList();

            var allergies =
                patient.Allergies
                .Select(x => x.Name)
                .ToList();

            var prompt = $@"
أنت مساعد طبي ذكي متخصص في الفرز الطبي (Medical Triage Assistant).

{Specialties.Prompt}

التزم بالقواعد التالية:

- لا تعط تشخيصاً نهائياً أبداً.
- قدم تحليلاً مبدئياً فقط.
- اختر تخصصاً واحداً فقط من القائمة السابقة.
- لا تكتب أي تخصص غير الموجود بالقائمة.
- اكتب اسم التخصص باللغة الإنجليزية فقط كما هو موجود.
- إذا كانت الحالة خطيرة اطلب التوجه للطوارئ.
- راع التاريخ المرضي والحساسيات.
- أجب بالعربية.

يجب أن يكون الرد بالشكل التالي فقط وبدون أي إضافات:

🩺 التحليل المبدئي:
(فقرة قصيرة)

👨‍⚕️ التخصص:
(اسم تخصص واحد فقط)

🚨 مستوى الخطورة:
(منخفضة أو متوسطة أو مرتفعة)

💡 النصائح:
- نصيحة 1
- نصيحة 2
- نصيحة 3

==================

Patient Information

Age: {age}

Gender: {patient.Gender}

Medical History:
{(medicalHistory.Any()
    ? string.Join(", ", medicalHistory)
    : "None")}

Allergies:
{(allergies.Any()
    ? string.Join(", ", allergies)
    : "None")}

Symptoms:
{request.Symptoms}
";

            var aiResponse =
                await _aiProvider.AnalyzeAsync(prompt);

            Console.WriteLine("========== AI RESPONSE ==========");
            Console.WriteLine(aiResponse);
            Console.WriteLine("=================================");

            string specialty = "";

            var lines =
                aiResponse
                .Split('\n', StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < lines.Length; i++)
            {
                var current =
                    lines[i]
                    .Replace("*", "")
                    .Replace("`", "")
                    .Replace("#", "")
                    .Trim();

                if (current.Contains("التخصص"))
                {
                    // لو التخصص موجود بعد :
                    var index = current.IndexOf(':');

                    if (index != -1)
                    {
                        specialty =
                            current[(index + 1)..]
                            .Trim();
                    }

                    // لو السطر فاضي يبقى خده من السطر اللي بعده
                    if (string.IsNullOrWhiteSpace(specialty))
                    {
                        if (i + 1 < lines.Length)
                        {
                            specialty =
                                lines[i + 1]
                                .Replace("*", "")
                                .Replace("`", "")
                                .Replace("#", "")
                                .Trim();
                        }
                    }

                    break;
                }
            }

            // تنظيف إضافي
            specialty = specialty
                .Replace("👨‍⚕️", "")
                .Replace(":", "")
                .Trim();

            Console.WriteLine($"Detected Specialty = [{specialty}]");

            Console.WriteLine($"Detected Specialty = [{specialty}]");

            List<DoctorDto> doctors =
                new();

            if (!string.IsNullOrWhiteSpace(specialty))
            {
                try
                {
                    doctors =
                        await _doctorClient
                        .GetBySpecialtyAsync(
                            specialty,
                            token);

                    Console.WriteLine(
                        $"Doctors Count = {doctors.Count}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        $"Doctor Service Error: {ex.Message}");
                }
            }

            var result = new StringBuilder();

            result.AppendLine("👤 بيانات المريض");
            result.AppendLine();

            result.AppendLine($"Age: {age}");
            result.AppendLine($"Gender: {patient.Gender}");

            result.AppendLine(
                $"Medical History: {(medicalHistory.Any()
                    ? string.Join(", ", medicalHistory)
                    : "None")}");

            result.AppendLine(
                $"Allergies: {(allergies.Any()
                    ? string.Join(", ", allergies)
                    : "None")}");

            result.AppendLine($"Symptoms: {request.Symptoms}");

            result.AppendLine();
            result.AppendLine("================================");
            result.AppendLine();

            result.AppendLine(aiResponse);

            result.AppendLine();
            result.AppendLine("================================");
            result.AppendLine("👨‍⚕️ الأطباء المقترحون لدينا");
            result.AppendLine();

            if (doctors.Any())
            {
                foreach (var doctor in doctors)
                {
                    result.AppendLine($"🩺 {doctor.Name}");
                    result.AppendLine($"التخصص: {doctor.Specialty}");

                    if (doctor.Clinics != null &&
                        doctor.Clinics.Any())
                    {
                        result.AppendLine("العيادات:");

                        foreach (var clinic in doctor.Clinics)
                        {
                            result.AppendLine($"- {clinic}");
                        }
                    }

                    result.AppendLine("--------------------------------");
                }
            }
            else
            {
                result.AppendLine("لا يوجد طبيب بهذا التخصص حالياً.");
            }

            Console.WriteLine($"ANALYZE USER = {identityUserId}");

            await _chatHistoryService.SaveChatAsync(
                                      identityUserId,
                                      new ChatMessageDto
                                      {
                                          Id = Guid.NewGuid(),
                                          UserId = identityUserId,
                                          Symptoms = request.Symptoms,
                                          AIResponse = result.ToString(),
                                          Specialty = specialty,
                                          CreatedAt = DateTime.UtcNow
                                      });

            return result.ToString();
        }
    }
}