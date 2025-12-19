using AutoMapper;
using AutoMapper.QueryableExtensions;
using Film.Application.Dtos;
using Film.Application.Interfaces;
using Film.Domain.Entities;
using Film.Domain.Exceptions;
using Film.Domain.Shared;
using Film.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Film.Infrastructure.Services;

public class MovieService(
    IMapper mapper,
    MovieDbContext db,
    ICacheService redis,
    ILogger<MovieService> logger,
    IEmailService emailService
    ) : IMovieService
{
    private async Task InvalidateAllMovieCaches()
    {
        logger.LogInformation("Invalidating all movie caches...");
        await redis.RemoveCache(CacheKeys.AllMovies());

        int movieCount = await db.Movies.CountAsync();
        for (int i = 1; i <= movieCount; i++)
        {
            await redis.RemoveCache(CacheKeys.MoviesList(i, 6, "")); // Base list
            await redis.RemoveCache(CacheKeys.MoviesList(i, 6, null)); // In case of null search
        }

        logger.LogInformation("Cache invalidation completed.");
    }

    public async Task<MovieDto> CreateAsync(CreateMovieDto dto)
    {
        var movie = mapper.Map<Movie>(dto);
        db.Add(movie);
        await db.SaveChangesAsync();

        await InvalidateAllMovieCaches();

        var movieDto = mapper.Map<MovieDto>(movie);
        logger.LogInformation($"Created movie '{movieDto.Title}' (ID: {movieDto.Id})");
       await emailService.SendNewMovieEmailAsync(movieDto);
        return movieDto;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var movie = await db.Movies.FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
            throw new NotFoundException(nameof(Movie), id.ToString());

        db.Movies.Remove(movie);
        await db.SaveChangesAsync();

        await redis.RemoveCache(CacheKeys.Movie(id));
        await InvalidateAllMovieCaches();

        logger.LogInformation($"Deleted movie (ID: {id})");
        return true;
    }

    public async Task<PagedResult<MovieDto>> GetAllMoviesAsync(int pageNumber, int pageSize, string searchTerm)
    {
        string safeSearchTerm = searchTerm?.ToLower() ?? string.Empty;
        string cacheKey = CacheKeys.MoviesList(pageNumber, pageSize, safeSearchTerm);

        var cachedItems = await redis.GetCache<List<MovieDto>>(cacheKey);
        if (cachedItems != null)
        {
            logger.LogInformation($"Cache HIT: {cacheKey}");
            var total = await db.Movies.CountAsync(m => m.Title.ToLower().Contains(safeSearchTerm));
            return new PagedResult<MovieDto>(cachedItems, total, pageNumber, pageSize);
        }

        var moviesQuery = db.Movies
            .Where(m => m.Title.ToLower().Contains(safeSearchTerm))
            .ProjectTo<MovieDto>(mapper.ConfigurationProvider);

        var totalCount = await moviesQuery.CountAsync();
        var items = await moviesQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        await redis.SetCache(cacheKey, items);
        logger.LogInformation($"Cache MISS → Stored: {cacheKey}");

        return new PagedResult<MovieDto>(items, totalCount, pageNumber, pageSize);
    }

    public async Task<IEnumerable<MovieDto>> GetAllMoviesAsync()
    {
        var cachedMovies = await redis.GetCache<List<MovieDto>>(CacheKeys.AllMovies());
        if (cachedMovies != null)
        {
            logger.LogInformation(" Cache HIT: AllMovies");
            return cachedMovies;
        }

        var movies = await db.Movies
            .ProjectTo<MovieDto>(mapper.ConfigurationProvider)
            .ToListAsync();

        await redis.SetCache(CacheKeys.AllMovies(), movies);
        logger.LogInformation(" Cache MISS → Stored: AllMovies");

        return movies;
    }

    public async Task<MovieDto?> GetMovieByIdAsync(int id)
    {
        var cacheKey = CacheKeys.Movie(id);
        var cachedMovie = await redis.GetCache<MovieDto>(cacheKey);
        if (cachedMovie != null)
        {
            logger.LogInformation($" Cache HIT: {cacheKey}");
            return cachedMovie;
        }

        var movie = await db.Movies.FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
            throw new NotFoundException(nameof(Movie), id.ToString());

        var movieDto = mapper.Map<MovieDto>(movie);
        await redis.SetCache(cacheKey, movieDto);

        logger.LogInformation($" Cache MISS → Stored: {cacheKey}");
        return movieDto;
    }

    public async Task<MovieDto> UpdateAsync(int id, CreateMovieDto dto)
    {
        var movie = await db.Movies.FindAsync(id);
        if (movie == null)
            throw new KeyNotFoundException($"Movie with id {id} not found");

        mapper.Map(dto, movie);
        await db.SaveChangesAsync();

        await redis.RemoveCache(CacheKeys.Movie(id));
        await InvalidateAllMovieCaches();

        var movieDto = mapper.Map<MovieDto>(movie);
        logger.LogInformation($"Updated movie '{movieDto.Title}' (ID: {id})");
      await  emailService.SendNewMovieEmailAsync(movieDto);
        return movieDto;
    }
}
