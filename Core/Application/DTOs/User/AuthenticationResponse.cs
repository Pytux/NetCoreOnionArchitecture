using System.Text.Json.Serialization;

namespace Application.DTOs.User;

public class AuthenticationResponse
{
    public string Id { get; set; } = null!;
    public string? UserName { get; set; } = null!;
    public string? Email { get; set; } = null!;
    public List<string> Roles { get; set; } = null!;
    public string JwToken { get; set; } = null!;

    [JsonIgnore] public string RefreshToken { get; set; } = null!;
}