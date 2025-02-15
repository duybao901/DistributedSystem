using Carter;
using DistributedSystem.API.DependencyInjection.Extensions;
using DistributedSystem.API.Middleware;
using DistributedSystem.Application.DependencyInjection.Extensions;
using DistributedSystem.Infrastructure.DependencyInjection.Extensions;
using DistributedSystem.Persistence.DependencyInjection.Extensions;
using DistributedSystem.Persistence.DependencyInjection.Options;
using DistributedSystem.Presentation.APIs.Products;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Logging.ClearProviders().AddSerilog();
builder.Host.UseSerilog();

// Add Authentication & Authorization
builder.Services.AddInfrastructureServices();
builder.Services.AddRedisService(builder.Configuration);
builder.Services.AddJwtAuthenticationAPI(builder.Configuration);

// Api
builder.Services.AddControllers().AddApplicationPart(DistributedSystem.Presentation.AssemblyReference.Assembly);
builder.Services.AddSingleton<ProductApi>();

// Add MediatR, AutoMapper
builder.Services.AddConfigureMediatR();
builder.Services.AddConfigureAutoMapper();

// Middleware
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

// Configure Options and SQL
builder.Services.AddInterceptorPersistence();
builder.Services.ConfigureSqlServerRetryOptionsPersistence(builder.Configuration.GetSection(nameof(SqlServerRetryOptions)));
builder.Services.AddRepositoryPersistence();
builder.Services.AddSqlServerPersistence();

// Add Carter
builder.Services.AddCarter();

// Add Swagger
builder.Services
        .AddSwaggerGenNewtonsoftSupport()
        .AddFluentValidationRulesToSwagger()
        .AddEndpointsApiExplorer()
        .AddSwaggerAPI();

builder.Services
    .AddApiVersioning(options => options.ReportApiVersions = true)
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

var app = builder.Build();

// Using middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Add Minial API Enpoint
//var versionedApi = app.NewVersionedApi();
//var apiV1 = app.Services.GetRequiredService<ProductApi>();
//apiV1.AddRoutes(versionedApi);


// Configure the HTTP request pipeline. 
if (builder.Environment.IsDevelopment() || builder.Environment.IsStaging())
    app.UseSwaggerAPI(); // => After MapCarter => Show Version

app.UseHttpsRedirection();

app.UseAuthentication(); // This need to be added before UseAuthorization
app.UseAuthorization();

//app.MapControllers();

// Add API Endpoint with carter module
app.MapCarter();

try
{
    await app.RunAsync();
    Log.Information("Stopped cleanly");
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
    await app.StopAsync();
}
finally
{
    Log.CloseAndFlush();
    await app.DisposeAsync();
}
