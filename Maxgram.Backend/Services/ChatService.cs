using Microsoft.EntityFrameworkCore;
using Maxgram.Backend.Data;
using Maxgram.Backend.Entities;
using Maxgram.Backend.Dto;
using Maxgram.Backend.Common;
using Maxgram.Backend.Services.Interfaces;

namespace Maxgram.Backend.Services;

public class ChatService : IChatService
{
    private readonly MaxgramDbContext _db;
    public ChatService(MaxgramDbContext db) { _db = db; }

    public async Task<List<ConversationDto>> GetUserChatsAsync(int userId)
    {
        var conversationIds = await _db.ConversationParticipants
            .Where(p => p.UserId == userId)
            .Select(p => p.ConversationId)
            .ToListAsync();

        var conversations = await _db.Conversations
            .Where(c => conversationIds.Contains(c.Id))
            .Include(c => c.Participants).ThenInclude(p => p.User)
            .Include(c => c.Messages)
            .ToListAsync();

        var result = new List<ConversationDto>();
        foreach (var c in conversations)
        {
            var lastMessage = c.Messages.OrderByDescending(m => m.SentAt).FirstOrDefault();
            string title = c.Type == ConversationType.Group
                ? (c.Title ?? "")
                : (c.Participants.FirstOrDefault(p => p.UserId != userId)?.User?.DisplayName ?? "");

            result.Add(new ConversationDto(
                c.Id,
                c.Type.ToString(),
                title,
                lastMessage != null && !lastMessage.IsDeleted ? lastMessage.Text : null,
                lastMessage?.Author?.DisplayName,
                lastMessage?.SentAt
            ));
        }

        return result.OrderByDescending(c => c.LastMessageAt ?? DateTime.MinValue).ToList();
    }

    public async Task<ServiceResult<ConversationDetailsDto>> CreatePrivateAsync(int userId, int targetUserId)
    {
        if (userId == targetUserId)
            return ServiceResult<ConversationDetailsDto>.Fail(ErrorCode.BadRequest, "Нельзя создать чат с самим собой");

        var targetExists = await _db.Users.AnyAsync(u => u.Id == targetUserId);
        if (!targetExists)
            return ServiceResult<ConversationDetailsDto>.Fail(ErrorCode.NotFound, "Пользователь не найден");

        var existing = await _db.Conversations
            .Where(c => c.Type == ConversationType.Private)
            .Where(c => c.Participants.Any(p => p.UserId == userId))
            .Where(c => c.Participants.Any(p => p.UserId == targetUserId))
            .FirstOrDefaultAsync();

        if (existing != null)
            return await GetDetailsAsync(userId, existing.Id);

        var conversation = new Conversation
        {
            Type = ConversationType.Private,
            CreatedByUserId = userId,
            Participants = new List<ConversationParticipant>
            {
                new() { UserId = userId },
                new() { UserId = targetUserId }
            }
        };
        _db.Conversations.Add(conversation);
        await _db.SaveChangesAsync();

        return await GetDetailsAsync(userId, conversation.Id);
    }

    public async Task<ServiceResult<ConversationDetailsDto>> CreateGroupAsync(int userId, string title, List<int> participantIds)
    {
        if (string.IsNullOrWhiteSpace(title))
            return ServiceResult<ConversationDetailsDto>.Fail(ErrorCode.BadRequest, "Название обязательно");

        var ids = participantIds.Distinct().Where(id => id != userId).ToList();
        ids.Add(userId);

        var existingCount = await _db.Users.CountAsync(u => ids.Contains(u.Id));
        if (existingCount != ids.Count)
            return ServiceResult<ConversationDetailsDto>.Fail(ErrorCode.BadRequest, "Один или несколько участников не найдены");

        if (ids.Count < 2)
            return ServiceResult<ConversationDetailsDto>.Fail(ErrorCode.BadRequest, "Нужно минимум 2 участника");

        var conversation = new Conversation
        {
            Type = ConversationType.Group,
            Title = title,
            CreatedByUserId = userId,
            Participants = ids.Select(id => new ConversationParticipant { UserId = id }).ToList()
        };
        _db.Conversations.Add(conversation);
        await _db.SaveChangesAsync();

        return await GetDetailsAsync(userId, conversation.Id);
    }

    public async Task<ServiceResult<ConversationDetailsDto>> GetDetailsAsync(int userId, int chatId)
    {
        var conversation = await _db.Conversations
            .Include(c => c.Participants).ThenInclude(p => p.User)
            .FirstOrDefaultAsync(c => c.Id == chatId);

        if (conversation == null)
            return ServiceResult<ConversationDetailsDto>.Fail(ErrorCode.NotFound, "Чат не найден");

        var isParticipant = conversation.Participants.Any(p => p.UserId == userId);
        if (!isParticipant)
            return ServiceResult<ConversationDetailsDto>.Fail(ErrorCode.Forbidden, "Нет доступа к этому чату");

        var participants = conversation.Participants
            .Select(p => new UserDto(p.User!.Id, p.User.Username, p.User.DisplayName, p.User.Email))
            .ToList();

        string title = conversation.Type == ConversationType.Group
            ? (conversation.Title ?? "")
            : (participants.FirstOrDefault(p => p.Id != userId)?.DisplayName ?? "");

        return ServiceResult<ConversationDetailsDto>.Ok(
            new ConversationDetailsDto(conversation.Id, conversation.Type.ToString(), title, participants));
    }

    public async Task<bool> IsParticipantAsync(int userId, int chatId) =>
        await _db.ConversationParticipants.AnyAsync(p => p.UserId == userId && p.ConversationId == chatId);

    public async Task<bool> ChatExistsAsync(int chatId) =>
        await _db.Conversations.AnyAsync(c => c.Id == chatId);
}
