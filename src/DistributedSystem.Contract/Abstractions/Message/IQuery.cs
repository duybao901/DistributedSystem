using DistributedSystem.Contract.Abtractions.Shared;
using MediatR;

namespace DistributedSystem.Contract.Abtractions.Message;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{

}
