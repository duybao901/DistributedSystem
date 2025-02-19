using DistributedSystem.Contract.Abstractions.Message;

namespace DistributedSystem.Contract.Services.V1.Identity;

public class Command
{
    public record Revoke(string AccessToken): ICommand;
}
