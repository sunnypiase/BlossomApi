using BlossomApi.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlossomApi.Dtos.FilterPanel;
using BlossomApi.Dtos;
using BlossomApi.Models;

namespace BlossomApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminFilterPanelController : ControllerBase
    {
        private readonly BlossomContext _context;

        public AdminFilterPanelController(BlossomContext context)
        {
            _context = context;
        }

        // GET: api/AdminFilterPanel
        [HttpGet]
        public async Task<ActionResult<AdminFilterPanelResponseDto>> GetAdminFilterPanel()
        {
            var filterPanelData = await FetchAdminFilterPanelData();
            if (filterPanelData == null)
            {
                return NotFound("No data available for filter panel");
            }
            return Ok(filterPanelData);
        }

        private async Task<AdminFilterPanelResponseDto> FetchAdminFilterPanelData()
        {
            // Fetch all categories and build the category tree
            var categories = await _context.Categories.ToListAsync();
            if (categories == null || !categories.Any())
            {
                return new AdminFilterPanelResponseDto
                {
                    Categories = new List<CategoryNode>(),
                    Brands = new List<FilterPanelOptionDto>(),
                    Characteristics = new List<FilterPanelCharacteristicDto>(),
                    MinPrice = 0,
                    MaxPrice = 0
                };
            }

            var categoryTree = BuildCategoryTree(categories);

            // Fetch all products
            var products = await _context.Products
                .Include(p => p.Characteristics)
                .Include(p => p.Categories)
                .Include(p => p.Brand)
                .ToListAsync();

            if (products == null || !products.Any())
            {
                return new AdminFilterPanelResponseDto
                {
                    Categories = categoryTree,
                    Brands = new List<FilterPanelOptionDto>(),
                    Characteristics = new List<FilterPanelCharacteristicDto>(),
                    MinPrice = 0,
                    MaxPrice = 0
                };
            }

            // Group characteristics by title and create options
            var characteristics = products
                .Where(p => p.Characteristics != null)
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

            // Group brands by title
            var brands = products
                .Where(p => p.Brand != null && !string.IsNullOrEmpty(p.Brand.Title)) // Ensure that Brand and Title are not null
                .Select(p => p.Brand)
                .DistinctBy(b => b.BrandId)
                .Select(g => new FilterPanelOptionDto
                {
                    Id = g.BrandId,
                    Option = g.Title ?? "Unknown", // Default title if null
                    ProductsAmount = products.Count(p => p.Brand != null && p.Brand.Title == g.Title)
                })
                .ToList();

            // Determine the minimum and maximum price across all products
            var minPrice = products.Any() ? products.Min(p => p.Price) : 0;
            var maxPrice = products.Any() ? products.Max(p => p.Price) : 0;

            // Return the filter panel response
            return new AdminFilterPanelResponseDto
            {
                Brands = brands,
                Categories = categoryTree,
                Characteristics = characteristics,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
            };
        }

        private List<CategoryNode> BuildCategoryTree(List<Category> categories)
        {
            if (categories == null || !categories.Any())
            {
                return new List<CategoryNode>();
            }

            var categoryDictionary = categories.ToDictionary(c => c.CategoryId, c => new CategoryNode
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                Children = new List<CategoryNode>()
            });

            foreach (var category in categories)
            {
                if (category.ParentCategoryId == 0)
                {
                    continue;
                }

                if (categoryDictionary.TryGetValue(category.ParentCategoryId, out var parentCategoryNode))
                {
                    parentCategoryNode.Children.Add(categoryDictionary[category.CategoryId]);
                }
            }

            return categoryDictionary.Values
                .Where(c => categories.Any(cat => cat.ParentCategoryId == 0 && cat.CategoryId == c.CategoryId))
                .ToList();
        }
    }

    public class AdminFilterPanelResponseDto
    {
        public List<CategoryNode> Categories { get; set; } = new List<CategoryNode>(); // List of root-level categories
        public List<FilterPanelOptionDto> Brands { get; set; } = new List<FilterPanelOptionDto>(); // List of brands
        public List<FilterPanelCharacteristicDto> Characteristics { get; set; } = new List<FilterPanelCharacteristicDto>(); // Characteristics
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }
}
