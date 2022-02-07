using Polly;
using Polly.Caching;

namespace Marvin.Polly.Cache.HttpCache.Strategies
{
    /// <summary>
    /// A strategy for defining the cache key which keeps the HTTP method, target URI 
    /// and, potentially, headers into account
    /// </summary>
    public class HttpCacheCacheKeyStrategy : ICacheKeyStrategy
    {
        public string GetCacheKey(Context context)
        { 
            throw new NotImplementedException();
        }
    }
}
