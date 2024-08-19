using BlossomApi.Models;
using BlossomApi.DB;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlossomApi.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BlossomApi.Services
{
    public class ProductRecommendationService
    {
        private readonly BlossomContext _context;
        private readonly IMemoryCache _cache;

        public ProductRecommendationService(BlossomContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
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

                cachedProducts = alsoBoughtProducts
                    .Select(p => MapToProductResponseDto(p.Product))
                    .ToList();

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
                    SlidingExpiration = TimeSpan.FromMinutes(10)
                };

                _cache.Set(cacheKey, cachedProducts, cacheEntryOptions);
            }

            return cachedProducts;
        }

        private static ProductResponseDto MapToProductResponseDto(Product product)
        {
            return new ProductResponseDto
            {
                Id = product.ProductId,
                Name = product.Name,
                NameEng = product.NameEng,
                Amount = product.AvailableAmount,
                Images = product.Images,
                Brand = product.Brand,
                Price = product.Price,
                Discount = product.Discount,
                IsNew = product.IsNew,
                Rating = product.Rating,
                NumberOfReviews = product.NumberOfReviews,
                NumberOfPurchases = product.NumberOfPurchases,
                NumberOfViews = product.NumberOfViews,
                Article = product.Article,
                Categories = product.Categories.Select(c => new CategoryResponseDto
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name,
                    ParentCategoryId = c.ParentCategoryId
                }).ToList(),
                DieNumbers = product.DieNumbers,
                Reviews = product.Reviews.Select(r => new ReviewDto
                {
                    Name = r.Name,
                    Review = r.ReviewText,
                    Rating = r.Rating,
                    Date = r.Date.ToString("dd.MM.yyyy")
                }).ToList(),
                Characteristics = product.Characteristics.Select(c => new CharacteristicDto
                {
                    Title = c.Title,
                    Desc = c.Desc
                }).ToList(),
                Description = product.Description,
                InStock = product.InStock
            };
        }
    }
}
