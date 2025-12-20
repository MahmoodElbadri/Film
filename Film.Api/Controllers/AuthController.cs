using Film.Application.Dtos;
using Film.Application.Interfaces;
using Film.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

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

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetFullName()
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdString != null)
        {
            int userId;
            if (int.TryParse(userIdString, out userId))
            {
                var user = await _authService.getUser(userId);
                return Ok(user);
            }
            else
            {
                return BadRequest("Invalid user ID");
            }
        }
        else
        {
            return Unauthorized();
        }
    }
}
