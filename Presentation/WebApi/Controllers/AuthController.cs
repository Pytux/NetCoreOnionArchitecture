using Application.DTOs.User;
using Application.Features.Authenticate.Commands.AuthenticateCommands;
using Application.Features.Authenticate.Commands.RegisterCommand;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("v1/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpPost("login")]
    public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request)
    {
        return Ok(await _mediator.Send(new AuthenticateCommand
        {
            Email = request.Email,
            Password = request.Password,
            IpAddress = GenerateIpAddress()
        }));
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(RegisterRequest request)
    {
        return Ok(await _mediator.Send(new RegisterCommand
        {
            Email = request.Email,
            Password = request.Password,
            FullName = request.FullName,
            UserName = request.Username,
            Origin = Request.Headers["origin"]!
        }));
    }

    private string GenerateIpAddress()
    {
        if (Request.Headers.ContainsKey("X-Forwarded-For"))
            return Request.Headers["X-Forwarded-For"]!;
        return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
    }
}