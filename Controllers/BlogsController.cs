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
        private readonly ProductAssociationService _productAssociationService;

        public BlogsController(BlossomContext context, IMapper mapper, ImageService imageService, ProductAssociationService productAssociationService)
        {
            _context = context;
            _mapper = mapper;
            _imageService = imageService;
            _productAssociationService = productAssociationService;
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
                AltText = blogDto.AltText
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

            if (blogUpdateDto.AltText != null)
            {
                blog.AltText = blogUpdateDto.AltText;
            }

            // Update product associations
            if (blogUpdateDto.ProductIds != null && blogUpdateDto.ProductIds.Count != 0)
            {
               await _productAssociationService.UpdateProductAssociationsAsync(blog, blogUpdateDto.ProductIds);
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