namespace Film.Application.Interfaces;

public interface ICacheService
{
    Task<T> GetCache<T>(string key);
    Task SetCache<T>(string key, T value);
    Task RemoveCache(string key);
}
