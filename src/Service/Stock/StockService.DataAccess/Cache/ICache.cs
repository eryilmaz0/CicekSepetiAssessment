namespace StockService.DataAccess.Cache;

public interface ICache
{
    public Task<T> ReadFromCacheAsync<T>(string key);
    public Task<bool> SetToCacheAsync<T>(string key, T data);
}