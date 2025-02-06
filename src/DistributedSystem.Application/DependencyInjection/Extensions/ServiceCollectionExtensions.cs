using DemoCICD.Application.Behaviors;
using DistributedSystem.Application.Behaviors;
using DistributedSystem.Application.Mapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DistributedSystem.Application.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfigureMediatR(this IServiceCollection services)
     => services.AddMediatR(config =>
         config.RegisterServicesFromAssembly(AssemblyReference.Assembly))
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>))
        .AddValidatorsFromAssembly(Contract.AssemblyReference.Assembly, includeInternalTypes: true) // Quét assembly được chỉ định để tìm và đăng ký tất cả các validator vào DI container
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformancePipelineBehavior<,>)) // Measure Permance
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(TracingPipelineBehavior<,>)); // Log/Tracing Request

    public static IServiceCollection AddConfigureAutoMapper(this IServiceCollection services)
        => services.AddAutoMapper(typeof(ServiceProfile));
}
