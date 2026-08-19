using Microsoft.AspNetCore.Mvc;

namespace Maxgram.Controllers
{
    [ApiController]
    [Route("/api[controler]")]
    public class ChatsController : ControllerBase
    {
        public ChatsController()
        {

        }

        [HttpGet]
        public IActionResult GetChats()
        {
            return Ok();
        }

        [HttpGet("{id:int}")]
        public IActionResult GetInfoChat(int id)
        {
            return Ok();
        }

        [HttpPost("private")]
        public IActionResult CreateChat()
        {
            return Ok();
        }

        [HttpPost("group")]
        public IActionResult CreateGroup()
        {
            return Ok();
        }

        [HttpGet("{id:int}/messages")]
        public IActionResult GetHistoryMessages(int id)
        {
            return Ok();
        }

        [HttpPost("{id:int}/messages")]
        public IActionResult SendMessage()
        {
            return Ok();
        }
    }
}
