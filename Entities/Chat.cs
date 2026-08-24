namespace Maxgram.API.Entities
{
    public enum ChatType
    {
        Personal,
        Group
    };

    public class Chat
    {
        public int Id { get; set; }

        public ChatType Type { get; set; }

        public string? Name { get; set; } = String.Empty;

        public DateTime CreatedAt { get; set; }

        public User? Creator { get; set; }
    }
}
