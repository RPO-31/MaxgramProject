using Microsoft.EntityFrameworkCore;
using Maxgram.Backend.Data;
using Maxgram.Backend.Entities;
using Maxgram.Backend.Dto;
using Maxgram.Backend.Common;
using Maxgram.Backend.Services.Interfaces;

namespace Maxgram.Backend.Services;

public class MessageService : IMessageService
{
    private readonly MaxgramDbContext _db;
    private readonly IChatService _chatService;

    public MessageService(MaxgramDbContext db, IChatService chatService)
    {
        _db = db;
        _chatService = chatService;
    }

    public async Task<ServiceResult<PagedResult<MessageDto>>> GetMessagesAsync(int userId, int chatId, int page, int pageSize)
    {
        if (!await _chatService.ChatExistsAsync(chatId))
            return ServiceResult<PagedResult<MessageDto>>.Fail(ErrorCode.NotFound, "Чат не найден");

        if (!await _chatService.IsParticipantAsync(userId, chatId))
            return ServiceResult<PagedResult<MessageDto>>.Fail(ErrorCode.Forbidden, "Нет доступа к этому чату");

        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var query = _db.Messages
            .Include(m => m.Author)
            .Where(m => m.ConversationId == chatId)
            .OrderByDescending(m => m.SentAt);

        var total = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(m => new MessageDto(
                m.Id, m.ConversationId, m.AuthorId,
                m.Author!.DisplayName,
                m.IsDeleted ? null : m.Text,
                m.SentAt, m.IsDeleted))
            .ToListAsync();

        return ServiceResult<PagedResult<MessageDto>>.Ok(new PagedResult<MessageDto>(items, page, pageSize, total));
    }

    public async Task<ServiceResult<MessageDto>> SendMessageAsync(int userId, int chatId, string text)
    {
        if (!await _chatService.ChatExistsAsync(chatId))
            return ServiceResult<MessageDto>.Fail(ErrorCode.NotFound, "Чат не найден");

        if (!await _chatService.IsParticipantAsync(userId, chatId))
            return ServiceResult<MessageDto>.Fail(ErrorCode.Forbidden, "Нет доступа к этому чату");

        if (string.IsNullOrWhiteSpace(text))
            return ServiceResult<MessageDto>.Fail(ErrorCode.BadRequest, "Текст сообщения пуст");

        var message = new Message
        {
            ConversationId = chatId,
            AuthorId = userId,
            Text = text
        };
        _db.Messages.Add(message);
        await _db.SaveChangesAsync();

        var author = await _db.Users.FindAsync(userId);

        return ServiceResult<MessageDto>.Ok(new MessageDto(
            message.Id, message.ConversationId, message.AuthorId,
            author!.DisplayName, message.Text, message.SentAt, false));
    }

    public async Task<ServiceResult<bool>> DeleteMessageAsync(int userId, int messageId)
    {
        var message = await _db.Messages.FindAsync(messageId);
        if (message == null)
            return ServiceResult<bool>.Fail(ErrorCode.NotFound, "Сообщение не найдено");

        if (message.AuthorId != userId)
            return ServiceResult<bool>.Fail(ErrorCode.Forbidden, "Нельзя удалить чужое сообщение");

        message.IsDeleted = true;
        message.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return ServiceResult<bool>.Ok(true);
    }
}
