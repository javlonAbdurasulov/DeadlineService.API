using DeadlineService.Application.Interfaces.Base;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DeadlineService.Application.Services
{
    public class RedisCacheService<T>:IRedisCacheService<T>
    {
        public IDistributedCache _cache;
        public DistributedCacheEntryOptions _options;

        public RedisCacheService(IDistributedCache cache, IConfiguration configuration)
        {
            _cache = cache;
            int absoluteExpirationRelativeToNow = Convert.ToInt32(configuration.GetConnectionString("Default"));
            _options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(absoluteExpirationRelativeToNow)
            };
        }

        public async Task SetAsync(string key, byte[] value)
        {
            await _cache.SetAsync(key, value, _options);
        }

        public async Task<byte[]> GetAsync(string key, T value)
        {
            var cacheResponse = await _cache.GetAsync(key);
            if(cacheResponse != null)
            {
                return cacheResponse;
            }
            var byteForCache = JsonSerializer.SerializeToUtf8Bytes(value);

            await _cache.SetAsync(key, byteForCache, _options);

            return byteForCache;
        }
    }
}
