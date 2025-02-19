using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Query.Domain.Abstractions.Repositories;
using Query.Infrastructure.Consumer.Repositories;

namespace Query.Persistence.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddServicePersistence(this IServiceCollection services, IConfiguration configuration)
    {      
        services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));
    }
}
