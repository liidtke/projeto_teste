using Microsoft.Extensions.Caching.Memory;
using WebApi.Domain;
namespace WebApi;

public class IncrementalIdCacheService : IIncrementalIdCacheService
{
    private readonly IMemoryCache _memoryCache;
    private readonly object _lockObject = new object();
    private const string CACHE_KEY_PREFIX = "incremental_id_";

    public IncrementalIdCacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public void Set(string key, int value)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("A chave não pode ser nula ou vazia", nameof(key));

        lock (_lockObject)
        {
            var cacheKey = GetCacheKey(key);
            _memoryCache.Set(cacheKey, value, TimeSpan.FromHours(24)); // Cache por 24 horas
        }
    }

    public void Update(string key, int value)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("A chave não pode ser nula ou vazia", nameof(key));

        lock (_lockObject)
        {
            var cacheKey = GetCacheKey(key);
            if (_memoryCache.TryGetValue(cacheKey, out _))
            {
                _memoryCache.Set(cacheKey, value, TimeSpan.FromHours(24));
            }
            else
            {
                throw new InvalidOperationException($"Chave '{key}' não encontrada no cache");
            }
        }
    }

    public int NextNumber(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("A chave não pode ser nula ou vazia", nameof(key));

        lock (_lockObject)
        {
            var cacheKey = GetCacheKey(key);
            
            if (_memoryCache.TryGetValue(cacheKey, out int currentValue))
            {
                var nextValue = currentValue + 1;
                _memoryCache.Set(cacheKey, nextValue, TimeSpan.FromHours(24));
                return nextValue;
            }
            else
            {
                // Se não existe, inicia com 1
                _memoryCache.Set(cacheKey, 1, TimeSpan.FromHours(24));
                return 1;
            }
        }
    }

    public int? GetCurrentValue(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("A chave não pode ser nula ou vazia", nameof(key));

        var cacheKey = GetCacheKey(key);
        
        if (_memoryCache.TryGetValue(cacheKey, out int currentValue))
        {
            return currentValue;
        }

        return null;
    }

    public void Remove(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("A chave não pode ser nula ou vazia", nameof(key));

        var cacheKey = GetCacheKey(key);
        _memoryCache.Remove(cacheKey);
    }

    public bool Exists(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return false;

        var cacheKey = GetCacheKey(key);
        return _memoryCache.TryGetValue(cacheKey, out _);
    }

    private string GetCacheKey(string key)
    {
        return $"{CACHE_KEY_PREFIX}{key}";
    }
}