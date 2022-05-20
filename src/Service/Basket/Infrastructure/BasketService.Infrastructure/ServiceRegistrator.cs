using BasketService.Application.Proxy;
using BasketService.Infrastructure.Proxy;
using Microsoft.Extensions.DependencyInjection;

namespace BasketService.Infrastructure;

public static class ServiceRegistrator
{
    public static void RegisterInfrastructureServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IStockProxy, StockProxy>();
    }
}