using System.Text.Json;
using Backend.Business.CacheService.Interfaces;
using StackExchange.Redis;

namespace Backend.Business.CacheService.Concretes;

public class RedisCacheService(IConnectionMultiplexer redis) : IRedisCacheService
{
    private readonly IDatabase _cache = redis.GetDatabase();

    public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
    {
        var jsonData = JsonSerializer.Serialize(value);
        await _cache.StringSetAsync(key, jsonData, expiration);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var data = await _cache.StringGetAsync(key);
        return data.IsNullOrEmpty ? default : JsonSerializer.Deserialize<T>(data!);
    }

    public async Task RemoveAsync(string key)
    {
        await _cache.KeyDeleteAsync(key);
    }
}