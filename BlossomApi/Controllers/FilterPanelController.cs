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

        // GET: api/FilterPanel/{categoryName}
        [HttpGet("{categoryName}")]
        public async Task<ActionResult<FilterPanelResponseDto>> GetFilterPanel(string categoryName)
        {
            var filterPanelData = await FetchFilterPanelData(categoryName);
            if (filterPanelData == null)
            {
                return NotFound();
            }
            return Ok(filterPanelData);
        }

        private async Task<FilterPanelResponseDto> FetchFilterPanelData(string categoryName)
        {
            categoryName = categoryName.ToLower();

            var rootCategory = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name.ToLower() == categoryName);

            if (rootCategory == null)
            {
                return null;
            }

            var categoryTree = await _categoryService.GetCategoryTreeAsync(categoryName);
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
                Categories = await _categoryService.GetCategoryTreeAsync(categoryName),
                Characteristics = characteristics,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
            };
        }
    }
}
