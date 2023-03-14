using AspNetCoreRateLimit;
using AspNetCoreRateLimit.Redis;
using StackExchange.Redis;

namespace SourceName.Api.Loaders;

internal static class RateLimitingConfiguration
{
    internal static void AddRateLimiting(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDistributedMemoryCache();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
        services.AddSingleton<IConnectionMultiplexer>(_ =>
            ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")!));
        services.AddRedisRateLimiting();
    }
}