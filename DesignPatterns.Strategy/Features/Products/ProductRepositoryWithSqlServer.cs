using DesignPatterns.Strategy.Database;
using DesignPatterns.Strategy.Shared.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DesignPatterns.Strategy.Features.Products;

public class ProductRepositoryWithSqlServer(ApplicationDbContextSqlServer context) : IProductRepository
{
    private readonly ApplicationDbContextSqlServer _context = context;
    private readonly DbSet<Product> _dbSet = context.Set<Product>();


    public async Task<Result<List<Product>>> GetAllProductsAsync(CancellationToken cancellationToken = default)
    {
        var products = await _dbSet.ToListAsync(cancellationToken: cancellationToken);
        if (products.IsNullOrEmpty())
        {
            products = [];
        }

        return Result.Success(products);
    }

    public async Task<Result<Product>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var product = await _dbSet.FindAsync(id);
        return product == null
            ? Result.Failure<Product>(Error.NotFound("Product.NotFound", "Product couldn't found."))
            : Result.Success(product);
    }

    public async Task<Result<Product>> SaveAsync(CreateProductDto createProduct, CancellationToken cancellationToken = default)
    {
        var product = new Product
        {
            Id = Guid.NewGuid().ToString(),
            Name = createProduct.Name,
            Description = createProduct.Description,
            Price = createProduct.Price
        };

        await _context.AddAsync(product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(product);
    }

    public async Task<Result> UpdateAsync(UpdateProductDto updateProduct, CancellationToken cancellationToken = default)
    {
        var getResult = await GetByIdAsync(updateProduct.Id, cancellationToken);
        if (getResult.IsFailure)
        {
            return getResult;
        }

        var product = getResult.Data;

        product.Name = updateProduct.Name;
        product.Description = updateProduct.Description;
        product.Price = updateProduct.Price;

        _context.Update(product);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var getResult = await GetByIdAsync(id, cancellationToken);
        if (getResult.IsFailure)
        {
            return getResult;
        }

        _context.Remove(getResult.Data);
        await _context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}