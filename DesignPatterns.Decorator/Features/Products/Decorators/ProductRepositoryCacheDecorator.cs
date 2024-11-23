using DesignPatterns.Decorator.Shared.Results;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace DesignPatterns.Decorator.Features.Products.Decorators;

public class ProductRepositoryCacheDecorator(
    IProductRepository repository,
    IDistributedCache cache) : ProductRepositoryDecorator(repository)
{
    private readonly IDistributedCache _cache = cache;

    public override async Task<Result<Product>> GetByIdAsync(string id)
    {
        var key = $"product-{id}";
        var productCache = await _cache.GetStringAsync(key);
        
        if (!string.IsNullOrWhiteSpace(productCache))
            return Result.Success(JsonConvert.DeserializeObject<Product>(productCache)!);

        var product = await base.GetByIdAsync(id);
        if (product.IsFailure)
        {
            return product;
        }

        await UpdateCache(key, product.Data);

        return product;
    }

    public override async Task<Result<List<Product>>> GetAllProductsAsync()
    {
        const string key = "products";
        var productsCache = await _cache.GetStringAsync(key);

        if (!string.IsNullOrWhiteSpace(productsCache))
            return Result.Success(JsonConvert.DeserializeObject<List<Product>>(productsCache)!);

        var products = await base.GetAllProductsAsync();
        if (products.IsFailure)
        {
            return products;
        }

        await UpdateCache(key, products.Data);

        return products;
    }

    public override async Task<Result<Product>> SaveProductAsync(ProductRequest productRequest)
    {
        var newProductResult = await base.SaveProductAsync(productRequest);

        if (newProductResult.IsSuccess) 
            await UpdateCache($"product-{newProductResult.Data.Id}", newProductResult.Data);

        return newProductResult;
    }

    public override async Task<Result> UpdateProductAsync(string id, ProductRequest productRequest)
    {
        var updatedProductResult = await base.UpdateProductAsync(id, productRequest);
        
        if (!updatedProductResult.IsSuccess) 
            return updatedProductResult;
        
        var productResult = await base.GetByIdAsync(id);
        if (productResult.IsFailure)
            return productResult;
        
        await UpdateCache($"product-{id}", productResult.Data);

        return updatedProductResult;
    }

    public override async Task<Result> DeleteProductAsync(string id)
    {
        var deleteProductResult = await base.DeleteProductAsync(id);

        if (deleteProductResult.IsFailure)
            return deleteProductResult;

        await _cache.RemoveAsync($"product-{id}");
        return deleteProductResult;
    }

    private async Task UpdateCache(string key, object value)
    {
        await _cache.SetStringAsync(key, JsonConvert.SerializeObject(value));
    }
}