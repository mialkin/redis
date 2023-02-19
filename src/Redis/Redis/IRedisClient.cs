using System;
using System.Threading.Tasks;

namespace Redis.Redis;

public interface IRedisClient
{
    public Task SetAsync<T>(string key, T value, TimeSpan ttl);
    public Task<T?> GetAsync<T>(string key);
    public Task<bool> ExistsAsync(string key);
}