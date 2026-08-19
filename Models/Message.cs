namespace Maxgram.API.Models
{
    public class Message
    {
        public int Id { get; set; }

        public Chat Chat { get; set; }

        public User Author { get; set; }

        public string Text { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public string InfoAboutDelete { get; set; } = string.Empty;
    }
}
