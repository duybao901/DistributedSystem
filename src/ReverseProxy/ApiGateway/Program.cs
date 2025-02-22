using ApiGateway.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRedisApiGateway(builder.Configuration);
builder.Services.AddJwtAuthenticationApiGateway(builder.Configuration);
builder.Services.AddReverseProxyApiGateway(builder.Configuration);

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapReverseProxy();

app.Run();
