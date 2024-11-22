using MediatR;

namespace DesignPatterns.Observer.Features.Products.EventHandlers;

public class ProductPriceChangedEventSendNotificationHandler(
    ILogger<ProductPriceChangedEventSendNotificationHandler> logger)
    : INotificationHandler<ProductPriceChangedEvent>
{
    private readonly ILogger<ProductPriceChangedEventSendNotificationHandler> _logger = logger;

    public Task Handle(ProductPriceChangedEvent notification, CancellationToken cancellationToken)
    {
        //Send user a notification to announce the product price has changed
        _logger.LogInformation("Product price has changed which in the favorites. New price: {@Price}", notification.Product?.Price);
        return Task.CompletedTask;
    }
}