using Microsoft.AspNetCore.Mvc;

namespace Maxgram.Controllers
{
    [ApiController]
    [Route("/api[controler]")]
    public class AuthController : ControllerBase
    {
        public AuthController()
        {

        }

        [HttpPost("register")]
        public IActionResult Register()
        {
            return Ok();
        }

        [HttpPost("login")]
        public IActionResult Login()
        {
            return Ok();
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok();
        }

        [HttpGet("me")]
        public IActionResult GetMe()
        {
            return Ok();
        }
    }
}
