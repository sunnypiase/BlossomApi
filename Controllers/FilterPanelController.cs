using BlossomApi.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlossomApi.Dtos.FilterPanel;
using BlossomApi.Services;

namespace BlossomApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilterPanelController : ControllerBase
    {
        private readonly BlossomContext _context;
        private readonly CategoryService _categoryService;

        public FilterPanelController(BlossomContext context, CategoryService categoryService)
        {
            _context = context;
            _categoryService = categoryService;
        }

        // GET: api/FilterPanel/{categoryId}
        [HttpGet("{categoryId}")]
        public async Task<ActionResult<FilterPanelResponseDto>> GetFilterPanel(int categoryId)
        {
            var filterPanelData = await FetchFilterPanelData(categoryId);
            if (filterPanelData == null)
            {
                return NotFound();
            }
            return Ok(filterPanelData);
        }

        private async Task<FilterPanelResponseDto> FetchFilterPanelData(int categoryId)
        {

            var rootCategory = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId);

            if (rootCategory == null)
            {
                return null;
            }

            var categoryTree = await _categoryService.GetCategoryTreeAsync(categoryId);
            var categoryNames = _categoryService.GetAllCategoryNames(categoryTree);

            var products = await _context.Products
                .Include(p => p.Characteristics)
                .Include(p => p.Categories)
                .Where(p => p.Categories.Any(c => categoryNames.Contains(c.Name.ToLower())))
                .ToListAsync();

            var characteristics = products
                .SelectMany(p => p.Characteristics)
                .GroupBy(c => c.Title)
                .Select(g => new FilterPanelCharacteristicDto
                {
                    CharacteristicName = g.Key,
                    Options = g.GroupBy(c => new { c.CharacteristicId, c.Desc })
                        .Select(og => new FilterPanelOptionDto
                        {
                            Id = og.Key.CharacteristicId,
                            Option = og.Key.Desc,
                            ProductsAmount = products.Count(p => p.Characteristics.Any(c => c.CharacteristicId == og.Key.CharacteristicId))
                        })
                        .ToList()
                })
                .ToList();

            var minPrice = products.Min(p => p.Price);
            var maxPrice = products.Max(p => p.Price);

            return new FilterPanelResponseDto
            {
                Categories = categoryTree,
                Characteristics = characteristics,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
            };
        }
    }
}
