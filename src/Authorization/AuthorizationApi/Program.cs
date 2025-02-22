using AuthorizationApi.APIs.Identity;
using AuthorizationApi.DependencyInjection.Extensions;
using AuthorizationApi.Middleware;
using Carter;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Logging.ClearProviders().AddSerilog();
builder.Host.UseSerilog();

// Add Carter
builder.Services.AddCarter();

// Add Swagger
builder.Services
        .AddSwaggerGenNewtonsoftSupport()
        .AddFluentValidationRulesToSwagger()
        .AddEndpointsApiExplorer()
        .AddSwaggerAuthorizationApi();

builder.Services
    .AddApiVersioning(options => options.ReportApiVersions = true)
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

// Api
builder.Services.AddControllers().AddApplicationPart(AuthorizationApi.AssemblyReference.Assembly);
builder.Services.AddSingleton<AuthApi>();

// Service DI
builder.Services.AddMediatRAuthorizationApi();
builder.Services.AddRedisAuthorizationApi(builder.Configuration);
builder.Services.AddServicesAuthorizationApi(); 
builder.Services.AddJwtAuthenticationAPI(builder.Configuration);

// Middleware
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication(); // This need to be added before UseAuthorization
app.UseAuthorization();

// Add API Endpoint with carter module
app.MapCarter();

// Configure the HTTP request pipeline. 
if (builder.Environment.IsDevelopment() || builder.Environment.IsStaging())
    app.UseSwaggerAuthorizationApi(); // => After MapCarter => Show Version

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