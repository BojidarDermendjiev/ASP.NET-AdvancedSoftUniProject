namespace ServerAspNetCoreAPIMakePC.Infrastructure.Services
{
    using Application.Interfaces;

    public class CacheService : ICacheService
    {
        private readonly ICacheService _cacheService;
        public CacheService(ICacheService cacheService)
        {
           this._cacheService = cacheService;
        }
        public void Set<T>(string key, T value, TimeSpan? absoluteExpiration = null)
        {
            if (absoluteExpiration.HasValue)
            {
                this._cacheService.Set(key, value, absoluteExpiration.Value);
            }
            else
            {
                this._cacheService.Set(key, value);
            }
        }

        public bool TryGetValue<T>(string key, out T value)
        {
            return this._cacheService.TryGetValue(key, out value);
        }

        public void Remove(string key)
        {
            this._cacheService.Remove(key);
        }
    }
}
