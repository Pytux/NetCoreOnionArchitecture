using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.DTOs.User;
using Application.Enums;
using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;
using Domain.Entities;
using Domain.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<User> _signInManager;
    private readonly JwtSettings _jwtSettings;

    public AccountService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
        SignInManager<User> signInManager, IOptions<JwtSettings> jwtSettings)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request,
        string ipAddress)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null) throw new ApiException("Email or password is incorrect");

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded) throw new ApiException("Email or password is incorrect");

        var jwToken = await GenerateJwtToken(user);

        var response = new AuthenticationResponse
        {
            Id = user.Id,
            JwToken = new JwtSecurityTokenHandler().WriteToken(jwToken),
            Email = user.Email,
            UserName = user.UserName
        };

        var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
        response.Roles = rolesList.ToList();

        var refreshToken = GenerateRefreshToken(ipAddress);
        response.RefreshToken = refreshToken.Token;

        return new Response<AuthenticationResponse>(response, $"Successfully logged in {user.UserName}");
    }

    public async Task<Response<string>> RegisterAsync(RegisterRequest request, string origin)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user != null) throw new ApiException("Email is already taken");

        user = await _userManager.FindByNameAsync(request.Username);
        if (user != null) throw new ApiException("Username is already taken");

        user = new User
        {
            Email = request.Email,
            UserName = request.Username,
            FullName = request.FullName,
            EmailConfirmed = false
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, Roles.User.ToString());
            return new Response<string>(user.Id, "User registered successfully");
        }
        else
        {
            var errorString = "";
            foreach (var error in result.Errors) errorString += error.Description + " +";
            throw new ApiException($"{errorString}");
        }
    }

    private async Task<JwtSecurityToken> GenerateJwtToken(User user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.UserName!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new("id", user.Id)
        }.Union(userClaims).Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(_jwtSettings.DurationInMinutes),
            signingCredentials: signingCredentials
        );

        return jwtSecurityToken;
    }

    private RefreshToken GenerateRefreshToken(string ipAddress)
    {
        return new RefreshToken
        {
            Token = RandomTokenString(),
            Expires = DateTime.Now.AddDays(7),
            Created = DateTime.Now,
            CreatedByIp = ipAddress
        };
    }

    private string RandomTokenString()
    {
        using var rngCryptoServiceProvider = RandomNumberGenerator.Create();
        var randomBytes = new byte[40];
        rngCryptoServiceProvider.GetBytes(randomBytes);
        return BitConverter.ToString(randomBytes).Replace("-", "");
    }
}