using Marvin.Polly.Cache.HttpCache.Domain; 

namespace Marvin.Polly.Cache.HttpCache.Contexts
{
    internal class HttpCacheContext
    {
        public PrimaryCacheKey PrimaryCacheKey { get; private set; }

        // TODO secondary keys (?)
        public HttpCacheContext(PrimaryCacheKey primaryCacheKey)
        {
            PrimaryCacheKey = primaryCacheKey;
        }
    }
}
