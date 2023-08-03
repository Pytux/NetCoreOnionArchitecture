using Application.DTOs.User;
using Application.Interfaces;
using Application.Wrappers;
using MediatR;

namespace Application.Features.Authenticate.Commands.RegisterCommand;

public class RegisterCommand : IRequest<Response<AuthenticationResponse>>
{
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Origin { get; set; } = null!;
}

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Response<AuthenticationResponse>>
{
    private readonly IAccountService _accountService;

    public RegisterCommandHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public async Task<Response<AuthenticationResponse>> Handle(RegisterCommand request,
        CancellationToken cancellationToken)
    {
        return await _accountService.RegisterAsync(new RegisterRequest
        {
            Email = request.Email,
            Password = request.Password,
            Username = request.UserName,
            FullName = request.FullName
        }, request.Origin);
    }
}