using Microsoft.Extensions.Caching.Distributed;
using userService.Api.Services.Caching.Interfaces;

namespace userService.Api.Services.Caching;

public class CachingService : ICachingService
{
    private readonly IDistributedCache _cache;
    private readonly DistributedCacheEntryOptions _options;
    public CachingService(IDistributedCache cache)
    {
        _cache = cache;
        _options = new DistributedCacheEntryOptions {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1),
            SlidingExpiration = TimeSpan.FromSeconds(20)
        };
    }

    public async Task<string> Get(string key)
    {
        return await _cache.GetStringAsync(key);
    }

    public async Task Set(string key, string value)
    {
        await _cache.SetStringAsync(key, value, _options);
    }
}
