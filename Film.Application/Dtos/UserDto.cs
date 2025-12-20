using Film.Domain.Enums;

namespace Film.Application.Dtos;

public class UserDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public UserRole Role { get; set; }
}
