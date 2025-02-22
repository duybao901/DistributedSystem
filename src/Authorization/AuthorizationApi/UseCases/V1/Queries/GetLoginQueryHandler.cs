using AuthorizationApi.Abstractions;
using DistributedSystem.Contract.Abstractions.Message;
using DistributedSystem.Contract.Abstractions.Shared;
using DistributedSystem.Contract.Services.V1.Identity;
using System.Security.Claims;

namespace AuthorizationApi.UseCases.V1.Queries;
public class GetLoginQueryHandler : IQueryHandler<Query.Login, Response.Authenticated>
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ICacheService _cacheService;

    public GetLoginQueryHandler(IJwtTokenService jwtTokenService, ICacheService cacheService)
    {
        _jwtTokenService = jwtTokenService;
        _cacheService = cacheService;
    }

    public async Task<Result<Response.Authenticated>> Handle(Query.Login request, CancellationToken cancellationToken)
    {
        // Check User

        // Generate JWT Token
        var claims = new List<Claim>
        {
            new Claim("UserName", request.UserName),
            new Claim(ClaimTypes.Role, "Senior .NET")
        };

        var accessToken = _jwtTokenService.GenerateAccessToken(claims);
        var refrestToken = _jwtTokenService.GenerateRefreshToken();

        var response = new Response.Authenticated()
        {
            AccessToken = accessToken,
            RefreshToken = refrestToken,
            RefreshTokenExpiryTime = DateTime.Now.AddMinutes(5)
        };

        // Key is unique, ex: email
        await _cacheService.SetAsync(request.UserName, response);

        return Result.Success(response);
    }
}
