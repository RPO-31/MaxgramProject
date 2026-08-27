using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Maxgram.Backend.Services;
using Maxgram.Backend.Common;

namespace Maxgram.Backend.Controllers;

[ApiController]
[Route("api/messages")]
[Authorize]
public class MessagesController : ControllerBase
{
    private readonly MessageService _messageService;
    public MessagesController(MessageService messageService) { _messageService = messageService; }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _messageService.DeleteMessageAsync(userId, id);
        return result.ToActionResult().Result!;
    }
}
