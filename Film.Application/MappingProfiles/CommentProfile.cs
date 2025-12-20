using AutoMapper;
using Film.Application.Dtos;
using Film.Domain.Entities;

namespace Film.Application.MappingProfiles;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<CreateCommentDto, Comment>();
        CreateMap<Comment, CommentDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.AppUser.FullName));
    }
}
