using System.ComponentModel.DataAnnotations;

namespace Maxgram.Backend.Dto;

public class RegisterRequest
{
    [Required, MinLength(3), MaxLength(30)]
    public string Username { get; set; } = "";
    [Required, MaxLength(50)]
    public string DisplayName { get; set; } = "";
    [Required, EmailAddress]
    public string Email { get; set; } = "";
    [Required, MinLength(6)]
    public string Password { get; set; } = "";
    [Required]
    public string ConfirmPassword { get; set; } = "";
}

public class LoginRequest
{
    [Required]
    public string UsernameOrEmail { get; set; } = "";
    [Required]
    public string Password { get; set; } = "";
}
