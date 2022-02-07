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
            throw new NotImplementedException();
        }
    }
}
