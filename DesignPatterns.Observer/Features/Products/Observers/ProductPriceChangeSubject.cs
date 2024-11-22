namespace DesignPatterns.Observer.Features.Products.Observers;

public class ProductPriceChangeSubject : IProductPriceChangeSubject
{
    private readonly List<IProductPriceChangeObserver> _observers = [];

    public void RegisterObserver(IProductPriceChangeObserver observer)
    {
        _observers.Add(observer);
    }

    public void UnregisterObserver(IProductPriceChangeObserver observer)
    {
        _observers.Remove(observer);
    }

    public void NotifyObservers(Product product)
    {
        _observers.ForEach(o => o.ProductPriceChangedEvent(product));
    }
}