using Data;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Newtonsoft.Json;
using System;

namespace Business
{
    public class RedisCacheService : ICacheManager
    {
        private RedisCacheOptions options;
        public RedisCacheService()
        {
            options = new RedisCacheOptions
            {
                Configuration = "127.0.0.1:6379",
                InstanceName = ""
            };
        }

        public string Get(string cacheKey)
        {
            var value = "";
            using (var redisCache = new RedisCache(options))
            {
                value = redisCache.GetString(cacheKey);
            }

            return value;
        }

        public void Remove(string cacheKey)
        {
            using (var redisCache = new RedisCache(options))
            {
                redisCache.Remove(cacheKey);
            }
        }

        public void Set(string cacheKey, string value)
        {
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(90)
            };

            using (var redisCache = new RedisCache(options))
            {
                var valueString = JsonConvert.SerializeObject(value);
                redisCache.SetString(cacheKey, valueString, cacheOptions);
            }
        }
    }
}
