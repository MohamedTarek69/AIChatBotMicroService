using AIChatBotMicroService.DTOs;

namespace AIChatBotMicroService.Services
{
    public interface IChatHistoryService
    {
            Task SaveChatAsync(
                Guid identityUserId,
                ChatMessageDto message);

            Task<List<ChatMessageDto>>
                GetChatHistoryAsync(
                    Guid identityUserId);

            Task ClearChatHistoryAsync(
                Guid identityUserId);
    }
}
