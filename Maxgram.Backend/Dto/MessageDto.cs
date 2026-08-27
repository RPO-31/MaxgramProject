using System.ComponentModel.DataAnnotations;

namespace Maxgram.Backend.Dto;

public class SendMessageRequest
{
    [Required, MaxLength(4000)]
    public string Text { get; set; } = "";
}

public record MessageDto(int Id, int ConversationId, int AuthorId, string AuthorDisplayName, string? Text, DateTime SentAt, bool IsDeleted);
public record PagedResult<T>(List<T> Items, int Page, int PageSize, int TotalCount);
