using DistributedSystem.Contract.Abtractions.Shared;
using MediatR;

namespace DistributedSystem.Contract.Abtractions.Message;

public interface ICommand : IRequest<Result>
{

}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{

}
