using Maxgram.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Maxgram.API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<List<User>> GetUsersAsync();

        public Task<User> GetUserByIdAsync(int id);

        public Task<User> AddUserAsync(User user);
    }
}
