using MediatR;

namespace DesignPatterns.Observer.Features.Products;

public class ProductPriceChangedEvent : INotification
{
    public Product Product { get; init; } = new();
}