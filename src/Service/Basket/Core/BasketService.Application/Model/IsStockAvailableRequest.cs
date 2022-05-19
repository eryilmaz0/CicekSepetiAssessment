namespace BasketService.Application.Model;

public class IsStockAvailableRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}