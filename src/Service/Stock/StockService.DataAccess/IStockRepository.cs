using StockService.Entity.Entity;

namespace StockService.DataAccess;

public interface IStockRepository
{
    public Task<Stock> GetStockByProductIdAsync(int productId);
}