using BlossomApi.DB;
using BlossomApi.Dtos;
using BlossomApi.Models;
using BlossomApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlossomApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly BlossomContext _context;
        private readonly CategoryService _categoryService;

        public CategoryController(BlossomContext context, CategoryService categoryService)
        {
            _context = context;
            _categoryService = categoryService;
        }

        // GET: api/Category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetCategories()
        {
            // var currentUser = _userManager.GetUserId(User);
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

        // GET: api/Category/Search
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> SearchCategories([FromQuery] string? searchTerm)
        {
            var categoriesQuery = _context.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var lowerSearchTerm = searchTerm.ToLower();
                categoriesQuery = categoriesQuery.Where(c => c.Name.ToLower().Contains(lowerSearchTerm));
            }

            var categories = await categoriesQuery
                .Select(c => new CategoryResponseDto
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name,
                    ParentCategoryId = c.ParentCategoryId
                })
                .ToListAsync();

            return Ok(categories);
        }
        
        // GET: api/Category/Search
        [HttpGet("SearchAdditional")]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> SearchAdditionalCategories([FromQuery] string? searchTerm, int mainCategoryId)
        {
            var categoryIds = (await _categoryService.GetAllChildCategoriesAsync(mainCategoryId)).Select(x => x.CategoryId);
            var categoriesQuery = _context.Categories.AsQueryable().Where(x => categoryIds.Contains(x.CategoryId));

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var lowerSearchTerm = searchTerm.ToLower();
                categoriesQuery = categoriesQuery.Where(c => c.Name.ToLower().Contains(lowerSearchTerm));
            }

            var categories = await categoriesQuery
                .Select(c => new CategoryResponseDto
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name,
                    ParentCategoryId = c.ParentCategoryId
                })
                .ToListAsync();

            return Ok(categories);
        }

        // GET: api/Category/Search
        [HttpGet("SearchMain")]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> SearchMainCategories([FromQuery] string? searchTerm)
        {
            var categoriesQuery = _context.Categories.AsQueryable().Where(x => x.ParentCategoryId == 0);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var lowerSearchTerm = searchTerm.ToLower();
                categoriesQuery = categoriesQuery.Where(c => c.Name.ToLower().Contains(lowerSearchTerm));
            }

            var categories = await categoriesQuery
                .Select(c => new CategoryResponseDto
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name,
                    ParentCategoryId = c.ParentCategoryId
                })
                .ToListAsync();

            return Ok(categories);
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
        public async Task<ActionResult<CategoryNode>> GetCategoryTree(int categoryId)
        {
            var categoryTree = await _categoryService.GetCategoryTreeAsync(categoryId);
            if (categoryTree == null)
            {
                return NotFound("Category not found");
            }

            return Ok(categoryTree);
        }
    }
}
