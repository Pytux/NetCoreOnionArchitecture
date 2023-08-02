namespace Application.DTOs.User;

public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; } = null!;
    public DateTime Expires { get; set; }
    public bool IsExpired => DateTime.Now >= Expires;
    public DateTime Created { get; set; }
    public string CreatedByIp { get; set; } = null!;
    public DateTime? Revoked { get; set; }
    public string RevokedByIp { get; set; } = null!;
    public string ReplacedByToken { get; set; } = null!;
    public bool IsActive => Revoked == null && !IsExpired;
}