namespace Film.Domain.Shared;

public static class CacheKeys
{
    //Cache Keys for Movies
    public static string MoviesList(int page, int size, string? searchTerm) => $"allMovies:{page}:{size}:{searchTerm}";
    public static string Movie(int id) => $"movie:{id}";
    public static string AllMovies() => $"allMovies";

    //Cache Keys for Reviews
    public static string Review(int movieId) => $"review:{movieId}";
    public static string AllReviews() => $"allReviews";
}
