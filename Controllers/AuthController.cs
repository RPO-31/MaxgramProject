using Maxgram.API.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Maxgram.Controllers
{
    [ApiController]
    [Route("/api[controler]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(string username, string name, string email, string password, string avPassword)
        {
            var result = await _authService.RegisterAsync(username, name, email, password, avPassword);

            if (result.IsSuccess)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(string username, string password)
        {
            var result = await _authService.LoginAsync(username, password);

            if (result.IsSuccess)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            return Ok();
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            return Ok();
        }
    }
}
