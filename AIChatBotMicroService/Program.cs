using AIChatBotMicroService.Clients;
using AIChatBotMicroService.Providers;
using AIChatBotMicroService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using StackExchange.Redis;

namespace AIChatBotMicroService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region 🔹 Controllers
            builder.Services.AddControllers();
            #endregion

            #region 🔹 HttpContext
            builder.Services.AddHttpContextAccessor();
            #endregion

            #region 🔹 App Services (Gemini AI)
            //builder.Services.AddScoped<IAIProvider, GeminiProvider>();
            builder.Services.AddScoped<ChatbotService>();
            //builder.Services.AddScoped<IAIProvider, OpenRouterProvider>();
            builder.Services.AddScoped<IAIProvider, GroqProvider>();

            builder.Services.AddSingleton<IConnectionMultiplexer>(
               ConnectionMultiplexer.Connect(
                   $"{builder.Configuration["Redis:ConnectionString"]},abortConnect=false"));

            builder.Services.AddScoped<
                IChatHistoryService,
                ChatHistoryService>();
            

            #endregion

            #region 🔹 HTTP Clients
            builder.Services.AddHttpClient<IPatientClient, PatientClient>(client =>
            {
                client.BaseAddress =
                    new Uri(builder.Configuration["PatientService:BaseUrl"]!);
            });
            builder.Services.AddHttpClient<IDoctorClient, DoctorClient>(client =>
            {
                client.BaseAddress =
                    new Uri(builder.Configuration["DoctorService:BaseUrl"]!);
            });
            #endregion

            #region 🔹 JWT Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = "http://host.docker.internal:8080",
                    ValidAudience = "http://host.docker.internal:4200",

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["JWTOptions:SecretKey"]!)),

                    RoleClaimType = ClaimTypes.Role,
                    NameClaimType = ClaimTypes.NameIdentifier,
                    ClockSkew = TimeSpan.Zero
                };
            });
            #endregion

            #region 🔹 Authorization Policies
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", p => p.RequireRole("Admin"));
                options.AddPolicy("DoctorOnly", p => p.RequireRole("Doctor"));
                options.AddPolicy("PatientOnly", p => p.RequireRole("Patient"));

                options.AddPolicy("AdminOrDoctor",
                    p => p.RequireRole("Admin", "Doctor"));

                options.AddPolicy("AdminOrPatient",
                    p => p.RequireRole("Admin", "Patient"));
            });
            #endregion

            #region 🔹 CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });
            #endregion

            #region 🔹 Swagger
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(7126);
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            #endregion

            Console.WriteLine($"Groq Key: {builder.Configuration["Groq:ApiKey"]?.Substring(0, 10)}...");

            var app = builder.Build();

            #region 🔹 Middleware Pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            #endregion

            await app.RunAsync();
        }
    }
}