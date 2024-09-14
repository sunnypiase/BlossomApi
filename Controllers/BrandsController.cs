using AutoMapper;
using BlossomApi.DB;
using BlossomApi.Dtos;
using BlossomApi.Dtos.Brends;
using BlossomApi.Models;
using BlossomApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlossomApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly BlossomContext _context;
        private readonly IMapper _mapper;
        private readonly ImageService _imageService;

        public BrandsController(BlossomContext context, IMapper mapper, ImageService imageService)
        {
            _context = context;
            _mapper = mapper;
            _imageService = imageService;
        }

        // POST: api/Brands
        [HttpPost]
        public async Task<ActionResult<BrandResponseDto>> CreateBrand([FromForm] BrandCreateDto brandDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var brand = new Brand
            {
                Title = brandDto.Title,
                Description = brandDto.Description,
                MetaKeywords = brandDto.MetaKeywords,
                MetaDescription = brandDto.MetaDescription
            };

            // Upload images
            brand.ImageUrl = await UploadImageAsync(brandDto.Image);
            brand.LogoImageUrl = await UploadImageAsync(brandDto.LogoImage);

            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();

            var brandResponse = _mapper.Map<BrandResponseDto>(brand);

            return CreatedAtAction(nameof(GetBrand), new { id = brand.BrandId }, brandResponse);
        }

        // GET: api/Category/Search
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<BrandResponseDto>>> SearchBrands([FromQuery] string? searchTerm)
        {
            var brandsQuery = _context.Brands.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var lowerSearchTerm = searchTerm.ToLower();
                brandsQuery = brandsQuery.Where(c => c.Title.ToLower().Contains(lowerSearchTerm));
            }

            var brandDtos = _mapper.Map<List<BrandResponseDto>>(await brandsQuery.ToListAsync());

            return Ok(brandDtos);
        }

        // GET: api/Brands
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrandResponseDto>>> GetBrands()
        {
            var brands = await _context.Brands.ToListAsync();
            var brandDtos = _mapper.Map<List<BrandResponseDto>>(brands);
            return Ok(brandDtos);
        }

        // GET: api/Brands/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<BrandWithProductsDto>> GetBrand(int id)
        {
            var brand = await _context.Brands
                .Include(b => b.Products)
                .FirstOrDefaultAsync(b => b.BrandId == id);

            if (brand == null)
            {
                return NotFound();
            }

            var brandResponse = _mapper.Map<BrandWithProductsDto>(brand);
            return Ok(brandResponse);
        }

        // DELETE: api/Brands/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            // Delete associated images
            await _imageService.DeleteImageAsync(brand.ImageUrl);
            await _imageService.DeleteImageAsync(brand.LogoImageUrl);

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // PUT: api/Brands/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBrand(int id, [FromForm] BrandUpdateDto brandUpdateDto)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            if (brandUpdateDto.Title != null)
            {
                brand.Title = brandUpdateDto.Title;
            }
            if (brandUpdateDto.Description != null)
            {
                brand.Description = brandUpdateDto.Description;
            }
            if (brandUpdateDto.MetaKeywords != null)
            {
                brand.MetaKeywords = brandUpdateDto.MetaKeywords;
            }
            if (brandUpdateDto.MetaDescription != null)
            {
                brand.MetaDescription = brandUpdateDto.MetaDescription;
            }
            // Delete old images if new ones are provided
            if (brandUpdateDto.Image != null)
            {
                await _imageService.DeleteImageAsync(brand.ImageUrl);
                brand.ImageUrl = await UploadImageAsync(brandUpdateDto.Image);
            }

            if (brandUpdateDto.LogoImage != null)
            {
                await _imageService.DeleteImageAsync(brand.LogoImageUrl);
                brand.LogoImageUrl = await UploadImageAsync(brandUpdateDto.LogoImage);
            }

            _context.Entry(brand).State = EntityState.Modified;
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
