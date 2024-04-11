using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace API.CacheMemoryService
{
    public interface IRedisCache
    {
        Task<T?> GetAsync<T>(string key) where T : class;
        Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null) where T : class;
        Task RemoveAsync(string key);
    }

    public class RedisCache(IDistributedCache cache) : IRedisCache
    {
        public async Task<T?> GetAsync<T>(string key) where T : class
        {
            var jsonString = await cache.GetStringAsync(key);

            if (string.IsNullOrEmpty(jsonString))
                return null;

            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null) where T : class
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromHours(1),
                SlidingExpiration = unusedExpireTime
            };

            var jsonString = JsonConvert.SerializeObject(value);

            await cache.SetStringAsync(key, jsonString, options);
        }

        public async Task RemoveAsync(string key)
        {
            await cache.RemoveAsync(key);
        }

    }
}
