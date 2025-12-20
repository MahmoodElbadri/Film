using AutoMapper;
using Film.Application.Dtos;
using Film.Application.Interfaces;
using Film.Domain.Entities;
using Film.Domain.Exceptions;
using Film.Domain.Shared;
using Film.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Film.Infrastructure.Services;

public class ReviewService(
    IEmailService emailService,
    IMovieService movieService,
    ILogger<ReviewService> logger,
    ICacheService redis,
    IMapper mapper,
    MovieDbContext db
) : IReviewService
{
    public async Task<ReviewDto> AddReviewAsync(CreateReviewDto dto, int userId)
    {
        if (!await IsMovieExistAsync(dto.MovieId))
            throw new BadHttpRequestException($"Movie No.{dto.MovieId} not found.");

        // Invalidate caches
        await redis.RemoveCache(CacheKeys.AllReviewsForMovie(dto.MovieId));
        await redis.RemoveCache(CacheKeys.AverageRatingForMovie(dto.MovieId));

        var review = mapper.Map<Review>(dto);
        review.UserId = userId;

        db.Reviews.Add(review);
        await db.SaveChangesAsync();

        var savedReview = await db.Reviews
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == review.Id);

        return mapper.Map<ReviewDto>(savedReview);
    }

    public async Task<double> GetAverageRatingForMovieAsync(int movieId)
    {
        var cached = await redis.GetCache<double>(CacheKeys.AverageRatingForMovie(movieId));
        if (cached != default) return cached;

        if (!await IsMovieExistAsync(movieId))
            throw new BadHttpRequestException($"Movie No.{movieId} not found.");

        var avg = await GetAverageRatingAsync(movieId);
        await redis.SetCache(CacheKeys.AverageRatingForMovie(movieId), avg);
        return avg;
    }

    public async Task<IEnumerable<ReviewDto>> GetReviewsByMovieIdAsync(int movieId)
    {
        var cached = await redis.GetCache<List<ReviewDto>>(CacheKeys.AllReviewsForMovie(movieId));
        if (cached is not null) return cached;

        if (!await IsMovieExistAsync(movieId))
            throw new BadHttpRequestException($"Movie No.{movieId} not found.");

        var reviews = await db.Reviews
            .Include(r => r.User)
            .Where(r => r.MovieId == movieId)
            .ToListAsync();

        var reviewDtos = mapper.Map<List<ReviewDto>>(reviews);
        await redis.SetCache(CacheKeys.AllReviewsForMovie(movieId), reviewDtos);
        return reviewDtos;
    }

    private async Task<bool> IsMovieExistAsync(int movieId)
        => await movieService.GetMovieByIdAsync(movieId) is not null;

    private async Task<double> GetAverageRatingAsync(int movieId)
    {
        if (!await db.Reviews.AnyAsync(r => r.MovieId == movieId)) return 0;

        return await db.Reviews
            .Where(r => r.MovieId == movieId)
            .AverageAsync(r => (double)r.Rating);
    }
}

