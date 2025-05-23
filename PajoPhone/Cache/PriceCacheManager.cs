using Microsoft.Extensions.Caching.Memory;
using PajoPhone.Api.Scraper;

namespace PajoPhone.Cache;

public class PriceCacheManager
{
    private readonly IMemoryCache _memoryCache;
    private readonly GooshiShopScraper _gooshiShopScraper;

    public PriceCacheManager(IMemoryCache memoryCache , GooshiShopScraper gooshiShopScraper)
    {
        _memoryCache = memoryCache;
        _gooshiShopScraper = gooshiShopScraper;
    }
    public async Task<decimal> GetCachedPrice(string name)
    {
        var cacheKey = $"price_{name}";
        if (_memoryCache.TryGetValue(name, out decimal value))
        {
            return value;
        }
        value = decimal.Parse(await _gooshiShopScraper.GetPriceAsync(name));
        var cacheEntryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
            SlidingExpiration = TimeSpan.FromMinutes(10)
        };
        _memoryCache.Set(name, value, cacheEntryOptions);
        return value;
    }
}