using DistributedSystem.Contract.Abtractions.Message;

namespace DistributedSystem.Contract.Services.V1.Product;

public static class Query
{
    public record GetProductQuery() : IQuery<List<Response.ProductResponse>>;
    public record GetProductByIdQuery(Guid Id) : IQuery<Response.ProductResponse>;
}
