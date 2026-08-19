using Microsoft.AspNetCore.Mvc;

namespace Maxgram.Controllers
{
    [ApiController]
    [Route("/api[controler]")]
    public class MessagessController : ControllerBase
    {
        public MessagessController()
        {

        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteMessage(int id)
        {
            return Ok();
        }
    }
}
