using Asp.Versioning.Builder;
using Carter;
using DistributedSystem.Presentation.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DistributedSystem.Presentation.APIs.Products;

public class ProductApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/products";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group1 = app.NewVersionedApi("products").MapGroup(BaseUrl).HasApiVersion(1);

        group1.MapGet(string.Empty, () =>
        {
            return Results.Ok("Hello 1");
        });

        var group2 = app.NewVersionedApi("products").MapGroup(BaseUrl).HasApiVersion(2);

        group2.MapGet(string.Empty, () =>
        {
            return Results.Ok("Hello 2");
        });
    }
}
