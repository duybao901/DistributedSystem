using AuthorizationApi.Abstractions;
using DistributedSystem.Contract.Abstractions.Message;
using DistributedSystem.Contract.Abstractions.Shared;
using DistributedSystem.Contract.Services.V1.Identity;
using System.Security.Claims;

namespace AuthorizationApi.UseCases.V1.Commands;

public class RevokeTokenCommandHandler : ICommandHandler<Command.Revoke>
{
    private readonly ICacheService _cacheService;
    private readonly IJwtTokenService _jwtTokenService;

    public RevokeTokenCommandHandler(ICacheService cacheService, IJwtTokenService jwtTokenService)
    {
        _cacheService = cacheService;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<Result> Handle(Command.Revoke request, CancellationToken cancellationToken)
    {
        var AccessToken = request.AccessToken;
        var principal = _jwtTokenService.GetPrincipalFromExpiredToken(AccessToken);
        var userNameKey = principal.FindFirstValue("UserName").ToString();

        var authenticated = await _cacheService.GetAsync<Response.Authenticated>(userNameKey);

        if (authenticated is null)
        {
            throw new Exception("Can not get value from Redis");
        }

        await _cacheService.RemoveAsync(userNameKey, cancellationToken);

        return Result.Success();
    }
}
