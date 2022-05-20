using System.Linq.Expressions;
using BasketService.Application.Repository;
using BasketService.Domain.Entity;
using BasketService.Persistence.MongoDB.Context;
using MongoDB.Driver;

namespace BasketService.Persistence.MongoDB.Repository;

public class MongoBasketRepository : IBasketRepository
{
    private readonly MongoContext _context;
    private IMongoCollection<Basket> _collection;

    public MongoBasketRepository(MongoContext context)
    {
        _context = context;
        _collection = _context.GetCollection<Basket>(nameof(Basket));
    }

    public async Task<Basket> FindAsync(Expression<Func<Basket, bool>> filter)
    {
        return await this._collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateBasketAsync(Basket basket)
    {
        await _collection.FindOneAndReplaceAsync(x => x.Id.Equals(basket.Id), basket);
        return true;
    }

    public async Task AddBasketAsync(Basket basket)
    {
        await _collection.InsertOneAsync(basket);
    }
}