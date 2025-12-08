using Film.Application.Dtos;
using Film.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Film.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService _authService) : ControllerBase
{
    [HttpPost("register")]
    public IActionResult Register(RegisterDto dto)
    {
        var token = _authService.Register(dto);
        return Ok(new { Token = token });
    }

    [HttpPost("login")]
    public IActionResult Login(LoginDto dto)
    {
        var token = _authService.Login(dto);
        return Ok(new { Token = token });
    }
}
