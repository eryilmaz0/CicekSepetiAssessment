using System.Linq.Expressions;
using BasketService.Domain.Entity;

namespace BasketService.Application.Repository;

public interface IBasketRepository
{
    public Task<Basket> FindAsync(Expression<Func<Basket, bool>> filter);
    public Task<bool> UpdateBasketAsync(Basket basket);
    public Task AddBasket(Basket basket);
}