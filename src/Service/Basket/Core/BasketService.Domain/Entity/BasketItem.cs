using MongoDB.Bson.Serialization.Attributes;

namespace BasketService.Domain.Entity;

public class BasketItem : Entity<string>
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProductImage { get; set; }
    public decimal ProductPrice { get; set; }
    public long Quantity { get; set; }
    public string DiscountCode { get; set; }
    public int DiscountRate { get; set; }
    
    [BsonIgnore]
    public decimal TotalPrice
    {
        get
        {
            if (DiscountRate != 0)
                return (ProductPrice - (ProductPrice * DiscountRate) / 100) * Quantity;
            return ProductPrice;
        }
    }
}