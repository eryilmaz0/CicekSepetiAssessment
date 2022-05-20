using BasketService.Application.Handler;
using BasketService.Application.Helper;
using BasketService.Application.Proxy;
using BasketService.Application.Service;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace BasketService.Application;

public static class ServiceRegistrator
{
    public static void RegisterApplicationServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMediatR(typeof(AddProductToBasketRequestHandler));
        serviceCollection.AddSingleton<IAuthService, AuthService>();
        serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        //serviceCollection.AddSingleton<IStockProxy, MockServiceProxy>();
        serviceCollection.AddSingleton<DataInitializer>();
    }
}