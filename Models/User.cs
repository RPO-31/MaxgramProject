namespace Maxgram.API.Models
{
    public class User
    {
        int Id { get; set; }

        string Username { get; set; } = String.Empty;

        string Name { get; set; } = String.Empty;

        string Email { get; set; } = String.Empty;

        HashCode Password { get; set; }

        DateTime CreatedAt { get; set; }
    }
}
