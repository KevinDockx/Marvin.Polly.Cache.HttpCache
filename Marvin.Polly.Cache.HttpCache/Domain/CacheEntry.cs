using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Marvin.Polly.Cache.HttpCache.Domain
{
    /// <summary>
    /// Each cache entry consists of a cache key and one or more HTTP responses corresponding 
    /// to prior requests that used the same key. 
    /// 
    /// The primary cache key consists of the request method and target URI. However, since HTTP caches 
    /// in common use today are typically limited to caching responses to GET, many caches simply 
    /// decline other methods and use only the URI as the primary cache key.
    ///
    /// If a request target is subject to content negotiation, its cache entry might consist of multiple 
    /// stored responses, each differentiated by a secondary key for the values of the original request's 
    /// selecting header fields.
    /// 
    /// cfr: https://httpwg.org/specs/rfc7234.html
    /// </summary>
    public class CacheEntry : ConcurrentDictionary<SecondaryCacheKey, HttpResponseMessage>
    {
        public PrimaryCacheKey PrimaryCacheKey { get; private set; }

        private readonly ConcurrentDictionary<SecondaryCacheKey, HttpResponseMessage> _httpResponses
        = new ConcurrentDictionary<SecondaryCacheKey, HttpResponseMessage>();

        public CacheEntry(PrimaryCacheKey key)
        {
            PrimaryCacheKey = key;
        }
    }
}
