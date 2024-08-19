using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlossomApi.DB;
using BlossomApi.Models;
using BlossomApi.Dtos;
using BlossomApi.Services;
using System.Linq.Expressions;
using System.Linq;

namespace BlossomApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminProductController : ControllerBase
    {
        private readonly BlossomContext _context;
        private readonly ProductQueryService _productQueryService;

        public AdminProductController(BlossomContext context, ProductQueryService productQueryService)
        {
            _context = context;
            _productQueryService = productQueryService;
        }

        // POST: api/AdminProduct/ToggleIsNew/{id}
        [HttpPost("ToggleIsNew/{id}")]
        public async Task<IActionResult> ToggleIsNew(int id, [FromBody] bool isNew)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            product.IsNew = isNew;
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { Message = $"Product 'IsNew' status updated to {isNew}." });
        }

        // POST: api/AdminProduct/ToggleIsHit/{id}
        [HttpPost("ToggleIsHit/{id}")]
        public async Task<IActionResult> ToggleIsHit(int id, [FromBody] bool isHit)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            product.IsHit = isHit;
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { Message = $"Product 'IsHit' status updated to {isHit}." });
        }

        // POST: api/AdminProduct/ToggleIsShown/{id}
        [HttpPost("ToggleIsShown/{id}")]
        public async Task<IActionResult> ToggleIsShown(int id, [FromBody] bool isShown)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            product.IsShown = isShown;
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { Message = $"Product 'IsShown' status updated to {isShown}." });
        }

        // POST: api/AdminProduct/GetProductsByAdminFilter
        [HttpPost("GetProductsByAdminFilter")]
        public async Task<ActionResult<GetProductsByFilterResponse>> GetProductsByAdminFilter(GetProductsByAdminFilterRequestDto request)
        {
            var query = await _productQueryService.ApplyFilterAndSortAsync(request);

            var totalCount = await query.CountAsync();

            var products = await query
                .Skip(request.Start)
                .Take(request.Amount)
                .Select(p => MapToProductResponseDto(p))
                .ToListAsync();

            var response = new GetProductsByFilterResponse
            {
                Products = products,
                TotalCount = totalCount
            };

            return Ok(response);
        }

        private static ProductResponseDto MapToProductResponseDto(Product p)
        {
            return new ProductResponseDto
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
            };
        }
    }
}
public class GetProductsByAdminFilterRequestDto
{
    public string? SearchTerm { get; set; }

    public int? CategoryId { get; set; }
    public List<int>? SelectedCharacteristics { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public bool? IsShown { get; set; }
    public bool? IsHit { get; set; }
    public bool? HasDiscount { get; set; }

    public string? SortOption { get; set; }

    public int Start { get; set; }
    public int Amount { get; set; }
}

