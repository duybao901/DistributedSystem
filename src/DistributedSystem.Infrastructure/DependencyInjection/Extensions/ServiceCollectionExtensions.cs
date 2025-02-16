using DistributedSystem.Application.Abstractions;
using DistributedSystem.Contract.JsonConverters;
using DistributedSystem.Infrastructure.Authentication;
using DistributedSystem.Infrastructure.BackgroundJobs;
using DistributedSystem.Infrastructure.Caching;
using DistributedSystem.Infrastructure.DependencyInjection.Options;
using DistributedSystem.Infrastructure.PipelineObservers;
using DistributedSystem.Infrastructure.PipeObservers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Quartz;
using Quartz.Simpl;
using System.Reflection;

namespace DistributedSystem.Infrastructure.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddServicesInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<IJwtTokenService, JwtTokenService>();
        services.AddTransient<ICacheService, CacheService>();
    }

    public static void AddRedisInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(redisOptions =>
        {
            var redisConnectionString = configuration.GetConnectionString("Redis");
            redisOptions.Configuration = redisConnectionString;
        });
    }

    // Configure for masstransit with rabbitMQ
    // Configure for masstransit with rabbitMQ
    public static IServiceCollection AddMasstransitRabbitMQInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var masstransitConfiguration = new MasstransitConfiguration();
        configuration.GetSection(nameof(MasstransitConfiguration)).Bind(masstransitConfiguration);

        var messageBusOption = new MessageBusOptions();
        configuration.GetSection(nameof(MessageBusOptions)).Bind(messageBusOption);

        services.AddMassTransit(cfg =>
        {
            // ===================== Setup for Consumer =====================
            cfg.AddConsumers(Assembly.GetExecutingAssembly()); // Add all of consumers to masstransit instead above command

            // ?? => Configure endpoint formatter. Not configure for producer Root Exchange
            cfg.SetKebabCaseEndpointNameFormatter(); // ??

            cfg.UsingRabbitMq((context, bus) =>
            {
                bus.Host(masstransitConfiguration.Host, masstransitConfiguration.Port, masstransitConfiguration.VHost, h =>
                {
                    h.Username(masstransitConfiguration.UserName);
                    h.Password(masstransitConfiguration.Password);
                });

                // Consumer xử lý 3 lần vẫn lỗi sẽ tạo error quere
                // product-created_error classic D   idle    1   0   1   0.00 / s
                bus.UseMessageRetry(retry
                => retry.Incremental(
                           retryLimit: messageBusOption.RetryLimit,
                           initialInterval: messageBusOption.InitialInterval,
                           intervalIncrement: messageBusOption.IntervalIncrement));

                bus.UseNewtonsoftJsonSerializer();

                bus.ConfigureNewtonsoftJsonSerializer(settings =>
                {
                    settings.Converters.Add(new TypeNameHandlingConverter(TypeNameHandling.Objects));
                    settings.Converters.Add(new DateOnlyJsonConverter());
                    settings.Converters.Add(new ExpirationDateOnlyJsonConverter());
                    return settings;
                });

                bus.ConfigureNewtonsoftJsonDeserializer(settings =>
                {
                    settings.Converters.Add(new TypeNameHandlingConverter(TypeNameHandling.Objects));
                    settings.Converters.Add(new DateOnlyJsonConverter());
                    settings.Converters.Add(new ExpirationDateOnlyJsonConverter());
                    return settings;
                });

                bus.ConnectReceiveObserver(new LoggingReceiveObserver());
                bus.ConnectConsumeObserver(new LoggingConsumeObserver());
                bus.ConnectPublishObserver(new LoggingPublishObserver());
                bus.ConnectSendObserver(new LoggingSendObserver());

                // Rename for Root Exchange and setup for consumer also
                bus.MessageTopology.SetEntityNameFormatter(new KebabCaseEntityNameFormatter());

                // ===================== Setup for Consumer =====================

                // Importantce to create Echange and Queue
                bus.ConfigureEndpoints(context);
            });
        });

        return services;
    }

    // Configure Job
    public static void AddQuartzInfrastructure(this IServiceCollection services)
    {
        services.AddQuartz(config =>
        {
            var jobKey = new JobKey(nameof(ProcessOutBoxMessagesJob));

            config.AddJob<ProcessOutBoxMessagesJob>(jobKey)
                .AddTrigger(
                    trigger =>
                        trigger.ForJob(jobKey)
                            .WithSimpleSchedule(
                                schedule =>
                                    schedule.WithIntervalInSeconds(100)
                                        .RepeatForever()));

            config.UseJobFactory<MicrosoftDependencyInjectionJobFactory>();
        });

        services.AddQuartzHostedService();
    }

    public static void AddMediatRInfrastructure(this IServiceCollection services)
        => services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(AssemblyReference.Assembly));
}
