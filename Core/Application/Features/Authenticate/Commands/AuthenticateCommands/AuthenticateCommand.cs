using Application.DTOs.User;
using Application.Interfaces;
using Application.Wrappers;
using MediatR;

namespace Application.Features.Authenticate.Commands.AuthenticateCommands;

public class AuthenticateCommand : IRequest<Response<AuthenticationResponse>>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string IpAddress { get; set; } = null!;
}

public class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand, Response<AuthenticationResponse>>
{
    private readonly IAccountService _accountService;

    public AuthenticateCommandHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public async Task<Response<AuthenticationResponse>> Handle(AuthenticateCommand request,
        CancellationToken cancellationToken)
    {
        return await _accountService.AuthenticateAsync(new AuthenticationRequest
        {
            Email = request.Email,
            Password = request.Password
        }, request.IpAddress);
    }
}