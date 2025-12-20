using Film.Application.Dtos;

namespace Film.Application.Interfaces;

public interface IReviewService
{
    Task<ReviewDto> AddReviewAsync(CreateReviewDto dto, int userId);
    Task<IEnumerable<ReviewDto>> GetReviewsByMovieIdAsync(int movieId);
    Task<double> GetAverageRatingForMovieAsync(int movieId);
}
