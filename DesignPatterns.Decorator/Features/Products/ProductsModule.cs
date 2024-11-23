using Carter;
using DesignPatterns.Decorator.Shared.Results;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DesignPatterns.Decorator.Features.Products;

public class ProductsModule() : CarterModule("api/products")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (IProductRepository repository) =>
        {
            var productResult = await repository.GetAllProductsAsync();

            return productResult.Match(Results.Ok, ApiResults.Problem);
        });

        app.MapGet("/{id}", async (string id, IProductRepository repository) =>
        {
            var productResult = await repository.GetByIdAsync(id);

            return productResult.Match(Results.Ok, ApiResults.Problem);
        });

        app.MapPost("/", async ([FromBody] ProductRequest productRequest, IProductRepository repository) =>
        {
            var productResult = await repository.SaveProductAsync(productRequest);

            return productResult.Match(Results.Ok, ApiResults.Problem);
        });

        app.MapPut("/{id}",
            async (string id, [FromBody] ProductRequest productRequest, IProductRepository repository) =>
            {
                var productResult = await repository.UpdateProductAsync(id, productRequest);

                return productResult.Match(Results.NoContent, ApiResults.Problem);
            });

        app.MapDelete("/{id}", async (string id, IProductRepository repository) =>
        {
            var productResult = await repository.DeleteProductAsync(id);

            return productResult.Match(Results.NoContent, ApiResults.Problem);
        });
    }
}