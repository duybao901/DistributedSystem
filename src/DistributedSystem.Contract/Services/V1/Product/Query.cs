using DistributedSystem.Contract.Abstractions.Message;

namespace DistributedSystem.Contract.Services.V1.Product;

public static class Query
{
    public record GetProductsQuery() : IQuery<List<Response.ProductResponse>>;
    public record GetProductByIdQuery(Guid Id, int a) : IQuery<Response.ProductResponse>;
}
