using BlossomApi.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlossomApi.Dtos.FilterPanel;
using BlossomApi.Models;

namespace BlossomApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilterPanelController : ControllerBase
    {
        private readonly BlossomContext _context;

        public FilterPanelController(BlossomContext context)
        {
            _context = context;
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

        // POST: api/FilterPanel/GetProductCountByFilters
        [HttpPost("GetProductCountByFilters")]
        public async Task<ActionResult<ProductCountResponseDto>> GetProductCountByFilters(FilterRequestDto request)
        {
            var totalProductCount = await CountProductsByFilters(request);
            return Ok(new ProductCountResponseDto { TotalProductCount = totalProductCount });
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

            var allCategories = new List<Category> { rootCategory };
            allCategories.AddRange(await GetAllChildCategoriesAsync(rootCategory.CategoryId));

            var categoryNames = allCategories.Select(c => c.Name).ToList();

            var products = await _context.Products
                .Where(p => p.Categories.Any(c => categoryNames.Contains(c.Name)))
                .ToListAsync();

            var characteristics = products
                .SelectMany(p => p.Characteristics)
                .GroupBy(c => c.Title)
                .Select(g => new FilterPanelCharacteristicDto
                {
                    CharacteristicName = g.Key,
                    Options = g.GroupBy(c => c.Desc)
                        .Select(og => new FilterPanelOptionDto
                        {
                            Option = og.Key,
                            ProductsAmount = og.Count()
                        })
                        .ToList()
                })
                .ToList();

            var minPrice = products.Min(p => p.Price);
            var maxPrice = products.Max(p => p.Price);

            return new FilterPanelResponseDto
            {
                Categories = categoryNames,
                Characteristics = characteristics,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
            };
        }

        private async Task<List<Category>> GetAllChildCategoriesAsync(int parentId)
        {
            var categories = await _context.Categories
                .Where(c => c.ParentCategoryId == parentId)
                .ToListAsync();

            var allChildCategories = new List<Category>();

            foreach (var category in categories)
            {
                allChildCategories.Add(category);
                allChildCategories.AddRange(await GetAllChildCategoriesAsync(category.CategoryId));
            }

            return allChildCategories;
        }

        private async Task<int> CountProductsByFilters(FilterRequestDto request)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(request.CategoryName))
            {
                var rootCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name.ToLower() == request.CategoryName.ToLower());

                if (rootCategory != null)
                {
                    var allCategories = new List<Category> { rootCategory };
                    allCategories.AddRange(await GetAllChildCategoriesAsync(rootCategory.CategoryId));

                    var categoryNames = allCategories.Select(c => c.Name).ToList();
                    query = query.Where(p => p.Categories.Any(c => categoryNames.Contains(c.Name)));
                }
            }

            if (request.SelectedCharacteristics != null && request.SelectedCharacteristics.Any())
            {
                foreach (var characteristic in request.SelectedCharacteristics)
                {
                    query = query.Where(p => p.Characteristics.Any(c => c.Desc == characteristic));
                }
            }

            if (request.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= request.MinPrice.Value);
            }

            if (request.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= request.MaxPrice.Value);
            }

            return await query.CountAsync();
        }
    }
}
