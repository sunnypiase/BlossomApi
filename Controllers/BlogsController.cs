using AutoMapper;
using BlossomApi.DB;
using BlossomApi.Dtos;
using BlossomApi.Dtos.Blogs;
using BlossomApi.Models;
using BlossomApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlossomApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly BlossomContext _context;
        private readonly IMapper _mapper;
        private readonly ImageService _imageService;

        public BlogsController(BlossomContext context, IMapper mapper, ImageService imageService)
        {
            _context = context;
            _mapper = mapper;
            _imageService = imageService;
        }

        // POST: api/Blogs
        [HttpPost]
        public async Task<ActionResult<BlogResponseDto>> CreateBlog([FromForm] BlogCreateDto blogDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var blog = new Blog
            {
                Title = blogDto.Title,
                Description = blogDto.Description,
                MetaKeywords = blogDto.MetaKeywords,
                MetaDescription = blogDto.MetaDescription,
                DesktopAltText = blogDto.DesktopAltText,
                LaptopAltText = blogDto.LaptopAltText,
                TabletAltText = blogDto.TabletAltText,
                PhoneAltText = blogDto.PhoneAltText
            };

            // Upload images
            blog.DesktopImageUrl = await UploadImageAsync(blogDto.DesktopImage);
            blog.LaptopImageUrl = await UploadImageAsync(blogDto.LaptopImage);
            blog.TabletImageUrl = await UploadImageAsync(blogDto.TabletImage);
            blog.PhoneImageUrl = await UploadImageAsync(blogDto.PhoneImage);

            // Fetch associated products
            if (blogDto.ProductIds.Any())
            {
                var products = await _context.Products
                    .Where(p => blogDto.ProductIds.Contains(p.ProductId))
                    .ToListAsync();

                blog.Products = products;
            }

            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();

            var blogResponse = _mapper.Map<BlogResponseDto>(blog);

            return CreatedAtAction(nameof(GetBlog), new { id = blog.BlogId }, blogResponse);
        }

        // GET: api/Blogs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogResponseDto>>> GetBlogs()
        {
            var blogs = await _context.Blogs.ToListAsync();
            var blogDtos = _mapper.Map<List<BlogResponseDto>>(blogs);
            return Ok(blogDtos);
        }

        // GET: api/Blogs/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<BlogWithProductsDto>> GetBlog(int id)
        {
            var blog = await _context.Blogs
                .Include(b => b.Products)
                .FirstOrDefaultAsync(b => b.BlogId == id);

            if (blog == null)
            {
                return NotFound();
            }

            var blogResponse = _mapper.Map<BlogWithProductsDto>(blog);
            return Ok(blogResponse);
        }

        // DELETE: api/Blogs/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }

            // Delete associated images
            await _imageService.DeleteImageAsync(blog.DesktopImageUrl);
            await _imageService.DeleteImageAsync(blog.LaptopImageUrl);
            await _imageService.DeleteImageAsync(blog.TabletImageUrl);
            await _imageService.DeleteImageAsync(blog.PhoneImageUrl);

            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // PUT: api/Blogs/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlog(int id, [FromForm] BlogUpdateDto blogUpdateDto)
        {
            var blog = await _context.Blogs.Include(b => b.Products).FirstOrDefaultAsync(b => b.BlogId == id);
            if (blog == null)
            {
                return NotFound();
            }

            if (blogUpdateDto.Title != null)
            {
                blog.Title = blogUpdateDto.Title;
            }

            if (blogUpdateDto.Description != null)
            {
                blog.Description = blogUpdateDto.Description;
            }

            if (blogUpdateDto.MetaKeywords != null)
            {
                blog.MetaKeywords = blogUpdateDto.MetaKeywords;
            }

            if (blogUpdateDto.MetaDescription != null)
            {
                blog.MetaDescription = blogUpdateDto.MetaDescription;
            }

            if (blogUpdateDto.DesktopAltText != null)
            {
                blog.DesktopAltText = blogUpdateDto.DesktopAltText;
            }

            if (blogUpdateDto.LaptopAltText != null)
            {
                blog.LaptopAltText = blogUpdateDto.LaptopAltText;
            }

            if (blogUpdateDto.TabletAltText != null)
            {
                blog.TabletAltText = blogUpdateDto.TabletAltText;
            }

            if (blogUpdateDto.PhoneAltText != null)
            {
                blog.PhoneAltText = blogUpdateDto.PhoneAltText;
            }

            // Update product associations
            if (blogUpdateDto.ProductIds != null && blogUpdateDto.ProductIds.Count != 0)
            {
                // Get the products currently associated with the blog
                var currentProductIds = blog.Products.Select(p => p.ProductId).ToList();

                // Find the products to remove (those that are in the current collection but not in the update list)
                var productsToRemove = blog.Products.Where(p => !blogUpdateDto.ProductIds.Contains(p.ProductId)).ToList();
                foreach (var productToRemove in productsToRemove)
                {
                    blog.Products.Remove(productToRemove);
                }

                // Find the products to add (those that are in the update list but not in the current collection)
                var newProductIds = blogUpdateDto.ProductIds.Except(currentProductIds).ToList();
                if (newProductIds.Count > 0)
                {
                    var productsToAdd = await _context.Products
                        .Where(p => newProductIds.Contains(p.ProductId))
                        .ToListAsync();

                    foreach (var productToAdd in productsToAdd)
                    {
                        blog.Products.Add(productToAdd);
                    }
                }
            }

            // Delete old images if new ones are provided
            if (blogUpdateDto.DesktopImage != null)
            {
                await _imageService.DeleteImageAsync(blog.DesktopImageUrl);
                blog.DesktopImageUrl = await UploadImageAsync(blogUpdateDto.DesktopImage);
            }

            if (blogUpdateDto.LaptopImage != null)
            {
                await _imageService.DeleteImageAsync(blog.LaptopImageUrl);
                blog.LaptopImageUrl = await UploadImageAsync(blogUpdateDto.LaptopImage);
            }

            if (blogUpdateDto.TabletImage != null)
            {
                await _imageService.DeleteImageAsync(blog.TabletImageUrl);
                blog.TabletImageUrl = await UploadImageAsync(blogUpdateDto.TabletImage);
            }

            if (blogUpdateDto.PhoneImage != null)
            {
                await _imageService.DeleteImageAsync(blog.PhoneImageUrl);
                blog.PhoneImageUrl = await UploadImageAsync(blogUpdateDto.PhoneImage);
            }

            _context.Entry(blog).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok();
        }


        private async Task<string> UploadImageAsync(IFormFile imageFile)
        {
            using var stream = new MemoryStream();
            await imageFile.CopyToAsync(stream);
            stream.Position = 0;
            return await _imageService.UploadImageAsync(imageFile.FileName, stream);
        }
    }
}