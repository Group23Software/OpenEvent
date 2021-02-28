using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace OpenEvent.Web.Helpers
{
    public class CacheHelpers
    {
        public static async Task Set<T>(IDistributedCache distributedCache, string key, string prefix, T obj)
        {
            var serializedEvent = JsonConvert.SerializeObject(obj, Formatting.None, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Error
            });
            var encoded = Encoding.UTF8.GetBytes(serializedEvent);
            await distributedCache.SetAsync(prefix + key, encoded, new DistributedCacheEntryOptions()
            {
                SlidingExpiration = TimeSpan.FromMinutes(5),
                AbsoluteExpiration = DateTimeOffset.Now.AddHours(6)
            });
        }

        public static async Task<T> Get<T>(IDistributedCache distributedCache, string key, string prefix)
        {
            var cached = await distributedCache.GetAsync(prefix + key);
            if (cached != null)
            {
                var serialized = Encoding.UTF8.GetString(cached);
                return JsonConvert.DeserializeObject<T>(serialized);
            }

            return default;
        }
    }
}