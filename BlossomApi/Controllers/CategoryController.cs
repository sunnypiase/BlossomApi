using BlossomApi.DB;
using BlossomApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlossomApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlossomApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly BlossomContext _context;

        public CategoryController(BlossomContext context)
        {
            _context = context;
        }

        // GET: api/Category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetCategories()
        {
            return await _context.Categories
                .Select(c => new CategoryResponseDto
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name,
                    ParentCategoryId = c.ParentCategoryId
                })
                .ToListAsync();
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryResponseDto>> GetCategory(int id)
        {
            var category = await _context.Categories
                .Select(c => new CategoryResponseDto
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name,
                    ParentCategoryId = c.ParentCategoryId
                })
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        // PUT: api/Category/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, CategoryCreateDto categoryDto)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            category.Name = categoryDto.Name;
            category.ParentCategoryId = categoryDto.ParentCategoryId;

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Category
        [HttpPost]
        public async Task<ActionResult<CategoryResponseDto>> PostCategory(CategoryCreateDto categoryDto)
        {
            var category = new Category
            {
                Name = categoryDto.Name,
                ParentCategoryId = categoryDto.ParentCategoryId
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var categoryResponse = new CategoryResponseDto
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                ParentCategoryId = category.ParentCategoryId
            };

            return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryId }, categoryResponse);
        }

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }

        // GET: api/Category/GetCategoryTree
        [HttpGet("GetCategoryTree")]
        public async Task<ActionResult<CategoryNode>> GetCategoryTree(string categoryName)
        {
            var allCategories = await _context.Categories.ToListAsync();

            var rootCategory = allCategories.FirstOrDefault(c => c.Name == categoryName);
            if (rootCategory == null)
            {
                return NotFound("Category not found");
            }

            var categoryTree = BuildCategoryTree(rootCategory, allCategories);

            return Ok(categoryTree);
        }

        private CategoryNode BuildCategoryTree(Category root, List<Category> allCategories)
        {
            var rootNode = new CategoryNode
            {
                CategoryId = root.CategoryId,
                Name = root.Name,
                Children = new List<CategoryNode>()
            };

            var childCategories = allCategories.Where(c => c.ParentCategoryId == root.CategoryId).ToList();

            foreach (var child in childCategories)
            {
                rootNode.Children.Add(BuildCategoryTree(child, allCategories));
            }

            return rootNode;
        }
    }

    public class CategoryNode
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public List<CategoryNode> Children { get; set; }
    }
}
