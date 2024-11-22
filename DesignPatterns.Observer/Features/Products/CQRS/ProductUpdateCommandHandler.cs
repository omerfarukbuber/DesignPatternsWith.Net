using DesignPatterns.Observer.Database;
using DesignPatterns.Observer.Features.Products.Observers;
using DesignPatterns.Observer.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DesignPatterns.Observer.Features.Products.CQRS;

internal sealed class ProductUpdateCommandHandler(ApplicationDbContextSqlServer context, IProductPriceChangeSubject priceChangeSubject)
    : IRequestHandler<ProductUpdateCommand, Result>
{
    private readonly ApplicationDbContextSqlServer _context = context;
    private readonly DbSet<Product> _productDbSet = context.Set<Product>();
    private readonly IProductPriceChangeSubject _priceChangeSubject = priceChangeSubject;


    public async Task<Result> Handle(ProductUpdateCommand request, CancellationToken cancellationToken)
    {
        var product = await _productDbSet.FindAsync(request.Id);
        if (product is null)
        {
            return Result.Failure(Error.NotFound("Product.NotFound",
                $"Product couldn't found specified by id = {request.Id}"));
        }

        var isPriceChanged = product.Price != request.Price;

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;

        _productDbSet.Update(product);
        await _context.SaveChangesAsync(cancellationToken);

        if (isPriceChanged)
        {
            _priceChangeSubject.NotifyObservers(product);
        }

        return Result.Success();
    }
}