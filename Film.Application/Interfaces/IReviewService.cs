using Film.Application.Dtos;

namespace Film.Application.Interfaces;

public interface IReviewService
{
    Task AddReviewAsync(CreateReviewDto dto, string userId); 
    Task<IEnumerable<ReviewDto>> GetReviewsByMovieIdAsync(int movieId);
    Task<double> GetAverageRatingAsync(int movieId);
}
