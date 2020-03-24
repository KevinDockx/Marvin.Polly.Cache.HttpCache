using System;
using System.Collections.Generic;
using System.Text;

namespace Marvin.Polly.Cache.HttpCache.Domain
{
    public class PrimaryCacheKey : Dictionary<string, string>
    {
        public string HttpMethod { get; set; }
        public Uri Uri { get; set; }

        public override string ToString() => string.Join("-", Values);
    }
}
