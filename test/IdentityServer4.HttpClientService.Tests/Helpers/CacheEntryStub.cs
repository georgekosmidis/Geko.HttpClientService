using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer4.HttpClientService.Tests.Helpers
{
    public class CacheEntryStub : ICacheEntry
    {

        public object Key { get; set; }
        public object Value { get; set; }

        public DateTimeOffset? AbsoluteExpiration { get; set; }
        public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }
        public TimeSpan? SlidingExpiration { get; set; }

        public IList<IChangeToken> ExpirationTokens { get; set; }

        public IList<PostEvictionCallbackRegistration> PostEvictionCallbacks { get; set; }

        public CacheItemPriority Priority { get; set; }
        public long? Size { get; set; }

        public CacheEntryStub(object key, object value)
        {
            Key = key;
            Value = value;
        }
        public void Dispose() { }
    }
}
