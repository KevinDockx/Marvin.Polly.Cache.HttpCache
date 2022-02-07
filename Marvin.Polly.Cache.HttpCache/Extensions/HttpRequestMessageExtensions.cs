using Marvin.Polly.Cache.HttpCache.Contexts;
using Marvin.Polly.Cache.HttpCache.Domain;
using Polly; 

namespace Marvin.Polly.Cache.HttpCache.Extensions
{
    public static class HttpRequestMessageExtensions
    {
        public static void SetPolicyExecutionContextForHttpCache(this HttpRequestMessage request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            // create cache keys 
            var primaryCacheKey = new PrimaryCacheKey();
            primaryCacheKey.Add("HttpMethod", request.Method.ToString());
            primaryCacheKey.Add("Uri", request.RequestUri.ToString());

            // create the HttpCacheContext
            var httpCacheContext = new HttpCacheContext(primaryCacheKey);

            // create a policy context, add the HttpCacheContext and set it
            var context = new Context("DefaultHttpCacheContext");
            context.Add("HttpCacheContext", httpCacheContext);

            request.SetPolicyExecutionContext(context); 
        }
    }
}
