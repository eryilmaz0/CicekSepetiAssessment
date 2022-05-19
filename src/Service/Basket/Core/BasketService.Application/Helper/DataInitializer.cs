using BasketService.Application.Repository;
using BasketService.Domain.Entity;

namespace BasketService.Application.Helper;

public class DataInitializer
{
    private readonly IBasketRepository _basketRepository;

    public DataInitializer(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }
    
    public async Task InitializeBasketData()
    {
        List<Basket> data = new()
        {
            new() { UserEmail = "eryilmaz0@hotmail.com", LastUpdatedTime = DateTime.Now, BasketItems = new() },
            new() { UserEmail = "user1@hotmail.com", LastUpdatedTime = DateTime.Now, BasketItems = new() },
            new() { UserEmail = "user2@hotmail.com", LastUpdatedTime = DateTime.Now, BasketItems = new() },
            new() { UserEmail = "user3@hotmail.com", LastUpdatedTime = DateTime.Now, BasketItems = new() },
        };
        
        data.ForEach(async (basket) =>
        {
            if (await _basketRepository.FindAsync(x => x.UserEmail.Equals(basket.UserEmail)) == null)
            {
                await _basketRepository.AddBasket(basket);
            }
        });
    }
}