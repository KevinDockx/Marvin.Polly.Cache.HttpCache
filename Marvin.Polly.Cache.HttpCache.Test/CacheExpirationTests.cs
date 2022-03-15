using Marvin.Polly.Cache.HttpCache.Extensions;
using Marvin.Polly.Cache.HttpCache.Test.Fixtures;
using Moq;
using Moq.Protected;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Marvin.Polly.Cache.HttpCache.Test
{
    public class CacheExpirationTests : IClassFixture<GeneralServiceRegistrationFixture>
    {
        private readonly GeneralServiceRegistrationFixture _serviceFixture;

        public CacheExpirationTests(
            GeneralServiceRegistrationFixture generalServiceRegistrationFixture)
        {
            _serviceFixture = generalServiceRegistrationFixture;
        }
         

        [Fact]
        public async Task ExpirationModel_CacheControlMaxAgeNegative_ResponseIsNotStored()
        {
            
        }


        [Fact]
        public async Task ExpirationModel_CacheControlMaxAgePositive_ResponseIsStored()
        { 

        }
    }
}