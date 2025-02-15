using DistributedSystem.Contract.Abstractions.Message;

namespace DistributedSystem.Contract.Services.V1.Identity;
public static class Query
{
    public record Login(string UserName, string Password) : IQuery<Response.Authenticated>;
    public record Token(string? AccessToken, string? RefreshToken) : IQuery<Response.Authenticated>;
}

