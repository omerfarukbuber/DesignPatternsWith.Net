using DesignPatterns.Decorator.Shared.Results;

namespace DesignPatterns.Decorator.Features.Products.Decorators;

public class ProductRepositoryDecorator(IProductRepository repository) : IProductRepository
{
    private readonly IProductRepository _repository = repository;

    public virtual Task<Result<List<Product>>> GetAllProductsAsync() =>
        _repository.GetAllProductsAsync();

    public virtual Task<Result<Product>> GetByIdAsync(string id) =>
        _repository.GetByIdAsync(id);

    public virtual Task<Result<Product>> SaveProductAsync(ProductRequest productRequest) =>
        _repository.SaveProductAsync(productRequest);

    public virtual Task<Result> UpdateProductAsync(string id, ProductRequest productRequest) =>
        _repository.UpdateProductAsync(id, productRequest);

    public virtual Task<Result> DeleteProductAsync(string id) =>
        _repository.DeleteProductAsync(id);
}