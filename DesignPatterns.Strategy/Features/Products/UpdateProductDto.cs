namespace DesignPatterns.Strategy.Features.Products;

public sealed record UpdateProductDto(int Id, string Name, string Description, decimal Price);