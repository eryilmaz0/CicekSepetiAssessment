using BasketService.Application.Repository;
using BasketService.Persistence.MongoDB.Context;
using BasketService.Persistence.MongoDB.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace BasketService.Persistence;

public static class ServiceRegistrator
{
    public static void RegisterPersistenceServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<MongoContext>();
        serviceCollection.AddSingleton<IBasketRepository, MongoBasketRepository>();
    }   
}