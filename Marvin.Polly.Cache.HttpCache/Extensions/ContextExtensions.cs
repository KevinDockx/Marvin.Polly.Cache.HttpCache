using Marvin.Polly.Cache.HttpCache.Contexts;
using Marvin.Polly.Cache.HttpCache.Domain;
using Polly;

namespace Marvin.Polly.Cache.HttpCache.Extensions
{
    public static class ContextExtensions
    { 
        public static void SetHttpCacheContext(this Context context, HttpRequestMessage request)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }
             
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.RequestUri is null)
            {
                throw new ArgumentNullException(nameof(request.RequestUri));
            }

            // create cache keys 
            var primaryCacheKey = new PrimaryCacheKey(request.Method, request.RequestUri);

            // TODO secondary keys

            // create the HttpCacheContext
            var httpCacheContext = new HttpCacheContext(primaryCacheKey);

            // Add the HttpCacheContext to the context            
            context.Add("HttpCacheContext", httpCacheContext);
        }
    }
}
