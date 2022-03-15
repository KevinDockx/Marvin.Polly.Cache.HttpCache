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

        public static string GetHttpCacheKey(this Context context)
        {
            var httpCacheContext = ValidateContext(context);

            // for now, use just the primary key as cache key 
            return httpCacheContext.PrimaryCacheKey.ToString();
        }

        public static PrimaryCacheKey GetPrimaryCacheKey(this Context context)
        {
            var httpCacheContext = ValidateContext(context);

            // return the primary cache key
            return httpCacheContext.PrimaryCacheKey;
        }

        private static HttpCacheContext ValidateContext(Context context)
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
                throw new InvalidCastException(
                    $"HttpCacheContext value on Context dictionary must be of type {typeof(HttpCacheContext)}");
            }

            return httpCacheContext;
        }
    }
}
