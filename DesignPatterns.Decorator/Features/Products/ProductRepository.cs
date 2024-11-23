using DesignPatterns.Decorator.Database;
using DesignPatterns.Decorator.Shared.Results;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DesignPatterns.Decorator.Features.Products;

public class ProductRepository(ApplicationDbContextSqlServer context) : IProductRepository
{
    private readonly ApplicationDbContextSqlServer _context = context;

    public async Task<Result<List<Product>>> GetAllProductsAsync()
    {
        var products = await _context.Products.ToListAsync();
        if (products.IsNullOrEmpty())
        {
            products = [];
        }

        return Result.Success(products);
    }

    public async Task<Result<Product>> GetByIdAsync(string id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product is null)
        {
            return Result.Failure<Product>(Error.NotFound("Product.NotFound",
                "The product couldn't found which specified by id."));
        }

        return Result.Success(product);
    }

    public async Task<Result<Product>> SaveProductAsync(ProductRequest productRequest)
    {
        var product = productRequest.Adapt<Product>();
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        return Result.Success(product);
    }

    public async Task<Result> UpdateProductAsync(string id, ProductRequest productRequest)
    {
        var productResult = await GetByIdAsync(id);
        if (productResult.IsFailure)
        {
            return productResult;
        }

        var product = productResult.Adapt<Product>();
        product.Name = productRequest.Name;
        product.Description = productRequest.Description;
        product.Price = productRequest.Price;

        _context.Products.Update(product);
        await _context.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> DeleteProductAsync(string id)
    {
        var productResult = await GetByIdAsync(id);

        if (productResult.IsFailure)
        {
            return productResult;
        }

        _context.Products.Remove(productResult.Data);
        await _context.SaveChangesAsync();

        return Result.Success();
    }
}