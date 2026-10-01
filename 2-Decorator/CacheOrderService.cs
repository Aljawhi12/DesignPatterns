using Microsoft.Extensions.Caching.Memory;

namespace _2_Decorator
{
    public class CacheOrderService : IOrderService
    {
        private const string CacheKey = "order";

        private readonly IOrderService _inner;
        private readonly IMemoryCache _cache;
        private readonly ILogger<CacheOrderService> _logger;

        public CacheOrderService(
            IOrderService inner,
            IMemoryCache cache,
            ILogger<CacheOrderService> logger)
        {
            _inner = inner;
            _cache = cache;
            _logger = logger;
        }

        public string GetOrder()
        {
            if (_cache.TryGetValue(CacheKey, out string? cached) && cached is not null)
            {
                _logger.LogInformation("------------Cache hit----------");
                return cached;
            }

            _logger.LogInformation("------------Cache miss----------");

            var result = _inner.GetOrder();

            _cache.Set(CacheKey, result, TimeSpan.FromSeconds(30));

            return result;
        }
    }
}
