using Maxgram.API.Models;
using System.Net;

namespace Maxgram.API.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<RegistrationResult> RegisterAsync(string username, string name, string email, string password, string avPassword);

        public Task<RegistrationResult> LoginAsync(string username, string password);
    }
}
