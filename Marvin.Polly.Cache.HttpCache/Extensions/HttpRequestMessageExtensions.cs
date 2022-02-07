﻿using Marvin.Polly.Cache.HttpCache.Contexts;
using Marvin.Polly.Cache.HttpCache.Domain;
using Polly; 

namespace Marvin.Polly.Cache.HttpCache.Extensions
{
    public static class HttpRequestMessageExtensions
    {
        public static void SetPolicyExecutionContextForHttpCache(this HttpRequestMessage request)
        {
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

            // create a policy context, add the HttpCacheContext and set it
            var context = new Context
            {
                { "HttpCacheContext", httpCacheContext }
            };  

            request.SetPolicyExecutionContext(context); 
        }
    }
}
