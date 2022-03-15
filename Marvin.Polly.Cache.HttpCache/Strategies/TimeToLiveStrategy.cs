using Marvin.Polly.Cache.HttpCache.Extensions;
using Polly;
using Polly.Caching; 

namespace Marvin.Polly.Cache.HttpCache.Strategies
{
    /// <summary>
    /// Time to live strategy that inspects HTTP Cache headers to 
    /// determine the time to live for a given HTTP response message
    /// </summary>
    public class TimeToLiveStrategy : ITtlStrategy<HttpResponseMessage>
    {  
        public Ttl GetTtl(Context context, HttpResponseMessage result)
        {
            // Note: use Timespan.Zero to avoid caching
            // (as per https://github.com/App-vNext/Polly/wiki/Cache#usage-recommendations)

            // Only successful responses should potentially cached
            if (!result.IsSuccessStatusCode)
            {
                return new Ttl(TimeSpan.Zero);
            }

            var primaryCacheKey = context.GetPrimaryCacheKey();

            // Only responses to GET requests should potentially be cached
            if (!(primaryCacheKey.HttpMethod == HttpMethod.Get))
            {
                return new Ttl(TimeSpan.Zero);
            }

            // If no cache control header is available, the response
            // should not be cached (FOR NOW - TODO: support others means 
            // of cache control like the Expires header)
            if (!result.Headers.Contains("Cache-Control"))
            {
                return new Ttl(TimeSpan.Zero);
            }

            // If no-store is true, the response should not be cached
            if (result.Headers.CacheControl.NoStore)
            {
                return new Ttl(TimeSpan.Zero);
            }

            // Responses with public/private directives can both
            // potentially be cached by a private cache store
            // - no action required

            // Get max-age and store in cache for that amount of time
            if (result.Headers.CacheControl.MaxAge != null)
            {
                return new Ttl(result.Headers.CacheControl.MaxAge.Value);
            }

            // Store for a few minutes by default (?), or maybe consider heuristic checks: 
            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Caching#freshness
            return new Ttl(new TimeSpan(0, 10, 0));
        }
    }
}
