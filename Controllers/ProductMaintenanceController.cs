using BlossomApi.DB;
using BlossomApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlossomApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductMaintenanceController : ControllerBase
    {
        private readonly BlossomContext _context;

        public ProductMaintenanceController(BlossomContext context)
        {
            _context = context;
        }

        // GET: api/ProductMaintenance/FixMissingMainCategories
        [HttpGet("FixMissingMainCategories")]
        public async Task<IActionResult> FixMissingMainCategories()
        {
            var products = await _context.Products
                .Include(p => p.Categories)
                .Where(p => (p.MainCategoryId == null || p.MainCategoryId == 0) && p.Categories.Any())
                .ToListAsync();

            foreach (var product in products)
            {
                var firstAdditionalCategory = product.AdditionalCategories.FirstOrDefault();
                if (firstAdditionalCategory != null)
                {
                    product.MainCategory = firstAdditionalCategory;
                }
                else
                {
                    // If there are no additional categories, you may want to handle this scenario
                    // For now, we just skip these products
                    continue;
                }

                _context.Entry(product).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = $"Fixed {products.Count} products by assigning a main category from additional categories."
            });
        }
    }
}
