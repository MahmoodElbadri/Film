using AutoMapper;
using AutoMapper.QueryableExtensions;
using Film.Application.Dtos;
using Film.Application.Interfaces;
using Film.Domain.Entities;
using Film.Domain.Exceptions;
using Film.Domain.Shared;
using Film.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Film.Infrastructure.Services;

public class MovieService(IMapper mapper, MovieDbContext db, ICacheService redis) : IMovieService
{
    public async Task<MovieDto> CreateAsync(CreateMovieDto dto)
    {
        var movie = mapper.Map<Movie>(dto);
        db.Add(movie); //here you are just marking the entity as added in tracker we only need the async in saving 
        await db.SaveChangesAsync();
        var movieDto = mapper.Map<MovieDto>(movie);
        return movieDto;
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

    public async Task<PagedResult<MovieDto>> GetAllMoviesAsync(int pageNumber, int pageSize, string searchTerm)
    {
        var movies = db.Movies.Where(tmp => tmp.Title.ToLower().Contains(searchTerm.ToLower()));
        var total = await movies.CountAsync();
        var items = await movies
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ProjectTo<MovieDto>(mapper.ConfigurationProvider)
            .ToListAsync();
        var cachedItems = await redis.GetCache<List<MovieDto>>($"allMovies page number: {pageNumber}, page size: {pageSize}, search term: {searchTerm}");
        if (cachedItems != null)
        {
            return new PagedResult<MovieDto>(cachedItems, total, pageNumber, pageSize);
        }
        await redis.SetCache($"allMovies page number: {pageNumber}, page size: {pageSize}, search term: {searchTerm}", items);
        return new PagedResult<MovieDto>(items, total, pageNumber, pageSize);
    }
    public async Task<IEnumerable<MovieDto>> GetAllMoviesAsync()
    {

        var cachedMovies = await redis.GetCache<List<MovieDto>>("allMovies");
        if (cachedMovies != null)
        {
            return cachedMovies;
        }
        var movies = await db.Movies.ProjectTo<MovieDto>(mapper.ConfigurationProvider).ToListAsync();
        
        await redis.SetCache("allMovies", movies);
        return movies;
    }

    public async Task<MovieDto?> GetMovieByIdAsync(int id)
    {
        var cachedMovie = await redis.GetCache<MovieDto>($"movie id: {id}");
        if (cachedMovie != null)
        {
            return cachedMovie;
        }
        var movie = await db.Movies.FirstOrDefaultAsync(tmp => tmp.Id == id);
        if (movie == null)
        {
            throw new NotFoundException(nameof(Movie), id.ToString());
        }
        var movieDto = mapper.Map<MovieDto>(movie);
        await redis.SetCache($"movie id: {id}", movieDto);
        return movieDto;
    }

    public async Task<MovieDto> UpdateAsync(int id, CreateMovieDto dto)
    {
        var cachedMovie = await redis.GetCache<MovieDto>($"movie id: {id}");
        if (cachedMovie != null)
        {
            await redis.RemoveCache($"movie id: {id}");
        }
        var movie = await db.Movies.FindAsync(id);
        if (movie == null)
            throw new KeyNotFoundException($"Movie with id {id} not found");
        mapper.Map(dto, movie);
        await db.SaveChangesAsync();
        var movieDto = mapper.Map<MovieDto>(movie);
        return movieDto;
    }
}
