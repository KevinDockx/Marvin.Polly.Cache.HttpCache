using Marvin.Polly.Cache.HttpCache.Contexts;
using Polly;
using Polly.Caching;

namespace Marvin.Polly.Cache.HttpCache.Strategies
{
    /// <summary>
    /// A strategy for defining the cache key which keeps the HTTP method, target URI 
    /// and, potentially, other headers into account
    /// </summary>
    public class CacheKeyStrategy : ICacheKeyStrategy
    {
        public string GetCacheKey(Context context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // try and find the HttpCacheContext
            if (!context.TryGetValue("HttpCacheContext", out var httpCacheContextAsObject))
            {
                throw new ArgumentNullException("HttpCacheContext not found on policy context.  " +
                    "Set it via context.SetHttpCacheContext(request) " +
                    "or request.SetPolicyExecutionContextForHttpCache()");
            }

            var httpCacheContext = httpCacheContextAsObject as HttpCacheContext;
            if (httpCacheContext == null)
            {
                throw new InvalidCastException("HttpCacheContext value on Context dictionary must be of type HttpCacheContext.");
            }

            // for now, use just the primary key as cache key 
            return httpCacheContext.PrimaryCacheKey.ToString();
        }
    }
}
