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

            //    result.Headers.CacheControl
            // TODO implement Http cache support

            // Use Timespan.Zero to avoid caching
            // (as per https://github.com/App-vNext/Polly/wiki/Cache#usage-recommendations)
            return new Ttl(new TimeSpan(0, 10, 0));
        }
    }
}
