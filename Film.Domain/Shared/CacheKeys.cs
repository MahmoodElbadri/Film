namespace Film.Domain.Shared;

public static class CacheKeys
{
    public static string MoviesList(int page, int size, string? searchTerm) => $"allMovies:{page}:{size}:{searchTerm}";
    public static string Movie(int id) => $"movie:{id}";
    public static string AllMovies() => $"allMovies";
}
