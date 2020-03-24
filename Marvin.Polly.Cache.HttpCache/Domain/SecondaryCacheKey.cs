using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace Marvin.Polly.Cache.HttpCache.Domain
{
    public class SecondaryCacheKey : Dictionary<string, string>
    {
        public override string ToString() => string.Join("-", Values);

        public SecondaryCacheKey()
        {

        }
    }
}
