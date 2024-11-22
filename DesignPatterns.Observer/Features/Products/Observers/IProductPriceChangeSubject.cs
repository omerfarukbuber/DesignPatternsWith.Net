namespace DesignPatterns.Observer.Features.Products.Observers;

public interface IProductPriceChangeSubject
{
    void RegisterObserver(IProductPriceChangeObserver observer);
    void UnregisterObserver(IProductPriceChangeObserver observer);
    void NotifyObservers(Product product);
}