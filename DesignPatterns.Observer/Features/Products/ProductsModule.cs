using Carter;
using DesignPatterns.Observer.Features.Products.CQRS;
using DesignPatterns.Observer.Shared.Results;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DesignPatterns.Observer.Features.Products;

public class ProductsModule() : CarterModule("api/products")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/", async ([FromBody] ProductRequest productRequest, ISender sender) =>
        {
            var result = await sender.Send(productRequest.Adapt<ProductCreateCommand>());

            return result.Match(Results.Ok, ApiResults.Problem);
        });

        app.MapPut("/{id}", async (string id, [FromBody] ProductRequest productRequest, ISender sender) =>
        {
            var result = await sender.Send(productRequest.Adapt<ProductUpdateCommand>() with { Id = id });

            return result.Match(Results.NoContent, ApiResults.Problem);
        });
    }
}