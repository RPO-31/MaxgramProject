namespace Maxgram.API.Models
{
    public class RegistrationResult
    {
        public bool IsSuccess { get; set; }
        public List<string>? Errors { get; set; }
    }
}
