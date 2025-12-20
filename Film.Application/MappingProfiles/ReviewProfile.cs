using AutoMapper;
using Film.Application.Dtos;
using Film.Domain.Entities;

namespace Film.Application.MappingProfiles;

public class ReviewProfile:Profile
{
    public ReviewProfile()
    {
        CreateMap<CreateReviewDto, Review>()
            ;
        CreateMap<Review, ReviewDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName));
    }
}
