using DistributedSystem.Infrastructure.DependencyInjection.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DistributedSystem.API.DependencyInjection.Extensions;

public static class JwtExtensions
{
    public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            JwtOption jwtOption = new JwtOption();
            configuration.GetSection(nameof(JwtOption)).Bind(jwtOption);

            /**
            * Storing the JWT in the AuthenticationProperties allows you to retrieve it from elsewhere within your application.
            * public async Task<IActionResult> SomeAction()
               {
                   // using Microsoft.AspNetCore.Authentication;
                   var accessToken = await HttpContext.GetTokenAsync("access_token");
                   // ...
               }
            */
            options.SaveToken = true;

            // HMACSHA256 required bytes[]
            var SecretKey = Encoding.UTF8.GetBytes(jwtOption.SecretKey);
            var SecurityKey = new SymmetricSecurityKey(SecretKey);

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false, // on production make it true
                ValidateAudience = false, // on production make it true
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtOption.Issuer,
                ValidAudience = jwtOption.Audience,
                IssuerSigningKey = SecurityKey,  // Khóa bí mật
                ClockSkew = TimeSpan.Zero
            };

            options.Events = new JwtBearerEvents 
            { 
                OnAuthenticationFailed = context =>
                {
                    if(context.Exception.GetType() == typeof(SecurityTokenExpiredException)){
                        context.Response.Headers.Append("IS-TOKEN-EXPIRED", "true");
                    }
                    return Task.CompletedTask;
                }    
            };
        });

        services.AddAuthorization();
    }
}
