namespace DesignPatterns.Observer.Features.Products.Observers;

public class ProductPriceChangeObserverUpdateBasket(ILogger<ProductPriceChangeObserverUpdateBasket> logger)
    : IProductPriceChangeObserver
{
    private readonly ILogger<ProductPriceChangeObserverUpdateBasket> _logger = logger;

    public void ProductPriceChangedEvent(Product product)
    {
        //Basket update codes
        _logger.LogInformation("Product price has changed. Basket updating...");
    }
}