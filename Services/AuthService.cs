using Maxgram.API.Models;
using Maxgram.API.Repositories.Interfaces;
using System.Net;

namespace Maxgram.API.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<RegistrationResult> RegisterAsync(string username, string name, string email, string password, string avPassword)
        {
            return new RegistrationResult();
        }

        public async Task<RegistrationResult> LoginAsync(string username, string password)
        {
            return new RegistrationResult();
        }
    }
}
