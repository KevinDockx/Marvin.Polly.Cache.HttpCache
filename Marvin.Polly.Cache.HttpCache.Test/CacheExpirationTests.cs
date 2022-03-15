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
            // ARRANGE
            // set up the response
            var responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent("some content", Encoding.ASCII, "text/html")
            };
            responseMessage.Headers.CacheControl = new CacheControlHeaderValue()
            {
               // TODO TODO TODO
            };

            // mock a handler
            var cacheControlHttpMessageHandlerMock = new Mock<HttpMessageHandler>();

            cacheControlHttpMessageHandlerMock.Protected()
                      .Setup<Task<HttpResponseMessage>>(
                      "SendAsync",
                      ItExpr.IsAny<HttpRequestMessage>(),
                      ItExpr.IsAny<CancellationToken>()).ReturnsAsync(responseMessage);

            // pass through the mock
            var client = new HttpClient(cacheControlHttpMessageHandlerMock.Object);

            // create the request message
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, "http://someurl:5000/api/values");
            var context = httpRequest.GeneratePolicyExecutionContextForHttpCache();

            // ACT
            // Pass through cache
            var httpResponse = await _serviceFixture.HttpCachePolicy.ExecuteAsync(
                async (context) =>
                {
                    return await client.SendAsync(httpRequest);
                },
                context: context);

            // Pass through cache a second time - use for checking stale, 
            // must-revalidate, ... stuff?  Can I force revalidation somehow?
            var httpResponseForSecondRequest = await _serviceFixture.HttpCachePolicy.ExecuteAsync(
                async (context) =>
                {
                    return await client.SendAsync(httpRequest);
                },
                context: context);

            // ASSERT
            var (wasInStore, resultFromStore) = await _serviceFixture.CacheProvider
                .TryGetAsync(context.GetHttpCacheKey(), CancellationToken.None, false);

            // TODO
            Assert.Equal(true, wasInStore);
        }


        [Fact]
        public async Task ExpirationModel_CacheControlMaxAgePositive_ResponseIsStored()
        { 

        }
    }
}