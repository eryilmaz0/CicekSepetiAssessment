using MongoDB.Bson.Serialization.Attributes;

namespace BasketService.Domain.Entity;

public class Basket : Entity<Guid>
{
    [BsonId]
    public Guid UserId { get; set; }
    public List<BasketItem> BasketItems { get; set; }
}