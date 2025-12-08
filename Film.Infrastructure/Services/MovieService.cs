using AutoMapper;
using Film.Application.Dtos;
using Film.Application.Interfaces;
using Film.Domain.Entities;
using Film.Domain.Exceptions;
using Film.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Film.Infrastructure.Services;

public class MovieService(IMapper mapper, MovieDbContext db) : IMovieService
{
    public async Task CreateAsync(CreateMovieDto dto)
    {
        var movie = mapper.Map<Movie>(dto);
        db.Add(movie); //here you are just marking the entity as added in tracker we only need the async in saving 
        await db.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var movie = await db.Movies.FirstOrDefaultAsync(tmp => tmp.Id == id);
        if (movie == null)
        {
            throw new NotFoundException(nameof(Movie), id.ToString());
        }
        else
        {
            db.Movies.Remove(movie); //here you are just marking the entity as deleted in tracker we only need the async in saving
            await db.SaveChangesAsync();
            return true;
        }
    }

    public async Task<IEnumerable<MovieDto>> GetAllMoviesAsync()
    {
        var movies = await db.Movies.ToListAsync();
        var moviesDto = mapper.Map<IEnumerable<MovieDto>>(movies);
        return moviesDto;
    }

    public async Task<MovieDto?> GetMovieByIdAsync(int id)
    {
        var movie = await db.Movies.FirstOrDefaultAsync(tmp => tmp.Id == id);
        if (movie == null)
        {
            throw new NotFoundException(nameof(Movie), id.ToString());
        }
        var movieDto = mapper.Map<MovieDto>(movie);
        return movieDto;
    }

    public async Task<MovieDto> UpdateAsync(int id, CreateMovieDto dto)
    {
        var movie = await db.Movies.FindAsync(id);
        if (movie == null)
            throw new KeyNotFoundException($"Movie with id {id} not found");
        mapper.Map(dto, movie);
        await db.SaveChangesAsync();
        var movieDto = mapper.Map<MovieDto>(movie);
        return movieDto;
    }
}
