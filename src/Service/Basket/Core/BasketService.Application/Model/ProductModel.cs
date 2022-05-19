namespace BasketService.Application.Model;

public class ProductModel
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProductImage { get; set; }
    public decimal ProductPrice { get; set; }
    public string DiscountCode { get; set; }
    public int DiscountRate { get; set; }
}