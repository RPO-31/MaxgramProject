using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Maxgram.Backend.Dto;
using Maxgram.Backend.Common;
using Maxgram.Backend.Services.Interfaces;

namespace Maxgram.Backend.Controllers;

[ApiController]
[Route("api/chats")]
[Authorize]
public class ChatsController : ControllerBase
{
    private readonly IChatService _chatService;
    private readonly IMessageService _messageService;

    public ChatsController(IChatService chatService, IMessageService messageService)
    {
        _chatService = chatService;
        _messageService = messageService;
    }

    private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _chatService.GetUserChatsAsync(CurrentUserId);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _chatService.GetDetailsAsync(CurrentUserId, id);
        return result.ToActionResult().Result!;
    }

    [HttpPost("private")]
    public async Task<IActionResult> CreatePrivate(CreatePrivateChatRequest request)
    {
        var result = await _chatService.CreatePrivateAsync(CurrentUserId, request.UserId);
        return result.ToActionResult().Result!;
    }

    [HttpPost("group")]
    public async Task<IActionResult> CreateGroup(CreateGroupChatRequest request)
    {
        var result = await _chatService.CreateGroupAsync(CurrentUserId, request.Title, request.ParticipantIds);
        return result.ToActionResult().Result!;
    }

    [HttpGet("{chatId}/messages")]
    public async Task<IActionResult> GetMessages(int chatId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _messageService.GetMessagesAsync(CurrentUserId, chatId, page, pageSize);
        return result.ToActionResult().Result!;
    }

    [HttpPost("{chatId}/messages")]
    public async Task<IActionResult> SendMessage(int chatId, SendMessageRequest request)
    {
        var result = await _messageService.SendMessageAsync(CurrentUserId, chatId, request.Text);
        return result.ToActionResult().Result!;
    }
}
