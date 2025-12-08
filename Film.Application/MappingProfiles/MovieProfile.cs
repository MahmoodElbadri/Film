using AutoMapper;
using Film.Application.Dtos;
using Film.Domain.Entities;

namespace Film.Application.MappingProfiles;

public class MovieProfile:Profile
{
    public MovieProfile()
    {
        CreateMap<CreateMovieDto, Movie>();
        CreateMap<Movie, MovieDto>();
    }
}
