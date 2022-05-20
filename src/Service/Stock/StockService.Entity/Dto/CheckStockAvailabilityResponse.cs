namespace StockService.Entity.Dto;

public class CheckStockAvailabilityResponse
{
    public bool IsAvailable { get; set; }
    public string ResultMessage { get; set; }
}