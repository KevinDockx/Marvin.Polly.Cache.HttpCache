using System;
using System.Collections.Generic;
using System.Text;

namespace Marvin.Polly.Cache.HttpCache.Domain
{
    public class PrimaryCacheKey : Dictionary<string, object>
    {
        public HttpMethod HttpMethod { get; private set; }

        public Uri Uri { get; private set; }

        public override string ToString() => string.Join("-", Values);

        public PrimaryCacheKey(HttpMethod httpMethod, Uri uri)
        {
            // for quick access
            HttpMethod = httpMethod;
            Uri = uri;

            // for correct ToString() functionality on the dictionary
            Add("HttpMethod", httpMethod.ToString());
            Add("Uri", uri.ToString());
        }
    }
}
