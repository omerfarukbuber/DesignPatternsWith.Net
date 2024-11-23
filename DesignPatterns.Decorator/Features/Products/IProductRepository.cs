using DesignPatterns.Decorator.Shared.Results;

namespace DesignPatterns.Decorator.Features.Products;

public interface IProductRepository
{
    Task<Result<List<Product>>> GetAllProductsAsync();
    Task<Result<Product>> GetByIdAsync(string id);
    Task<Result<Product>> SaveProductAsync(ProductRequest productRequest);
    Task<Result> UpdateProductAsync(string id, ProductRequest productRequest);
    Task<Result> DeleteProductAsync(string id);
}