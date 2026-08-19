namespace Maxgram.API.Models
{
    public class Chatter
    {
        public int Id { get; set; }

        public ChatType Type { get; set; }

        public string? Name { get; set; } = String.Empty;

        public DateTime CreatedAt { get; set; }

        public User? Creator { get; set; }
    }
}
