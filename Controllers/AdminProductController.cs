using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlossomApi.DB;
using BlossomApi.Models;
using BlossomApi.Dtos;
using BlossomApi.Services;
using System.Linq.Expressions;
using System.Linq;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using BlossomApi.Dtos.Product;

namespace BlossomApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminProductController : ControllerBase
    {
        private readonly BlossomContext _context;
        private readonly ProductQueryService _productQueryService;
        private readonly IMapper _mapper;

        public AdminProductController(
            BlossomContext context,
            ProductQueryService productQueryService,
            IMapper mapper)
        {
            _context = context;
            _productQueryService = productQueryService;
            _mapper = mapper;
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
        public async Task<IActionResult> GetProductsByAdminFilter(GetProductsByAdminFilterRequestDto request)
        {
            var query = await _productQueryService.ApplyFilterAndSortAsync(request);

            var totalCount = await query.CountAsync();

            var products = await query
                .Skip(request.Start)
                .Take(request.Amount)
                .ToListAsync();

            var productDtos = _mapper.Map<List<ProductResponseDto>>(products);

            var response = new
            {
                Products = productDtos,
                TotalCount = totalCount
            };

            return Ok(response);
        }
    }
}
public class GetProductsByAdminFilterRequestDto
{
    public string? SearchTerm { get; set; }

    public List<int>? CategoryIds { get; set; }
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

