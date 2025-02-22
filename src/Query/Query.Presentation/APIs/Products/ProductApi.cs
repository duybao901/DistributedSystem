using Carter;
using DistributedSystem.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using CommandV1 = DistributedSystem.Contract.Services.V1.Product;

namespace DistributedSystem.Presentation.APIs.Products;

public class ProductApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/products";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group1 = app.NewVersionedApi("products").MapGroup(BaseUrl).HasApiVersion(1);
        group1.MapGet(string.Empty, GetProductsV1);
        group1.MapGet("{productId}", GetProductByIdV1);

        //var group2 = app.NewVersionedApi("products").MapGroup(BaseUrl).HasApiVersion(2);
        //group2.MapGet(string.Empty, GetProductsV1);
        //group2.MapGet("{productId}", GetProductByIdV1);
    }

    public static async Task<IResult> GetProductsV1(ISender sender)
    {
        var getProductsQuery = new CommandV1.Query.GetProductsQuery();
        var result = await sender.Send(getProductsQuery);

        return Results.Ok(result);
    }

    public static async Task<IResult> GetProductByIdV1(ISender sender, Guid productId)
    {
        var getProductByIdQuery = new CommandV1.Query.GetProductByIdQuery(productId);
        var result = await sender.Send(getProductByIdQuery);

        return Results.Ok(result);
    }
}
