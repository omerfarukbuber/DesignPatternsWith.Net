namespace DesignPatterns.Strategy.Features.Products;

public sealed record UpdateProductDto(string Id, string Name, string Description, decimal Price);