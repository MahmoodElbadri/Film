using Film.Application.Dtos;

namespace Film.Application.Interfaces;

public interface IAuthService
{
    Task<string> Register(RegisterDto dto);
    Task<string> Login(LoginDto dto);
}
