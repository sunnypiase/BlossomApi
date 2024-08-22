using BlossomApi.Models;
using BlossomApi.DB;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlossomApi.Dtos;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace BlossomApi.Services
{
    public class ProductRecommendationService
    {
        private readonly BlossomContext _context;
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;

        public ProductRecommendationService(BlossomContext context, IMemoryCache cache, IMapper mapper)
        {
            _context = context;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<List<ProductResponseDto>> GetAlsoBoughtProductsAsync(int productId)
        {
            string cacheKey = $"AlsoBoughtProducts_{productId}";

            if (!_cache.TryGetValue(cacheKey, out List<ProductResponseDto> cachedProducts))
            {
                var cartsWithProduct = await _context.ShoppingCartProducts
                    .Where(scp => scp.ProductId == productId)
                    .Select(scp => scp.ShoppingCartId)
                    .Distinct()
                    .ToListAsync();

                var alsoBoughtProducts = await _context.ShoppingCartProducts
                    .Where(scp => cartsWithProduct.Contains(scp.ShoppingCartId) && scp.ProductId != productId)
                    .GroupBy(scp => scp.Product)
                    .Select(group => new
                    {
                        Product = group.Key,
                        Count = group.Sum(scp => scp.Quantity)
                    })
                    .OrderByDescending(p => p.Count)
                    .Take(10)
                    .ToListAsync();

                if (alsoBoughtProducts == null || !alsoBoughtProducts.Any())
                {
                    return new List<ProductResponseDto>();
                }

                cachedProducts = _mapper.Map<List<ProductResponseDto>>(alsoBoughtProducts.Select(p => p.Product).ToList());

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
                    SlidingExpiration = TimeSpan.FromMinutes(10)
                };

                _cache.Set(cacheKey, cachedProducts, cacheEntryOptions);
            }

            return cachedProducts;
        }
    }
}
