using AutoMapper;
using Film.Application.Dtos;
using Film.Domain.Entities;

namespace Film.Application.MappingProfiles;

public class UserProfile:Profile
{
    public UserProfile()
    {
        CreateMap<AppUser, UserDto>();
    }
}
