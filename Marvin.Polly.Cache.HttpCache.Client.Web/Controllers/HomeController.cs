using Marvin.Polly.Cache.HttpCache.Client.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Polly;
using System.Diagnostics;

namespace Marvin.Polly.Cache.HttpCache.Client.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(ILogger<HomeController> logger, 
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            // execute a call
            var httpClient = _httpClientFactory.CreateClient("ClientWithCache");
            var request = new HttpRequestMessage(
                HttpMethod.Get, "http://localhost:5124/api/weatherforecast");

            // set the context the policy can use, passing through the 
            // with which the response will be stored in the cache
            request.SetPolicyExecutionContext(new Context("AKeyForTesting"));

            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var deserializedResponse = await response.Content.ReadAsStringAsync();
                return Ok(deserializedResponse);
            }

            return View();
        } 

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}