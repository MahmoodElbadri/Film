using Film.Application.Dtos;

namespace Film.Application.Interfaces;

public interface IUserProfileService
{
    Task<ProfileDto> GetProfileAsync(int userId);
    Task UpdateProfileAsync(int userId, UpdateProfileDto dto);
    Task DeleteProfileAsync(int userId);
    Task UpdateProfileImageAsync(int userId, string path);
}
