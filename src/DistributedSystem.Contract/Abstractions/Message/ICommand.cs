using DistributedSystem.Contract.Abstractions.Shared;
using MediatR;

namespace DistributedSystem.Contract.Abstractions.Message;

public interface ICommand : IRequest<Result>
{

}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{

}
