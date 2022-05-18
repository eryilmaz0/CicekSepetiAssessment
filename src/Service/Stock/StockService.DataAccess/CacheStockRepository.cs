using StockService.DataAccess.Cache;
using StockService.Entity.Entity;

namespace StockService.DataAccess;

public class CacheStockRepository : IStockRepository
{
    private readonly ICache _cache;

    public CacheStockRepository(ICache cache)
    {
        _cache = cache;
    }

    public async Task<Stock> GetStockByProductIdAsync(int productId)
    {
        var stocksFromCache = await _cache.ReadFromCacheAsync<List<Stock>>("stocks");
        return stocksFromCache?.FirstOrDefault(x => x.ProductId.Equals(productId));
    }
}