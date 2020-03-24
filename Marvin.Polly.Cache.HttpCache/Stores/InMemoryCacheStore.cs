using Marvin.Polly.Cache.HttpCache.Domain;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Marvin.Polly.Cache.HttpCache.Stores
{
    public class InMemoryCacheStore : ICacheStore
    { 
        private readonly ConcurrentDictionary<PrimaryCacheKey, CacheEntry> _store
         = new ConcurrentDictionary<PrimaryCacheKey, CacheEntry>();

        public Task<CacheEntry> GetAsync(
            PrimaryCacheKey key)
        {
            throw new NotImplementedException();
        }

        public Task SetAsync(
            PrimaryCacheKey primaryCacheKey, 
            CacheEntry cacheEntry)
        {
            throw new NotImplementedException();
        }
    }
}
