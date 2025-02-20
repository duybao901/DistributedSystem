using DistributedSystem.Contract.Abstractions.Message;
using DistributedSystem.Contract.Abstractions.Shared;
using DistributedSystem.Contract.Services.V1.Product;
using Query.Domain.Abstractions.Repositories;
using Query.Domain.Documents;
using MongoDB.Driver;

namespace DistributedSystem.Application.UseCases.V1.Queries.Product;

public sealed class GetProductsQueryHandler : IQueryHandler<Contract.Services.V1.Product.Query.GetProductsQuery, List<Response.ProductResponse>>
{
    private readonly IMongoRepository<ProductProjection> _productRepository;

    public GetProductsQueryHandler(IMongoRepository<ProductProjection> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<List<Response.ProductResponse>>> Handle(Contract.Services.V1.Product.Query.GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.FindAll();
        var result = new List<Response.ProductResponse>();

        foreach (var product in products) {
            result.Add(new Response.ProductResponse(product.DocumentId, product.Name, product.Price, product.Description));
        }
        
        return Result.Success(result);
    }
}