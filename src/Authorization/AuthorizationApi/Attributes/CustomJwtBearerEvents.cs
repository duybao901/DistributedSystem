using AuthorizationApi.Abstractions;
using DistributedSystem.Contract.Services.V1.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;

namespace AuthorizationApi.Attributes;

public class CustomJwtBearerEvents : JwtBearerEvents
{
    private readonly ICacheService _cacheService;
    // private readonly ILogger<CustomJwtBearerEvents> _logger;

    public CustomJwtBearerEvents(ICacheService cacheService, ILogger<CustomJwtBearerEvents> logger)
    {
        _cacheService = cacheService;
        // _logger = logger;
    }

    public override async Task TokenValidated(TokenValidatedContext context)
    {
        // JsonWebToken accessToken = context.SecurityToken as JsonWebToken;
        // ✅ Kiểm tra và ép kiểu trong 1 bước: Down-cast the property to JsonWebToken
        if (context.SecurityToken is JsonWebToken accessToken)
        {
            var requestToken = accessToken.EncodedToken;

            var userNameKey = accessToken.Claims.FirstOrDefault(p => p.Type == "UserName")?.Value;
            var authenticated = await _cacheService.GetAsync<Response.Authenticated>(userNameKey);

            if (authenticated is null || authenticated.AccessToken != requestToken)
            {
                context.Response.Headers.Add("IS-TOKEN-REVOKED", "true");
                context.Fail("Authentication fail. Token has been revoked!");
            }
        }
        else
        {
            context.Fail("Authentication fail.");
        }
    }
}