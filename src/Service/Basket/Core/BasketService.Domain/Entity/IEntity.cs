namespace BasketService.Domain.Entity;

public interface IEntity<TPrimaryKey>
{
    public TPrimaryKey Id { get; set; }
    public DateTime LastUpdatedTime { get; set; }
}