using Cinema.EcommerceTicket.Domain.Infrastructure.Cache;
using Cinema.EcommerceTicket.Domain.Shared;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Cinema.EcommerceTicket.Infrastructure.Redis;

public class RedisRepository(IConnectionMultiplexer connectionMultiplexer) : ICacheRepository
{
    private readonly IDatabase _db = connectionMultiplexer.GetDatabase();
    private readonly string REDIS_INSTANCE_NAME = Constants.Redis.REDIS_INSTANCE_NAME;


    public async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken)
    {
        key = FormatKey(key);
        return await _db.KeyExistsAsync(key);
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken) where T : class
    {
        key = FormatKey(key);

        var value = await _db.StringGetAsync(key);
        
        if (value.IsNullOrEmpty) return null;
        return JsonConvert.DeserializeObject<T>(value!);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken)
    {
        key = FormatKey(key);
        await _db.KeyDeleteAsync(key);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan expiration, CancellationToken cancellationToken) where T : class
    {
        key = FormatKey(key);
        var json = JsonConvert.SerializeObject(value);
        await _db.StringSetAsync(key, json, expiration);
    }

    private string FormatKey(string key)
    {
        return $"{REDIS_INSTANCE_NAME}{key}";
    }
}
