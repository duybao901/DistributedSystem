using AuthorizationApi.Abstractions;
using AuthorizationApi.Authentication;
using AuthorizationApi.Behaviors;
using AuthorizationApi.Caching;
using FluentValidation;
using MediatR;

namespace AuthorizationApi.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddMediatRAuthorizationApi(this IServiceCollection services)
        => services.AddMediatR(config =>
             config.RegisterServicesFromAssembly(AssemblyReference.Assembly))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>))
            .AddValidatorsFromAssembly(DistributedSystem.Contract.AssemblyReference.Assembly, includeInternalTypes: true);
    public static void AddRedisAuthorizationApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(redisOptions =>
        {
            var redisConnectionString = configuration.GetConnectionString("Redis");
            redisOptions.Configuration = redisConnectionString;
        });
    }

    public static void AddServicesAuthorizationApi(this IServiceCollection services)
    {
        services.AddTransient<IJwtTokenService, JwtTokenService>();
        services.AddTransient<ICacheService, CacheService>();
    }
}
