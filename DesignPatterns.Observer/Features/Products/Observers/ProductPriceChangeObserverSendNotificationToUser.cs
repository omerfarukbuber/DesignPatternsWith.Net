namespace DesignPatterns.Observer.Features.Products.Observers;

public class ProductPriceChangeObserverSendNotificationToUser(ILogger logger) : IProductPriceChangeObserver
{
    private readonly ILogger _logger = logger;

    public void ProductPriceChangedEvent(Product product)
    {
        //Send user a notification to announce the product price has changed
        _logger.LogInformation("Product price has changed which in the favorites. New price: {@Price}", product.Price);
    }
}