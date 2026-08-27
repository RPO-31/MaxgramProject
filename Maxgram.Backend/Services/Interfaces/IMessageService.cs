using Maxgram.Backend.Common;
using Maxgram.Backend.Dto;
using Maxgram.Backend.Entities;

namespace Maxgram.Backend.Services.Interfaces
{
    public interface IMessageService
    {
        public Task<ServiceResult<PagedResult<MessageDto>>> GetMessagesAsync(int userId, int chatId, int page, int pageSize);

        public Task<ServiceResult<MessageDto>> SendMessageAsync(int userId, int chatId, string text);

        public Task<ServiceResult<bool>> DeleteMessageAsync(int userId, int messageId);
    }
}
