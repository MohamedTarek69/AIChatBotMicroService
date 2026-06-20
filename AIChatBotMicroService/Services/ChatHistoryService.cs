using AIChatBotMicroService.DTOs;
using StackExchange.Redis;
using System.Text.Json;

namespace AIChatBotMicroService.Services
{
    public class ChatHistoryService
        : IChatHistoryService
    {
        private readonly IDatabase _database;

        public ChatHistoryService(
            IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task SaveChatAsync(
            Guid identityUserId,
            ChatMessageDto message)
        {
            var key =
                $"chat:{identityUserId}";

            await _database.ListRightPushAsync(
                key,
                JsonSerializer.Serialize(message));

            await _database.KeyExpireAsync(
                key,
                TimeSpan.FromDays(7));
        }

        public async Task<List<ChatMessageDto>>
            GetChatHistoryAsync(
                Guid identityUserId)
        {
            var key =
                $"chat:{identityUserId}";

            var values =
                await _database.ListRangeAsync(key);

            Console.WriteLine($"HISTORY USER = {identityUserId}");
            return values
                .Select(x =>
                    JsonSerializer.Deserialize<ChatMessageDto>(x!)
                )
                .Where(x => x != null)
                .ToList()!;
        }

        public async Task ClearChatHistoryAsync(
            Guid identityUserId)
        {
            await _database.KeyDeleteAsync(
                $"chat:{identityUserId}");
        }
    }
}