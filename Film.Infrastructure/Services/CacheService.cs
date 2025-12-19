using Film.Application.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace Film.Infrastructure.Services;

public class CacheService : ICacheService
{
    private readonly IDatabase db;
    public CacheService(IConnectionMultiplexer redis)
    {
        db = redis.GetDatabase();
    }

    public Task<T?> GetCache<T>(string key)
    {
        var value = db.StringGet(key);
        if (value.IsNullOrEmpty)
        {
            return Task.FromResult<T?>(default);
        }

        var result = JsonSerializer.Deserialize<T>(value!);
        return Task.FromResult(result);
    }


    public async Task RemoveCache(string key)
    {
        bool isKeyExist = db.KeyExists(key);
        if (isKeyExist)
        {
            await db.KeyDeleteAsync(key);
        }
    }

    public async Task SetCache<T>(string key, T value)
    {
        var expiryTime = DateTime.Now.AddHours(1);
        var json = JsonSerializer.Serialize(value);
        await db.StringSetAsync(key, json, expiryTime);
    }
}
