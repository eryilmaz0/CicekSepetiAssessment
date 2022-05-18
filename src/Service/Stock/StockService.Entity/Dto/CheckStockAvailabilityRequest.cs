namespace StockService.Entity.Dto;

public class CheckStockAvailabilityRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}