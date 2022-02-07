using Marvin.Polly.Cache.HttpCache.Domain; 

namespace Marvin.Polly.Cache.HttpCache.Contexts
{
    public class HttpCacheContext
    {
        private readonly PrimaryCacheKey _primaryCacheKey;

        public HttpCacheContext(PrimaryCacheKey primaryCacheKey)
        {
            _primaryCacheKey = primaryCacheKey;
        }
    }
}
