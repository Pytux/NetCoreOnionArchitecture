namespace Application.DTOs.User;

public class AuthenticationRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}