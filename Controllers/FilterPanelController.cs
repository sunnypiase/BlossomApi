using BlossomApi.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlossomApi.Dtos.FilterPanel;
using BlossomApi.Services;
using System.Linq;
using System.Threading.Tasks;
using BlossomApi.Repositories;
using BlossomApi.Models;
using BlossomApi.Dtos.Banners;

namespace BlossomApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilterPanelController : ControllerBase
    {
        private readonly BlossomContext _context;
        private readonly CategoryService _categoryService;
        private readonly IShownProductRepository _shownProductRepository;

        public FilterPanelController(
            BlossomContext context,
            CategoryService categoryService,
            IShownProductRepository shownProductRepository)
        {
            _context = context;
            _categoryService = categoryService;
            _shownProductRepository = shownProductRepository;
        }

        // GET: api/FilterPanel/{categoryId}
        [HttpGet("{categoryId}")]
        public async Task<ActionResult<FilterPanelResponseDto>> GetFilterPanel(int categoryId)
        {
            var filterPanelData = await FetchFilterPanelDataByCategory(categoryId);
            if (filterPanelData == null)
            {
                return NotFound();
            }
            return Ok(filterPanelData);
        }

        // GET: api/FilterPanel/Banner/{bannerId}
        [HttpGet("Banner/{bannerId}")]
        public async Task<ActionResult<BannerFilterPanelResponseDto>> GetFilterPanelByBanner(int bannerId)
        {
            var filterPanelData = await FetchFilterPanelDataByBanner(bannerId);
            if (filterPanelData == null)
            {
                return NotFound();
            }
            return Ok(filterPanelData);
        }

        private async Task<BannerFilterPanelResponseDto> FetchFilterPanelDataByBanner(int bannerId)
        {
            var banner = await _context.Banners
                .Include(b => b.Products)
                .ThenInclude(p => p.Characteristics)
                .Include(b => b.Products)
                .ThenInclude(p => p.Categories)
                .FirstOrDefaultAsync(b => b.BannerId == bannerId);

            if (banner == null || !banner.Products.Any())
            {
                return null;
            }

            var categoryIds = banner.Products
                .Where(p => p.IsShown)
                .SelectMany(p => p.Categories)
                .Select(c => c.CategoryId)
                .Distinct()
                .ToList();

            var categories = await _context.Categories
                .Where(c => categoryIds.Contains(c.CategoryId))
                .ToListAsync();

            var categoryTree = _categoryService.BuildCategoryForestFromList(categories);

            var response = GenerateFilterPanelResponse(banner.Products.Where(x => x.IsShown).ToList());
            return new BannerFilterPanelResponseDto
            {
                Categories = categoryTree,
                Characteristics = response.Characteristics,
                MinPrice = response.MinPrice,
                MaxPrice = response.MaxPrice,
            };
        }

        private async Task<FilterPanelResponseDto> FetchFilterPanelDataByCategory(int categoryId)
        {
            var rootCategory = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId);

            if (rootCategory == null)
            {
                return null;
            }

            var categoryTree = await _categoryService.GetCategoryTreeAsync(categoryId);
            var categoryIds = _categoryService.GetAllCategoryIds(categoryTree);

            var products = await _shownProductRepository.GetProducts()
                .Include(p => p.Characteristics)
                .Include(p => p.Categories)
                .Where(p => p.Categories.Any(c => categoryIds.Contains(c.CategoryId)))
                .ToListAsync();

            if (products == null || !products.Any())
            {
                return new FilterPanelResponseDto
                {
                    Categories = categoryTree,
                    Characteristics = new List<FilterPanelCharacteristicDto>()
                };
            }

            var response = GenerateFilterPanelResponse(products);
            response.Categories = categoryTree;

            return response;
        }

        private FilterPanelResponseDto GenerateFilterPanelResponse(List<Product> products)
        {
            if(products == null || !products.Any())
            {
                return new FilterPanelResponseDto
                {
                    Characteristics = new List<FilterPanelCharacteristicDto>()
                };
            }
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
                Characteristics = characteristics,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
            };
        }
    }
}
