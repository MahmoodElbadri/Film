using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Film.Application.Dtos;
using Film.Application.Interfaces;
using Film.Domain.Entities;
using Film.Domain.Enums;
using Film.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Film.Infrastructure.Services;

public class AuthService(MovieDbContext db, IConfiguration config) : IAuthService
{
    public async Task<string> Login(LoginDto dto)
    {
        var isUserExist = await db.AppUsers.FirstOrDefaultAsync(tmp => tmp.Email == dto.Email);
        
        if (isUserExist == null || !BCrypt.Net.BCrypt.Verify(dto.Password, isUserExist.PasswordHash))
            throw new BadHttpRequestException("Invalid credentials.");

        return GenerateJwtToken(isUserExist);
    }

    public async Task<string> Register(RegisterDto dto)
    {
        var isUserExist = db.AppUsers.Any(tmp => tmp.Email == dto.Email);
        if (isUserExist)
        {
            throw new BadHttpRequestException("User already registered");
        }

        var user = new AppUser()
        {
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = UserRole.User,
            FullName = dto.FullName,
        };

        db.AppUsers.Add(user);
        await db.SaveChangesAsync();
        return GenerateJwtToken(user);
    }

    private string GenerateJwtToken(AppUser user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
        };

        var token = new JwtSecurityToken(
            config["Jwt:Issuer"],
            config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddHours(3),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
