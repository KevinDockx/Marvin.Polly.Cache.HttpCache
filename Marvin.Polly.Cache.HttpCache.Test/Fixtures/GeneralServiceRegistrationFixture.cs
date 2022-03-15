using Microsoft.Extensions.DependencyInjection;
using Polly;
using PollyCaching = Polly.Caching;
using Polly.Registry;
using Polly.Caching;
using Moq;
using Moq.Protected;
using System.Text;

namespace Marvin.Polly.Cache.HttpCache.Test.Fixtures
{
    public class GeneralServiceRegistrationFixture
    {

        private ServiceProvider _serviceProvider;

        public IAsyncCacheProvider CacheProvider
        {
            get
            {
#pragma warning disable CS8603 // Possible null reference return.
                return _serviceProvider.GetService<PollyCaching.IAsyncCacheProvider>();
#pragma warning restore CS8603 // Possible null reference return.
            }
        }

        public IPolicyRegistry<string> PolicyRegistry
        {
            get
            {
#pragma warning disable CS8603 // Possible null reference return.
                return _serviceProvider.GetService<IPolicyRegistry<string>>();
#pragma warning restore CS8603 // Possible null reference return.
            }
        }

        public ICacheKeyStrategy CacheKeyStrategy
        {
            get
            {
#pragma warning disable CS8603 // Possible null reference return.
                return _serviceProvider.GetService<ICacheKeyStrategy>();
#pragma warning restore CS8603 // Possible null reference return.
            }
        }

        public ITtlStrategy<HttpResponseMessage> TimeToLiveStrategy
        {
            get
            {
#pragma warning disable CS8603 // Possible null reference return.
                return _serviceProvider.GetService<ITtlStrategy<HttpResponseMessage>>();
#pragma warning restore CS8603 // Possible null reference return.
            }
        }
 

        //var httpCacheTimeToLiveStrategy = app.Services.GetService<ITtlStrategy<HttpResponseMessage>>();


        public AsyncCachePolicy<HttpResponseMessage> HttpCachePolicy
        {
            get
            {
                return Policy.CacheAsync(
                   CacheProvider.AsyncFor<HttpResponseMessage>(),
                   TimeToLiveStrategy,
                   CacheKeyStrategy, 
                   onCacheError: (a, b, c) => {
                       var x = true;
                   });
            }
        } 


        public GeneralServiceRegistrationFixture()
        {
            var services = new ServiceCollection();

            // register a cache provider
            services.AddMemoryCache();
            services.AddSingleton<PollyCaching.IAsyncCacheProvider,
                PollyCaching.Memory.MemoryCacheProvider>();

            // register the HTTP Cache enabling strategies
            services.AddSingleton<PollyCaching.ICacheKeyStrategy,
                Marvin.Polly.Cache.HttpCache.Strategies.CacheKeyStrategy>();
            services.AddSingleton<PollyCaching.ITtlStrategy<HttpResponseMessage>,
                Marvin.Polly.Cache.HttpCache.Strategies.TimeToLiveStrategy>();

            // register the policy registry services (and discard, don't need it here)
            _ = services.AddPolicyRegistry();

            // Register client with custom cache handler for when using HttpClientFactory
            services.AddHttpClient("ClientWithCache")
                    .AddPolicyHandlerFromRegistry((policyRegistry, httpRequestMessage) =>
                    {
                        var policy = policyRegistry
                            .Get<IAsyncPolicy<HttpResponseMessage>>("HttpCachePolicy");
                        return policy;
                    }); 
             
            // build provider
            _serviceProvider = services.BuildServiceProvider();
        }
    }
}
