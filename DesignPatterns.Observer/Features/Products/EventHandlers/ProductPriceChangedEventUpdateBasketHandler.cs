using MediatR;

namespace DesignPatterns.Observer.Features.Products.EventHandlers;

public class ProductPriceChangedEventUpdateBasketHandler(ILogger<ProductPriceChangedEventUpdateBasketHandler> logger)
    : INotificationHandler<ProductPriceChangedEvent>
{
    private readonly ILogger<ProductPriceChangedEventUpdateBasketHandler> _logger = logger;

    public Task Handle(ProductPriceChangedEvent notification, CancellationToken cancellationToken)
    {
        //Basket update codes
        _logger.LogInformation("Product price has changed. Basket updating...");

        return Task.CompletedTask;
    }
}