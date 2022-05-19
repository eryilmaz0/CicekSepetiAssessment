using BasketService.Application.Model;

namespace BasketService.Application.Proxy;

public interface IStockProxy
{
    public Task<bool> IsStockAvailableAsync(IsStockAvailableRequest request);
}