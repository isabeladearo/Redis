using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace src.Infra;

public class RedisDb
{
    private readonly IDatabase _database;

    public RedisDb(IConfiguration configuration)
    {
        var connectionString = configuration["Redis:ConnectionString"];

        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(connectionString);

        _database = redis.GetDatabase();
    }

    public string GetCacheKey(string key) => $"key:{key}";

    public async Task SetStringAsync<T>(string key, T value, TimeSpan? timeout = null)
    {
        var serializedValue = JsonSerializer.Serialize(value);

        await _database.StringSetAsync(key, serializedValue, timeout ?? TimeSpan.FromMinutes(5));
    }

    public async Task<T> GetStringAsync<T>(string key)
    {
        var serializedValue = await _database.StringGetAsync(key);

        if (!serializedValue.HasValue) return default;

        return JsonSerializer.Deserialize<T>(serializedValue);
    }
}
