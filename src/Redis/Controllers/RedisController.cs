using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Redis.Redis;

namespace Redis.Controllers;

[Route("redis")]
public class RedisController : ControllerBase
{
    private readonly IRedisClient _redisClient;

    public RedisController(IRedisClient redisClient) => _redisClient = redisClient;

    [HttpGet("add-random-key-value")]
    public async Task<IActionResult> AddRandomKeyValue()
    {
        var randomKey = "random_key:" + Guid.NewGuid();
        var value = "value_" + Guid.NewGuid();

        await _redisClient.SetAsync(randomKey, value, TimeSpan.FromHours(12));
        return Ok(randomKey);
    }

    [HttpGet("get-value-by-key")]
    public async Task<IActionResult> GetValueByKey(string key)
    {
        var value = await _redisClient.GetAsync<string>(key);
        return Ok(value);
    }
}