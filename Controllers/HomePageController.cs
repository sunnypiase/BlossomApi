using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        private readonly IMapper _mapper;

        public HomePageController(BlossomContext context, IShownProductRepository shownProductRepository, IMapper mapper)
        {
            _context = context;
            _shownProductRepository = shownProductRepository;
            _mapper = mapper;
        }

        [HttpGet("New")]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetNewProducts()
        {
            return await GetProductsAsync(p => p.IsNew, 10, "New products not found");
        }

        [HttpGet("Discounts")]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetDiscountedProducts()
        {
            return await GetProductsAsync(p => p.Discount > 0, 10, "Discounted products not found");
        }

        [HttpGet("Popular")]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetPopularProducts()
        {
            return await GetProductsAsync(p => p.IsHit, 10, "Popular products not found", orderBy: p => p.OrderByDescending(p => p.Rating));
        }

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
                .ProjectTo<ProductResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            if (products == null || products.Count == 0)
            {
                return NotFound(notFoundMessage);
            }

            return Ok(products);
        }
    }
}
