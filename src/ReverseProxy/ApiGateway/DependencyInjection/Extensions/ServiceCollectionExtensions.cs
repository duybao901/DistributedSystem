namespace ApiGateway.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddReverseProxyApiGateway(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddReverseProxy().LoadFromConfig(configuration.GetSection("ReverseProxy"));           
    }
    public static void AddRedisApiGateway(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(redisOptions =>
        {
            var redisConnectionString = configuration.GetConnectionString("Redis");
            redisOptions.Configuration = redisConnectionString;
        });
    }
}
