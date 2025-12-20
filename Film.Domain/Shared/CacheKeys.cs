namespace Film.Domain.Shared;

public static class CacheKeys
{
    // Movies
    public static string MoviesList(int page, int size, string? searchTerm)
        => $"movies:list:{page}:{size}:{searchTerm}";
    public static string Movie(int id) => $"movies:{id}";
    public static string AllMovies() => $"movies:all";

    // Reviews
    public static string ReviewForMovieId(int movieId) => $"reviews:{movieId}";
    public static string AllReviewsForMovie(int movieId) => $"reviews:all:{movieId}";
    public static string AverageRatingForMovie(int movieId) => $"reviews:avg:{movieId}";
}
