using Maxgram.Backend.Dto;

namespace Maxgram.Backend.Services.Interfaces
{
    public interface IUserService
    {
        public Task<List<UserSearchDto>> SearchAsync(int currentUserId, string? search);
    }
}
