using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AccountController : ControllerBase
{
    [HttpGet("")]
    public IActionResult Get()
    {
        return Ok("You are authorized");
    }

    [HttpGet("imAdmin")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetAdmin()
    {
        return Ok("You are admin");
    }
}