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
    public async Task AddReviewAsync(CreateReviewDto dto, string userId)
    {
        if (await IsMovieExist(dto.MovieId))
        {
            throw new BadHttpRequestException("Movie not found.");
        }

        var review = mapper.Map<Review>(dto);
        review.UserId = userId;
        db.Reviews.Add(review);
        await db.SaveChangesAsync();
    }

    public Task<double> GetAverageRatingAsync(int movieId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ReviewDto>> GetReviewsByMovieIdAsync(int movieId)
    {
        throw new NotImplementedException();
    }

    private async Task<bool> IsMovieExist(int movieId)
    {
        var movie = await db.Movies.FindAsync(movieId);
        return movie is not null;
    }
}
