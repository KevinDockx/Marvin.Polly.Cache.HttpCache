using Marvin.Polly.Cache.HttpCache.Stores;
using System;

namespace Marvin.Polly.Cache.HttpCache
{
    public class HttpCacheProvider
    {
        private readonly ICacheStore _cacheStore;

        public HttpCacheProvider(ICacheStore cacheStore)
        {
            _cacheStore = cacheStore;
        }
    }
}
