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

public class ReviewService(IEmailService emailService,
    IMovieService movieService,
    ILogger<ReviewService> logger,
    ICacheService redis,
    IMapper mapper,
    MovieDbContext db
    ) : IReviewService
{
    public async Task<ReviewDto> AddReviewAsync(CreateReviewDto dto, int userId)
    {
        var isMovieExist = await IsMovieExist(dto.MovieId);
        if (!isMovieExist)
        {
            throw new BadHttpRequestException($"Movie No.{dto.MovieId} not found.");
        }

        var review = mapper.Map<Review>(dto);
        review.UserId = userId;
        db.Reviews.Add(review);
        await db.SaveChangesAsync();
        var savedReview = await db
            .Reviews
            .Include(tmp=>tmp.User)
            .FirstOrDefaultAsync(tmp=>tmp.Id == review.Id);
        var reviewDto = mapper.Map<ReviewDto>(savedReview);
        return reviewDto;
    }

    public async Task<double> GetAverageRatingForMovieAsync(int movieId)
    {
        var isMovieExist = await IsMovieExist(movieId);
        if (!isMovieExist)
        {
            throw new BadHttpRequestException($"Movie No.{movieId} not found.");
        }
        return await GetAverageRating(movieId);
    }


    public async Task<IEnumerable<ReviewDto>> GetReviewsByMovieIdAsync(int movieId)
    {
        var isMovieExist = await IsMovieExist(movieId);
        if (!isMovieExist)
        {
            throw new BadHttpRequestException($"Movie No.{movieId} not found.");
        }
        var reviews = await db
            .Reviews
            .Include(tmp => tmp.User)
            .Where(tmp => tmp.MovieId == movieId)
            .ToListAsync();

        var reviewDtos = mapper.Map<List<ReviewDto>>(reviews);
        return reviewDtos;
    }

    private async Task<bool> IsMovieExist(int movieId)
    {
        var movie = await movieService.GetMovieByIdAsync(movieId);
        return movie is not null;
    }

    private async Task<double> GetAverageRating(int movieId)
    {
        var reviews = await db
            .Reviews
            .Where(tmp => tmp.MovieId == movieId)
            .ToListAsync();
        if (reviews.Count == 0)
        {
            return 0;
        }
        return reviews.Average(tmp => (double)tmp.Rating);
    }

}
