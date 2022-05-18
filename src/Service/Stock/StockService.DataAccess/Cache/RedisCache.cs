using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using StockService.Entity.Entity;
using StockService.Entity.Options;

namespace StockService.DataAccess.Cache;

public class RedisCache : ICache
{
    private readonly RedisOptions _redisOptions;
    private IConnectionMultiplexer _connection;
    private IDatabase _database;

    public RedisCache(RedisOptions redisOptions)
    {
        _redisOptions = redisOptions;
    }

    public async Task InitializeConnection()
    {
        var redisOptions = new ConfigurationOptions()
        {
            EndPoints = { _redisOptions.Host },
            DefaultDatabase = _redisOptions.DbIndex,
            Password = _redisOptions.Password,
            SyncTimeout = 10000
        };
        
        this._connection = await ConnectionMultiplexer.ConnectAsync(redisOptions);
        this._database = _connection.GetDatabase();
        await this.InitializeStockData();
    }


    private async Task InitializeStockData()
    {
        IEnumerable<Stock> stocks = new List<Stock>()
        {
            new() { ProductId = 1, TotalStocks = 100, AvailableStocks = 50 },
            new() { ProductId = 2, TotalStocks = 500, AvailableStocks = 250 },
            new() { ProductId = 3, TotalStocks = 20, AvailableStocks = 5 }
        };

        await this.SetToCacheAsync("stocks",stocks);
    }

    public async Task<T> ReadFromCacheAsync<T>(string key)
    {
        if (await _database.KeyExistsAsync(key))
        {
            string dataFromCache = await _database.StringGetAsync(key);
            return JsonSerializer.Deserialize<T>(dataFromCache);
        }

        return default(T);
    }

    public async Task<bool> SetToCacheAsync<T>(string key, T data)
    {
        await _database.StringSetAsync(key, JsonSerializer.Serialize(data));
        return true;
    }
}