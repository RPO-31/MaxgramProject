using System.ComponentModel.DataAnnotations;

namespace Maxgram.Backend.Dto;

public class CreatePrivateChatRequest
{
    [Required]
    public int UserId { get; set; }
}

public class CreateGroupChatRequest
{
    [Required, MaxLength(100)]
    public string Title { get; set; } = "";
    [Required]
    public List<int> ParticipantIds { get; set; } = new();
}

public record ConversationDto(int Id, string Type, string Title, string? LastMessage, string? LastMessageAuthor, DateTime? LastMessageAt);
public record ConversationDetailsDto(int Id, string Type, string Title, List<UserDto> Participants);
