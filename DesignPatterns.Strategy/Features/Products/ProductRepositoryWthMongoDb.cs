using DesignPatterns.Strategy.Shared.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace DesignPatterns.Strategy.Features.Products;

public class ProductRepositoryWthMongoDb : IProductRepository
{
    private readonly IMongoCollection<Product> _productsCollection;

    public ProductRepositoryWthMongoDb(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoDB");
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase("DesignPatternsDB");
        _productsCollection = database.GetCollection<Product>("Products");
    }

    public async Task<Result<List<Product>>> GetAllProductsAsync(CancellationToken cancellationToken = default)
    {
        var products =
            await (await _productsCollection.FindAsync(_ => true, cancellationToken: cancellationToken)).ToListAsync(
                cancellationToken: cancellationToken);

        if (products.IsNullOrEmpty())
        {
            products = [];
        }

        return Result.Success(products);
    }

    public async Task<Result<Product>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var product =
            await (await _productsCollection.FindAsync(p => p.Id == id, cancellationToken: cancellationToken))
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        return product == null
            ? Result.Failure<Product>(Error.NotFound("Product.NotFound", "Product couldn't found."))
            : Result.Success(product);
    }

    public async Task<Result<Product>> SaveAsync(CreateProductDto createProduct, CancellationToken cancellationToken = default)
    {
        var product = new Product
        {
            Name = createProduct.Name,
            Description = createProduct.Description,
            Price = createProduct.Price
        };

        await _productsCollection.InsertOneAsync(product, cancellationToken: cancellationToken);

        return Result.Success(product);
    }

    public async Task<Result> UpdateAsync(UpdateProductDto updateProduct, CancellationToken cancellationToken = default)
    {
        await _productsCollection.FindOneAndReplaceAsync<Product>(p => p.Id == updateProduct.Id, new Product
        {
            Id = updateProduct.Id,
            Name = updateProduct.Name,
            Description = updateProduct.Description,
            Price = updateProduct.Price
        }, cancellationToken: cancellationToken);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        await _productsCollection.DeleteOneAsync(p => p.Id == id, cancellationToken);
        return Result.Success();
    }
}