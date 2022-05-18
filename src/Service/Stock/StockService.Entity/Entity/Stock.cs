namespace StockService.Entity.Entity;

public class Stock
{
    public int ProductId { get; set; }
    public long TotalStocks { get; set; }
    public long AvailableStocks { get; set; }
}