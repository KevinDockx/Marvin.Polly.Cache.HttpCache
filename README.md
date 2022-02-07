# HTTP Cache Support for Polly

Marvin.Polly.Cache.HttpCache is an extension for Polly that enables client-side caching of HTTP response messages.  The implementation adheres to the HTTP Caching standard as defined by RFC7234 (https://www.rfc-editor.org/rfc/rfc7234.txt).  

The extension can work with any cache store that is supported by Polly: 
-  [MemoryCache](https://github.com/App-vNext/Polly.Caching.MemoryCache)
- Any cache implementation that supports [IDistributedCache](https://github.com/App-vNext/Polly.Caching.IDistributedCache) either directly or via [Microsoft.Extensions.Caching.Distributed.IDistributedCache](https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed), which includes: 
	- Redis
	- SQL
	- NCache 
	- ... and lots of third party implemenations
