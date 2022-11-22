namespace Application.DTOs.User;

public class RegisterRequest
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}