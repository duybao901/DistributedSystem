using Carter;
using DistributedSystem.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

using CommandV1 = DistributedSystem.Contract.Services.V1.Product;

namespace DistributedSystem.Presentation.APIs.Products;

public class ProductApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/products";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group1 = app.NewVersionedApi("products").MapGroup(BaseUrl).HasApiVersion(1);

        group1.MapPost(string.Empty, CreateProductsV1);
        group1.MapPut("{productId}", UpdateProductsV1);
        group1.MapDelete("{productId}", DeleteProductsV1);
        group1.MapGet(string.Empty, GetProductsV1);
        group1.MapGet("{productId}", GetProductByIdV1);

    }

    public async static Task<IResult> CreateProductsV1(ISender sender, [FromBody] CommandV1.Command.CreateProductCommand createProduct)
    {
        var createProductCommand = new CommandV1.Command.CreateProductCommand(createProduct.Name, createProduct.Price, createProduct.Description);
        var result = await sender.Send(createProductCommand);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> DeleteProductsV1(ISender sender, Guid productId)
    {
        var deleteProductCommand = new CommandV1.Command.DeleteProductCommand(productId);
        var result = await sender.Send(deleteProductCommand);
        return Results.Ok(result);
    }

    public static async Task<IResult> UpdateProductsV1(ISender sender, Guid productId, [FromBody] CommandV1.Command.UpdateProductCommand updateProduct)
    {
        var updateProductCommand = new CommandV1.Command.UpdateProductCommand(productId, updateProduct.Name, updateProduct.Price, updateProduct.Description);
        var result = await sender.Send(updateProductCommand);
        return Results.Ok(result);
    }

    public static async Task<IResult> GetProductsV1(ISender sender)
    {
        var getProductsQuery = new CommandV1.Query.GetProductsQuery();
        var result = await sender.Send(getProductsQuery);

        return Results.Ok(result);
    }

    public static async Task<IResult> GetProductByIdV1(ISender sender, Guid productId, int a)
    {
        var getProductByIdQuery = new CommandV1.Query.GetProductByIdQuery(productId, a);
        var result = await sender.Send(getProductByIdQuery);

        return Results.Ok(result);
    }
}
