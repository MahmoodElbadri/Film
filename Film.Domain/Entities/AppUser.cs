using Film.Domain.Enums;

namespace Film.Domain.Entities;

public class AppUser
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public UserRole Role { get; set; }
    public string? ProfileImagePath { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
