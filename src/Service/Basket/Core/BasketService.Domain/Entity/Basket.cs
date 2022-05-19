using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BasketService.Domain.Entity;

public class Basket : Entity<string>
{
    public string UserEmail { get; set; }
    public List<BasketItem> BasketItems { get; set; }
}