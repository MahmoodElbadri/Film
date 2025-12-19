using Film.Application.Interfaces;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Text.Json;

namespace Film.Infrastructure.Services;

public class CacheService : ICacheService
{
    private readonly IDatabase db;
    private readonly ILogger<CacheService> logger;
    public CacheService(IConnectionMultiplexer redis, ILogger<CacheService> logger)
    {
        db = redis.GetDatabase();
        this.logger = logger;

    }

    /// <summary>
    /// Get cache
    /// </summary>
    /// <typeparam name="T">Generic</typeparam>
    /// <param name="key">Key</param>
    /// <returns>Value</returns>

    /*
     GET Cache key: movie:5 → HIT
     SET Cache key: allMovies:1:5:action → Stored
     REMOVE Cache key: movie:5 → Removed
    */

    public Task<T?> GetCache<T>(string key)
    {
        logger.LogInformation($"Getting cache key: movie:{key}");
        var value = db.StringGet(key);
        if (value.IsNullOrEmpty)
        {
            logger.LogInformation($"Cache key: movie:{key} → MISS");
            return Task.FromResult<T?>(default);
        }
        logger.LogInformation($"Cache key: movie:{key} → HIT");
        var result = JsonSerializer.Deserialize<T>(value!);
        return Task.FromResult(result);
    }


    public async Task RemoveCache(string key)
    {
        bool isKeyExist = db.KeyExists(key);
        if (isKeyExist)
        {
            logger.LogInformation($"Removing cache key: movie:{key}");
            await db.KeyDeleteAsync(key);
        }
    }

    public async Task SetCache<T>(string key, T value)
    {
        var expiryTime = DateTime.Now.AddHours(1);
        var json = JsonSerializer.Serialize(value);
        logger.LogInformation($"Setting cache key: movie:{key}");
        await db.StringSetAsync(key, json, expiryTime);
    }
}
