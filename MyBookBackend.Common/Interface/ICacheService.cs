using Microsoft.Extensions.Caching.Memory;

namespace MyBookBackend.Common.Interfaces;

public interface ICacheService
{
    bool TryGetValue<T>(string key, out T? value);

    void Set<T>(
        string key,
        T value,
        MemoryCacheEntryOptions options);

    void Remove(string key);

    void RemoveByPrefix(string prefix);
}