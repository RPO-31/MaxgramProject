namespace Maxgram.Backend.Entities;

public class Conversation
{
    public int Id { get; set; }
    public ConversationType Type { get; set; }
    public string? Title { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int CreatedByUserId { get; set; }

    public List<ConversationParticipant> Participants { get; set; } = new();
    public List<Message> Messages { get; set; } = new();
}
