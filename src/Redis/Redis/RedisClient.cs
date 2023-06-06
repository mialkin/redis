using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Redis.Redis;

public class RedisClient : IRedisClient
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        Converters = { new JsonStringEnumConverter() },
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
    };

    private IDatabase Database => _connectionMultiplexer.GetDatabase() ??
                                  throw new RedisException(
                                      "Failed to obtain an interactive connection to a database inside Redis");

    public RedisClient(IConnectionMultiplexer connectionMultiplexer) => _connectionMultiplexer = connectionMultiplexer;

    public Task SetAsync<T>(string key, T value, TimeSpan ttl)
    {
        var json = JsonSerializer.Serialize(value, _jsonSerializerOptions);
        return Database.StringSetAsync(key, json, ttl);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var redisValue = await Database.StringGetAsync(key);
        return redisValue.HasValue
            ? JsonSerializer.Deserialize<T>(redisValue, _jsonSerializerOptions)
            : default;
    }

    public Task<bool> ExistsAsync(string key) => Database.KeyExistsAsync(key);
    public List<string> GetKeysByPattern(string pattern)
    {
        var endpoints = _connectionMultiplexer.GetEndPoints();
        var server = _connectionMultiplexer.GetServer(endpoints.First());

        var keys = new List<string>();
        
        foreach(var key in server.Keys(pattern: pattern)) {
            keys.Add(key);
        }

        return keys;
    }
}