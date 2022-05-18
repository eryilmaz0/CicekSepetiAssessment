using StockService.DataAccess;
using StockService.Entity.Dto;

namespace StockService.Business;

public class StockService : IStockService
{
    private readonly IStockRepository _stockRepository;

    public StockService(IStockRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }

    public async Task<CheckStockAvailabilityResponse> CheckStockAvailability(CheckStockAvailabilityRequest request)
    {
        var stock = await _stockRepository.GetStockByProductIdAsync(request.ProductId);

        if (stock is null)
            return new() { IsSuccess = false, ResultMessage = "Stock Not Found." };

        if (!(stock.AvailableStocks >= request.Quantity))
        {
            return new() { IsSuccess = false, ResultMessage = "Stock Not Available." };
        }

        return new() { IsSuccess = true, ResultMessage = "Stock is Available." };
    }
}