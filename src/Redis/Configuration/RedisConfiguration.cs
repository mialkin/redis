using Ardalis.GuardClauses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Redis.Redis;
using StackExchange.Redis;

namespace Redis.Configuration;

public static class RedisConfiguration
{
    public static void ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RedisSettings>(configuration.GetRequiredSection(nameof(RedisSettings)));
        services.AddSingleton<IConnectionMultiplexer>(x =>
        {
            var redisSettings = x.GetRequiredService<IOptions<RedisSettings>>().Value;
            var options = ConfigurationOptions.Parse(Guard.Against.NullOrWhiteSpace(redisSettings.Configuration));
            return ConnectionMultiplexer.Connect(options);
        });
        
        services.AddSingleton<IRedisClient, RedisClient>();
    }
}