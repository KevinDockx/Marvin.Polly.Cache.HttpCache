using Polly;
using Polly.Caching;
using Polly.Extensions.Http;
using Polly.Registry;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// register a cache provider
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<Polly.Caching.IAsyncCacheProvider, Polly.Caching.Memory.MemoryCacheProvider>();

// register the policy registry services (and discard, don't need it here)
_ = builder.Services.AddPolicyRegistry();

// Register client with custom cache handler. The policy itself is created and
// added to the registry when configuring the request pipeline
builder.Services.AddHttpClient("ClientWithCache") 
        .AddPolicyHandlerFromRegistry((policyRegistry, httpRequestMessage) =>
        {
            var policy = policyRegistry.Get<IAsyncPolicy<HttpResponseMessage>>("CustomCachePolicy");
            return policy;
        });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

var cacheProvider = app.Services.GetService<Polly.Caching.IAsyncCacheProvider>();
var policyRegistry = app.Services.GetService<IPolicyRegistry<string>>();  

// create the cache policy
var cachePolicy = Policy.CacheAsync<HttpResponseMessage>(
           cacheProvider.AsyncFor<HttpResponseMessage>(), 
           TimeSpan.FromSeconds(30), 
           onCacheError: (a, b, c) => { 
               var x = true; } );

policyRegistry.Add("CustomCachePolicy", cachePolicy);


app.Run(); 