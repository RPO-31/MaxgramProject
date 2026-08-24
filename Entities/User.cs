namespace Maxgram.API.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = String.Empty;

        public string Name { get; set; } = String.Empty;

        public string Email { get; set; } = String.Empty;

        public HashCode Password { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
