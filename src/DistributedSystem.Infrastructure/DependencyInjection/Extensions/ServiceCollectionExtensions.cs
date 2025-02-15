using DistributedSystem.Application.Abstractions;
using DistributedSystem.Infrastructure.Authentication;
using DistributedSystem.Infrastructure.Caching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DistributedSystem.Infrastructure.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddTransient<IJwtTokenService, JwtTokenService>();
        services.AddTransient<ICacheService, CacheService>();
    }

    public static void AddRedisService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(redisOptions =>
        {
            var redisConnectionString = configuration.GetConnectionString("Redis");
            redisOptions.Configuration = redisConnectionString;
        });
    } 
}
