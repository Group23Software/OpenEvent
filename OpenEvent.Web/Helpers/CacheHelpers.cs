using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace OpenEvent.Web.Helpers
{
    /// <summary>
    /// Set of helpers for saving and revive items in cache
    /// </summary>
    public class CacheHelpers
    {
        /// <summary>
        /// Insert new item into cache 
        /// </summary>
        /// <param name="distributedCache"></param>
        /// <param name="key"></param>
        /// <param name="prefix"></param>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>Completed task once item has been saved to cache</returns>
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

        /// <summary>
        /// Revive new item from cache
        /// </summary>
        /// <param name="distributedCache"></param>
        /// <param name="key"></param>
        /// <param name="prefix"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>Item of type T</returns>
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