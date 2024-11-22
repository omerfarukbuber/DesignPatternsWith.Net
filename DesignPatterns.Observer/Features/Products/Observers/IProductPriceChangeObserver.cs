namespace DesignPatterns.Observer.Features.Products.Observers;

public interface IProductPriceChangeObserver
{
    void ProductPriceChangedEvent(Product product);
}