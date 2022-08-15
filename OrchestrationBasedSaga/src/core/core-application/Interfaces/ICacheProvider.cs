namespace core_application
{
    public interface ICacheProvider
    {
        public Task<T> GetAsync<T>(string key);

        public Task SetAsync<T>(string key, T item, Action<CacheSettings> config);

        public Task RefreshAsync(string key);

        public Task RemoveAsync(string key);
    }

    public sealed class CacheSettings
    {
        public int AbsoluteExpiration { get; set; } 
    }
}
