using Application.Interfaces;
using Domain.Entities;
using Domain.Settings;
using Identity.Contexts;
using Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Identity.Services;

public class TokenService : ITokenService
{
    private readonly IdentityContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IOptions<JwtSettings> _jwtOptions;

    public TokenService(IdentityContext context,
        IHttpContextAccessor httpContextAccessor,
        IOptions<JwtSettings> jwtOptions
    )
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _jwtOptions = jwtOptions;
    }

    public async Task<bool> IsCurrentActiveToken()
    {
        return await IsActiveAsync(GetCurrentAsync());
    }

    public async Task DeactivateCurrentAsync()
    {
        await DeactivateAsync(GetCurrentAsync());
    }

    public async Task<bool> IsActiveAsync(string token)
    {
        return await _context.TokenStorage.AnyAsync(t =>
            t.Token == token && t.Active == true && t.Expiration > DateTime.UtcNow);
    }

    public async Task DeactivateAsync(string token)
    {
        await _context.TokenStorage
            .Where(t => t.Token == token)
            .ExecuteUpdateAsync(s =>
                s.SetProperty(t => t.Active, false));
    }

    private string GetCurrentAsync()
    {
        var authorizationHeader = _httpContextAccessor
            .HttpContext!.Request.Headers["authorization"];

        return authorizationHeader == StringValues.Empty
            ? string.Empty
            : authorizationHeader.Single()!.Split(" ").Last();
    }
    
    public async Task SaveTokenAsync(string token, User user)
    {
        await _context.TokenStorage
            .AddAsync(new TokenStorage()
            {
                Active = true,
                Expiration = DateTime.UtcNow.AddDays(_jwtOptions.Value.DurationInDays),
                Token = token,
                User = user,
            });
        await _context.SaveChangesAsync();
    }
}