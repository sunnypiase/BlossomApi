using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlossomApi.DB;
using BlossomApi.Repositories;

namespace BlossomApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly BlossomContext _context;
        private readonly IShownProductRepository _shownProductRepository;

        public SearchController(BlossomContext context, IShownProductRepository shownProductRepository)
        {
            _context = context;
            _shownProductRepository = shownProductRepository;
        }

        [HttpGet("products")]
        public async Task<IActionResult> SearchProducts(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return BadRequest("Query string cannot be empty.");
            }

            var normalizedQuery = query.ToLower();

            var result = await _shownProductRepository.GetProducts()
                .Where(p => p.Name.ToLower().Contains(normalizedQuery) || p.NameEng.ToLower().Contains(normalizedQuery))
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Rating)
                .Take(10)
                .Select(p => new ProductSearchResultDto
                {
                    Id = p.ProductId,
                    Name = p.Name,
                    Image = p.Images != null && p.Images.Any() ? p.Images.FirstOrDefault() : null
                })
                .ToListAsync();

            return Ok(result);
        }
    }

    public class ProductSearchResultDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
    }
}
