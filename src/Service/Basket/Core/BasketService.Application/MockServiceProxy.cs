using BasketService.Application.Model;
using BasketService.Application.Proxy;

namespace BasketService.Application;

public class MockServiceProxy : IStockProxy
{
    public Task<bool> IsStockAvailableAsync(IsStockAvailableRequest request)
    {
        return Task.FromResult(true);
    }
}