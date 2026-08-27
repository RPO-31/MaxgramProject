using Maxgram.Backend.Common;
using Maxgram.Backend.Dto;
using Maxgram.Backend.Entities;

namespace Maxgram.Backend.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<ServiceResult<UserDto>> RegisterAsync(RegisterRequest request);

        public Task<User?> ValidateLoginAsync(LoginRequest request);

        public Task<UserDto?> GetByIdAsync(int id);
    }
}
