using Microsoft.Extensions.DependencyInjection;

namespace Query.Application.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediatRApplication(this IServiceCollection services)
     => services.AddMediatR(config =>
         config.RegisterServicesFromAssembly(AssemblyReference.Assembly));
}
