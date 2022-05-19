using BasketService.Application.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BasketService.Persistence.MongoDB.Context;

public class MongoContext
{
    private IMongoClient _client;
    private IMongoDatabase _database;
    private MongoConfig _config;

    public MongoContext(IOptions<MongoConfig> config)
    {
        _config = config.Value;
        _client = new MongoClient(_config.ConnectionString);
        _database = _client.GetDatabase(_config.Database);
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName) where T : class
    {
        return _database.GetCollection<T>(collectionName);
    }
}