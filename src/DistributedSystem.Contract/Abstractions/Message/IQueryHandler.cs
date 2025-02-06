using DistributedSystem.Contract.Abtractions.Shared;
using MediatR;

namespace DistributedSystem.Contract.Abtractions.Message;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{

}
