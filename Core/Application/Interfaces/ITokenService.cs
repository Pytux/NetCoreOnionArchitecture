using Domain.Entities;

namespace Application.Interfaces;

public interface ITokenService
{
    Task<bool> IsCurrentActiveToken();
    Task DeactivateCurrentAsync();
    Task<bool> IsActiveAsync(string token);
    Task DeactivateAsync(string token);

    Task SaveTokenAsync(string token, User user);
}