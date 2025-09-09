namespace WebApi.Domain;

public interface IIncrementalIdCacheService
{
    void Set(string key, int value);
    void Update(string key, int value);
    int NextNumber(string key);
    int? GetCurrentValue(string key);
    void Remove(string key);
    bool Exists(string key);
}