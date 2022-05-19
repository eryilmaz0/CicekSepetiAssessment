using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BasketService.Domain.Entity;

public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public TPrimaryKey Id { get; set; }
    public DateTime LastUpdatedTime { get; set; }
}