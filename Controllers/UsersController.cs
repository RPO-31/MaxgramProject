using Microsoft.AspNetCore.Mvc;

namespace Maxgram.Controllers
{
    [ApiController]
    [Route("/api[controler]")]
    public class UsersController : ControllerBase
    {
        public UsersController()
        {

        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok();
        }
    }
}
