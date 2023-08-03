using Application.DTOs.User;
using Application.Wrappers;

namespace Application.Interfaces;

public interface IAccountService
{
    Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress);

    Task<Response<AuthenticationResponse>> RegisterAsync(RegisterRequest request, string origin);
}