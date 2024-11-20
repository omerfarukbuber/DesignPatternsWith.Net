using Carter;
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
    }
}

public enum StrategyDesignPatternType
{
    SqlServer = 0,
    Postgres = 1,
    MongoDb = 2
}