using DistributedSystem.Contract.Abtractions.Shared;
using MediatR;

namespace DistributedSystem.Contract.Abtractions.Message;

public interface ICommandHandler<TCommand> : IRequestHandler<ICommand, Result>
{
}

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<ICommand<TResponse>, Result<TResponse>>
{
}