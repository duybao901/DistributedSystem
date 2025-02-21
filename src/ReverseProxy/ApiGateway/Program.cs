using ApiGateway.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJwtAuthenticationApiGateway(builder.Configuration);
builder.Services.AddReverseProxyApiGateway(builder.Configuration);

var app = builder.Build();

app.MapReverseProxy();

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//app.MapControllers();

app.Run();
