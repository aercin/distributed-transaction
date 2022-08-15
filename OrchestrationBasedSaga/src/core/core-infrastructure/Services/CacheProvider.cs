using core_application;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace core_infrastructure.Services
{
    public class CacheProvider : ICacheProvider
    {
        private readonly IDistributedCache _distributedCache;
        public CacheProvider(IDistributedCache distributedCache)
        {
            this._distributedCache = distributedCache;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var cacheItem = await this._distributedCache.GetStringAsync(key);

            if (cacheItem != null)
            {
                if (typeof(T).IsValueType)
                {
                    return (T)Convert.ChangeType(cacheItem, typeof(T));
                }
                else
                {
                    return JsonSerializer.Deserialize<T>(cacheItem);
                }
            }
            return default(T);
        }

        public async Task RefreshAsync(string key)
        {
            await this._distributedCache.RefreshAsync(key);
        }

        public async Task RemoveAsync(string key)
        {
            await this._distributedCache.RemoveAsync(key);
        }

        public async Task SetAsync<T>(string key, T item, Action<CacheSettings> config)
        {
            string itemStringRepresentation;

            if (typeof(T).IsValueType)
            {
                itemStringRepresentation = item.ToString();
            }
            else
            {
                itemStringRepresentation = JsonSerializer.Serialize(item);
            }

            var cacheSettings = new CacheSettings();

            config(cacheSettings);

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(cacheSettings.AbsoluteExpiration)
            };

            await this._distributedCache.SetStringAsync(key, itemStringRepresentation, options);
        }
    }
}
