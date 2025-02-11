using AutoMapper;
using DistributedSystem.Contract.Abstractions.Message;
using DistributedSystem.Contract.Abstractions.Shared;
using DistributedSystem.Contract.Services.V1.Product;
using DistributedSystem.Domain.Abstractions.Repositories;
using DistributedSystem.Domain.Exceptions;
using DistributedSystem.Persistence;

namespace DistributedSystem.Application.UserCases.V1.Queries;

internal class GetProductByIdQueryHandler : IQueryHandler<Query.GetProductByIdQuery, Response.ProductResponse>
{
    private readonly IRepositoryBaseDbContext<ApplicationDbContext, DistributedSystem.Domain.Entities.Product, Guid> _repositoryBaseDbContext;
    private readonly IMapper _mapper;

    public GetProductByIdQueryHandler(IRepositoryBaseDbContext<ApplicationDbContext, Domain.Entities.Product, Guid> repositoryBaseDbContext, IMapper mapper)
    {
        _repositoryBaseDbContext = repositoryBaseDbContext;
        _mapper = mapper;
    }

    public async Task<Result<Response.ProductResponse>> Handle(Query.GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _repositoryBaseDbContext.FindByIdAsync(request.Id)
            ?? throw new ProductException.ProductNotFoundException(request.Id);

        var result = _mapper.Map<Response.ProductResponse>(product);

        return Result.Success(result);
    }
}
