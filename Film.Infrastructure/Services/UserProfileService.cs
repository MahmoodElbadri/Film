using Film.Application.Dtos;
using Film.Application.Interfaces;

namespace Film.Infrastructure.Services;

public class UserProfileService : IUserProfileService
{
    public Task DeleteProfileAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public Task<ProfileDto> GetProfileAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateProfileAsync(int userId, UpdateProfileDto dto)
    {
        throw new NotImplementedException();
    }

    public Task UpdateProfileImageAsync(int userId, string path)
    {
        throw new NotImplementedException();
    }
}
