using BlossomApi.DB;
using BlossomApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlossomApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomePageController : ControllerBase
    {
        private readonly BlossomContext _context;

        public HomePageController(BlossomContext context)
        {
            _context = context;
        }

        // GET: api/HomePage/New
        [HttpGet("New")]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetNewProducts()
        {
            var products = await _context.Products
                .Where(p => p.IsNew)
                .Take(10)
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
                    Categories = p.Categories.Select(c => new CategoryResponseDto() { CategoryId = c.CategoryId, Name = c.Name, ParentCategoryId = c.ParentCategoryId }).ToList(),
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

            return Ok(products);
        }

        // GET: api/HomePage/Discounts
        [HttpGet("Discounts")]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetDiscountedProducts()
        {
            var products = await _context.Products
                .Where(p => p.Discount > 0)
                .Take(10)
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
                    Categories = p.Categories.Select(c => new CategoryResponseDto() { CategoryId = c.CategoryId, Name = c.Name, ParentCategoryId = c.ParentCategoryId }).ToList(),
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

            return Ok(products);
        }

        // GET: api/HomePage/Popular
        [HttpGet("Popular")]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetPopularProducts()
        {
            var products = await _context.Products
                .OrderByDescending(p => p.Rating)
                .Take(10)
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
                    Categories = p.Categories.Select(c => new CategoryResponseDto() { CategoryId = c.CategoryId, Name = c.Name, ParentCategoryId = c.ParentCategoryId }).ToList(),
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

            return Ok(products);
        }
        // GET: api/HomePage/PopularByCategory/5
        [HttpGet("PopularByCategory/{categoryId}")]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetPopularProductsByCategory(int categoryId)
        {
            var products = await _context.Products
                .Include(x => x.Categories)
                .OrderByDescending(p => p.Rating)
                .Where(x => x.Categories.Any(c => c.CategoryId == categoryId))
                .Take(10)
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
                    Categories = p.Categories.Select(c => new CategoryResponseDto() { CategoryId = c.CategoryId, Name = c.Name, ParentCategoryId = c.ParentCategoryId }).ToList(),
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

            return Ok(products);
        }
    }
}