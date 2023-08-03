using Domain.Entities;

namespace Identity.Models;

public class TokenStorage
{
    public int Id { get; set; }
    public string Token { get; set; } = null!;
    public DateTime Expiration { get; set; }
    public bool Active { get; set; }
    public User User { get; set; } = null!;
    public string UserId { get; set; } = null!;
}