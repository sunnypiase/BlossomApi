using BlossomApi.DB;
using BlossomApi.Dtos;
using BlossomApi.Models;
using BlossomApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlossomApi.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminCategoryController : ControllerBase
    {
        private readonly BlossomContext _context;
        private readonly CategoryService _service;

        public AdminCategoryController(BlossomContext context, CategoryService service)
        {
            _context = context;
            _service = service;
        }

        // GET: api/admin/Category/tree
        [HttpGet("tree")]
        public async Task<ActionResult<List<CategoryNode>>> GetTree()
        {
            var all = await _context.Categories.ToListAsync();
            var forest = _service.BuildCategoryForestFromList(all);
            return Ok(forest);
        }

        // GET: api/admin/Category/parents
        [HttpGet("parents")]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetParents()
        {
            var parents = await _context.Categories
                .Where(c => c.ParentCategoryId == 0)
                .Select(c => new CategoryResponseDto
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name,
                    ParentCategoryId = c.ParentCategoryId
                })
                .ToListAsync();
            return Ok(parents);
        }

        // GET: api/admin/Category/{parentId}/children
        [HttpGet("{parentId}/children")]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetChildren(int parentId)
        {
            if (!await _context.Categories.AnyAsync(c => c.CategoryId == parentId))
                return NotFound($"Parent category {parentId} not found.");

            var children = await _context.Categories
                .Where(c => c.ParentCategoryId == parentId)
                .Select(c => new CategoryResponseDto
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name,
                    ParentCategoryId = c.ParentCategoryId
                })
                .ToListAsync();
            return Ok(children);
        }

        // POST: api/admin/Category
        [HttpPost]
        public async Task<ActionResult<CategoryResponseDto>> Create([FromBody] CategoryCreateDto dto)
        {
            // must have a parent, and parent must exist
            if (dto.ParentCategoryId == 0)
                return BadRequest("Cannot create a top-level category in admin.");
            var parent = await _context.Categories.FindAsync(dto.ParentCategoryId);
            if (parent == null) return NotFound("Parent not found.");

            // depth check: parent at level 1 (parent.ParentCategoryId=0) ⇒ new is level 2 OK
            //              parent at level 2 ⇒ new is level 3 OK
            //              parent at level 3 ⇒ new would be level 4 ❌
            var grandParent = parent.ParentCategoryId == 0
                ? null
                : await _context.Categories.FindAsync(parent.ParentCategoryId);
            if (grandParent?.ParentCategoryId != 0 && parent.ParentCategoryId != 0)
                return BadRequest("Cannot exceed 3 levels of nesting.");

            var cat = new Category
            {
                Name = dto.Name,
                ParentCategoryId = dto.ParentCategoryId
            };
            _context.Categories.Add(cat);
            await _context.SaveChangesAsync();

            var resp = new CategoryResponseDto
            {
                CategoryId = cat.CategoryId,
                Name = cat.Name,
                ParentCategoryId = cat.ParentCategoryId
            };
            return CreatedAtAction(
                nameof(GetChildren),
                new { parentId = resp.ParentCategoryId },
                resp
            );
        }

        // PUT: api/admin/Category/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryCreateDto dto)
        {
            var cat = await _context.Categories.FindAsync(id);
            if (cat == null) return NotFound();
            if (cat.ParentCategoryId == 0)
                return BadRequest("Cannot edit a top-level category.");

            // validate new parent
            if (dto.ParentCategoryId == 0)
                return BadRequest("Must have a non-zero parent.");
            var newParent = await _context.Categories.FindAsync(dto.ParentCategoryId);
            if (newParent == null) return NotFound("New parent not found.");

            // depth check (same logic as create)
            var gp = newParent.ParentCategoryId == 0
                ? null
                : await _context.Categories.FindAsync(newParent.ParentCategoryId);
            if (gp?.ParentCategoryId != 0 && newParent.ParentCategoryId != 0)
                return BadRequest("Cannot exceed 3 levels of nesting.");

            cat.Name = dto.Name;
            cat.ParentCategoryId = dto.ParentCategoryId;
            _context.Entry(cat).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/admin/Category/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cat = await _context.Categories.FindAsync(id);
            if (cat == null) return NotFound();
            if (cat.ParentCategoryId == 0)
                return BadRequest("Cannot delete a top-level category.");

            // optional: prevent deleting if has children
            bool hasKids = await _context.Categories.AnyAsync(c => c.ParentCategoryId == id);
            if (hasKids)
                return BadRequest("Please delete child categories first.");

            _context.Categories.Remove(cat);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
