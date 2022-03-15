using Marvin.Polly.Cache.HttpCache.Extensions;
using Marvin.Polly.Cache.HttpCache.Test.Fixtures;
using Moq;
using Moq.Protected;
using System.Net.Http.Headers;
using System.Text;
using Xunit;

namespace Marvin.Polly.Cache.HttpCache.Test
{
    public class CacheGenericTests : IClassFixture<GeneralServiceRegistrationFixture>
    {
        private readonly GeneralServiceRegistrationFixture _serviceFixture;

        public CacheGenericTests(
            GeneralServiceRegistrationFixture generalServiceRegistrationFixture)
        {
            _serviceFixture = generalServiceRegistrationFixture;
        }

        // "private"
        // Indicates that the response is intended for a single user only
        // and must not be stored by a shared cache.
        // A private browser cache may store the response in this case.

        // "public"
        // Indicates that the response may be cached by any cache.
        // This can be useful if pages with HTTP authentication or response status
        // codes that aren't normally cacheable should now be cached..

        // "no-store"
        // The cache should not store anything about the client request or server response.
        // A request is sent to the server and a full response is downloaded each and every time.


        [Theory]
        [InlineData(true, true, true, false)]
        [InlineData(true, false, false, true)]
        [InlineData(false, true, false, true)]
        [InlineData(false, false, true, false)]
        [InlineData(true, false, true, false)]
        [InlineData(false, true, true, false)]
        [InlineData(true, true, false, true)]
        [InlineData(false, false, false, true)] // check this? 
        public async Task Generic_CacheControlDirectives_ResponseIsStoredOrNot(
            bool isPublic, bool isPrivate, bool noStore, bool expectedResult)
        { 
            // ARRANGE
            // set up the response
            var responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent("some content", Encoding.ASCII, "text/html")
            };
            responseMessage.Headers.CacheControl = new CacheControlHeaderValue()
            {
                Private = isPrivate,
                Public = isPublic,
                NoStore = noStore
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

            // ASSERT
            var (wasInStore, resultFromStore) = await _serviceFixture.CacheProvider
                .TryGetAsync(context.GetHttpCacheKey(), CancellationToken.None, false);

            Assert.Equal(expectedResult, wasInStore);
        }

        [Fact]
        public async Task Generic_POST_ResponseIsNotStored()
        { 
        }

        [Fact]
        public async Task Generic_PUT_ResponseIsNotStored()
        {
        }

        [Fact]
        public async Task Generic_PATCH_ResponseIsNotStored()
        {
        }

        [Fact]
        public async Task Generic_DELETE_ResponseIsNotStored()
        {
        }

        [Fact]
        public async Task Generic_HEAD_ResponseIsNotStored()
        {
        }
    }
}
