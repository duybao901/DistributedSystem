using AuthorizationApi.Abstractions;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationApi.APIs.Identity;
public class AuthApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/auth";
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group1 = app.NewVersionedApi("Authentication")
            .MapGroup(BaseUrl).HasApiVersion(1).RequireAuthorization();

        group1.MapPost("login", AuthenticationV1).AllowAnonymous();
        group1.MapPost("refresh", RefreshTokenV1);
        group1.MapPost("revoke", RevokeTokenV1);

    }
    public static async Task<IResult> AuthenticationV1(ISender sender, [FromBody] DistributedSystem.Contract.Services.V1.Identity.Query.Login login)
    {
        var result = await sender.Send(login);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> RefreshTokenV1(ISender sender, HttpContext httpContext, [FromBody] DistributedSystem.Contract.Services.V1.Identity.Query.Token token)
    {
        var AccessToken = await httpContext.GetTokenAsync("access_token");
        var result = await sender.Send(new DistributedSystem.Contract.Services.V1.Identity.Query.Token(AccessToken, token.RefreshToken));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    // Revoke Token còn hạn được lưu trong Redis
    public static async Task<IResult> RevokeTokenV1(ISender sender, HttpContext httpContext, [FromBody] DistributedSystem.Contract.Services.V1.Identity.Command.Revoke revoke)
    {
        var AccessToken = await httpContext.GetTokenAsync("access_token");
        var result = await sender.Send(new DistributedSystem.Contract.Services.V1.Identity.Command.Revoke(AccessToken));

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }
}