using Carter;
using DesignPatterns.Strategy.Shared;
using DesignPatterns.Strategy.Shared.Results;

namespace DesignPatterns.Strategy.Features.Products;

public class ProductsModule() : CarterModule("api/products")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("strategy/{strategyType:int}", (int strategyType, IHttpContextAccessor contextAccessor) =>
        {
            var options = new CookieOptions
            {
                Secure = true,
                IsEssential = true,
                HttpOnly = false
            };
            strategyType = strategyType < 0 ? 0 : strategyType;
            strategyType = strategyType > 2 ? 0 : strategyType;

            var strategyDesignPatternType = ((StrategyDesignPatternType)strategyType).ToString();

            contextAccessor.HttpContext?.Response.Cookies.Append(
                "StrategyDesignPatternType",
                strategyDesignPatternType, options);

            return Results.Ok(strategyDesignPatternType);
        });

        app.MapGet("/", async (IProductRepository productRepository, CancellationToken cancellationToken = default) =>
        {
            var products = await productRepository.GetAllProductsAsync(cancellationToken);
            return products.Match(Results.Ok, ApiResults.Problem);
        });

        app.MapGet("/{id}",
            async (string id, IProductRepository productRepository, CancellationToken cancellationToken = default) =>
            {
                var product = await productRepository.GetByIdAsync(id, cancellationToken);
                return product.Match(Results.Ok, ApiResults.Problem);
            });

        app.MapPost("/",
            async (CreateProductDto createProduct, IProductRepository productRepository,
                CancellationToken cancellationToken = default) =>
            {
                var product = await productRepository.SaveAsync(createProduct, cancellationToken);
                return product.Match(Results.Ok, ApiResults.Problem);
            });
    }
}