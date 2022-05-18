using StockService.Entity.Dto;

namespace StockService.Business;

public interface IStockService
{
    public Task<CheckStockAvailabilityResponse> CheckStockAvailability(CheckStockAvailabilityRequest request);
}