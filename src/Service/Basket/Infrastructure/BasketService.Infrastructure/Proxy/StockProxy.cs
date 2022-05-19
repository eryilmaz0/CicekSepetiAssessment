using BasketService.Application.Model;
using BasketService.Application.Proxy;

namespace BasketService.Infrastructure.Proxy;

public class StockProxy : IStockProxy
{
    public Task<bool> IsStockAvailableAsync(IsStockAvailableRequest request)
    {
        return Task.FromResult(true);
    }
}