using Microsoft.Extensions.Caching.Memory;
using MyBookBackend.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBookBackend.Common.Service
{
    public class CacheService:ICacheService
    {
        private readonly IMemoryCache _cache;

        private readonly HashSet<string> _cacheKeys = new();

        private readonly object _lock = new();
        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }
        public bool TryGetValue<T>(string key, out T? value)
        {
            return _cache.TryGetValue(key, out value);
        }
        public void Set<T>(
    string key,
    T value,
    MemoryCacheEntryOptions options)
        {
            _cache.Set(key, value, options);

            lock (_lock)
            {
                _cacheKeys.Add(key);
            }
        }


        public void Remove(string key)
        {
            _cache.Remove(key);

            lock (_lock)
            {
                _cacheKeys.Remove(key);
            }
        }

        public void RemoveByPrefix(string prefix)
        {
            List<string> keys;

            lock (_lock)
            {
                keys = _cacheKeys
                    .Where(k => k.StartsWith(prefix))
                    .ToList();
            }

            foreach (var key in keys)
            {
                _cache.Remove(key);

                lock (_lock)
                {
                    _cacheKeys.Remove(key);
                }
            }
        }



    }
}
