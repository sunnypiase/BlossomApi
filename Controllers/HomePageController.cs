using BlossomApi.DB;
using BlossomApi.Dtos;
using BlossomApi.Models;
using BlossomApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BlossomApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomePageController : ControllerBase
    {
        private readonly BlossomContext _context;
        private readonly IShownProductRepository _shownProductRepository;

        public HomePageController(BlossomContext context, IShownProductRepository shownProductRepository)
        {
            _context = context;
            _shownProductRepository = shownProductRepository;
        }

        // GET: api/HomePage/New
        [HttpGet("New")]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetNewProducts()
        {
            return await GetProductsAsync(p => p.IsNew, 10, "New products not found");
        }

        // GET: api/HomePage/Discounts
        [HttpGet("Discounts")]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetDiscountedProducts()
        {
            return await GetProductsAsync(p => p.Discount > 0, 10, "Discounted products not found");
        }

        // GET: api/HomePage/Popular
        [HttpGet("Popular")]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetPopularProducts()
        {
            return await GetProductsAsync(p => p.IsHit, 10, "Popular products not found", orderBy: p => p.OrderByDescending(p => p.Rating));
        }

        // GET: api/HomePage/PopularByCategory/5
        [HttpGet("PopularByCategory/{categoryId}")]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetPopularProductsByCategory(int categoryId)
        {
            return await GetProductsAsync(p => p.Categories.Any(c => c.CategoryId == categoryId), 10, $"No popular products found for category {categoryId}", orderBy: p => p.OrderByDescending(p => p.Rating));
        }

        private async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetProductsAsync(
            Expression<Func<Product, bool>> filter,
            int take,
            string notFoundMessage,
            Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy = null)
        {
            var query = _shownProductRepository.GetProducts();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = query.OrderByDescending(p => p.InStock);

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            var products = await query
                .Take(take)
                .Select(p => new ProductResponseDto
                {
                    Id = p.ProductId,
                    Name = p.Name,
                    NameEng = p.NameEng,
                    Amount = p.AvailableAmount,
                    Images = p.Images,
                    Brand = p.Brand,
                    Price = p.Price,
                    Discount = p.Discount,
                    IsNew = p.IsNew,
                    Rating = p.Rating,
                    NumberOfReviews = p.NumberOfReviews,
                    NumberOfPurchases = p.NumberOfPurchases,
                    NumberOfViews = p.NumberOfViews,
                    Article = p.Article,
                    Categories = p.Categories.Select(c => new CategoryResponseDto { CategoryId = c.CategoryId, Name = c.Name, ParentCategoryId = c.ParentCategoryId }).ToList(),
                    DieNumbers = p.DieNumbers,
                    Reviews = p.Reviews.Select(r => new ReviewDto
                    {
                        Name = r.Name,
                        Review = r.ReviewText,
                        Rating = r.Rating,
                        Date = r.Date.ToString("dd.MM.yyyy")
                    }).ToList(),
                    Characteristics = p.Characteristics.Select(c => new CharacteristicDto
                    {
                        Title = c.Title,
                        Desc = c.Desc
                    }).ToList(),
                    Description = p.Description,
                    InStock = p.InStock
                })
                .ToListAsync();

            if (products == null || products.Count == 0)
            {
                return NotFound(notFoundMessage);
            }

            return Ok(products);
        }
    }
}
