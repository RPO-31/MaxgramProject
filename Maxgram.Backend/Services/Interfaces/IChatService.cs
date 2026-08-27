using Maxgram.Backend.Common;
using Maxgram.Backend.Dto;

namespace Maxgram.Backend.Services.Interfaces
{
    public interface IChatService
    {
        public Task<List<ConversationDto>> GetUserChatsAsync(int userId);

        public Task<ServiceResult<ConversationDetailsDto>> CreatePrivateAsync(int userId, int targetUserId);

        public Task<ServiceResult<ConversationDetailsDto>> CreateGroupAsync(int userId, string title, List<int> participantIds);

        public Task<ServiceResult<ConversationDetailsDto>> GetDetailsAsync(int userId, int chatId);

        public Task<bool> IsParticipantAsync(int userId, int chatId);

        public Task<bool> ChatExistsAsync(int chatId);
    }
}
