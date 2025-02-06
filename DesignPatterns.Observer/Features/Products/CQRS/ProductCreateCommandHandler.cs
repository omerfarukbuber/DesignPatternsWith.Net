using DesignPatterns.Observer.Database;
using DesignPatterns.Observer.Shared.Results;
using MediatR;

namespace DesignPatterns.Observer.Features.Products.CQRS;

public class ProductCreateCommandHandler(ApplicationDbContextSqlServer context)
    : IRequestHandler<ProductCreateCommand, Result<Product>>
{
    private readonly ApplicationDbContextSqlServer _context = context;

    public async Task<Result<Product>> Handle(ProductCreateCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Id = Guid.NewGuid().ToString(),
            Name = request.Name,
            Description = request.Description,
            Price = request.Price
        };

        await _context.Products.AddAsync(product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(product);

    }
}