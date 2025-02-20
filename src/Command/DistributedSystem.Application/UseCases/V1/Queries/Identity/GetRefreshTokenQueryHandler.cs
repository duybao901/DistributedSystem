using DistributedSystem.Application.Abstractions;
using DistributedSystem.Contract.Abstractions.Message;
using DistributedSystem.Contract.Abstractions.Shared;
using DistributedSystem.Contract.Services.V1.Identity;
using DistributedSystem.Domain.Exceptions;
using System.Security.Claims;

namespace DistributedSystem.Application.UseCases.V1.Queries.Identity;

public class GetRefreshTokenQueryHandler : IQueryHandler<Query.Token, Response.Authenticated>
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ICacheService _cacheService;

    public GetRefreshTokenQueryHandler(IJwtTokenService jwtTokenService, ICacheService cacheService)
    {
        _jwtTokenService = jwtTokenService;
        _cacheService = cacheService;
    }

    public async Task<Result<Response.Authenticated>> Handle(Query.Token request, CancellationToken cancellationToken)
    {
        var Accesstoken = request.AccessToken;
        var RefreshToken = request.RefreshToken;

        // Giải mã Access Token cũ, lấy danh tính (ClaimsPrincipal) của người dùng
        // Lưu ý: Dù Access Token đã hết hạn nhưng nó vẫn có thể được giải mã để lấy thông tin.
        var principal = _jwtTokenService.GetPrincipalFromExpiredToken(Accesstoken);
        var userNameKey = principal.FindFirstValue("UserName").ToString();

        var authenticated = await _cacheService.GetAsync<Response.Authenticated>(userNameKey);
        /*
            1. Tìm trong bộ nhớ cache (_cacheService.GetAsync) xem có thông tin xác thực của người dùng không.
            2. So sánh Refresh Token từ request với token trong cache: Nếu không khớp, token có thể bị giả mạo.
            3. Kiểm tra thời gian hết hạn của Refresh Token: Nếu hết hạn, yêu cầu bị từ chối và người dùng phải đăng nhập lại
         */
        if (authenticated is null || authenticated.RefreshToken != RefreshToken || authenticated.RefreshTokenExpiryTime <= DateTime.Now)
        {
            throw new IdentityException.TokenException("Request Token Invalid");
        }

        var newAccessToken = _jwtTokenService.GenerateAccessToken(principal.Claims);
        var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

        var newAuthenticated = new Response.Authenticated
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            RefreshTokenExpiryTime = DateTime.Now.AddMinutes(10),
        };

        // Will override with userNameKey
        await _cacheService.SetAsync(userNameKey, newAuthenticated, cancellationToken);

        return Result.Success(newAuthenticated);
    }
}
