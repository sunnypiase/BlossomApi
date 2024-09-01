using AutoMapper;
using BlossomApi.DB;
using BlossomApi.Dtos;
using BlossomApi.Dtos.Banners;
using BlossomApi.Dtos.Product;
using BlossomApi.Models;
using BlossomApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BlossomApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BannersController : ControllerBase
    {
        private readonly BlossomContext _context;
        private readonly IMapper _mapper;
        private readonly ImageService _imageService;
        private readonly ProductQueryService _productQueryService;

        public BannersController(BlossomContext context, IMapper mapper, ImageService imageService, ProductQueryService productQueryService)
        {
            _context = context;
            _mapper = mapper;
            _imageService = imageService;
            _productQueryService = productQueryService;
        }

        // POST: api/Banners
        [HttpPost]
        public async Task<ActionResult<BannerResponseDto>> CreateBanner([FromForm] BannerCreateDto bannerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var banner = new Banner
            {
                Title = bannerDto.Title,
                Description = bannerDto.Description,
                DesktopAltText = bannerDto.DesktopAltText,
                LaptopAltText = bannerDto.LaptopAltText,
                TabletAltText = bannerDto.TabletAltText,
                PhoneAltText = bannerDto.PhoneAltText
            };

            // Upload images
            banner.DesktopImageUrl = await UploadImageAsync(bannerDto.DesktopImage);
            banner.LaptopImageUrl = await UploadImageAsync(bannerDto.LaptopImage);
            banner.TabletImageUrl = await UploadImageAsync(bannerDto.TabletImage);
            banner.PhoneImageUrl = await UploadImageAsync(bannerDto.PhoneImage);

            // Fetch associated products
            if (bannerDto.ProductIds.Any())
            {
                var products = await _context.Products
                    .Where(p => bannerDto.ProductIds.Contains(p.ProductId))
                    .ToListAsync();

                banner.Products = products;
            }

            _context.Banners.Add(banner);
            await _context.SaveChangesAsync();

            var bannerResponse = _mapper.Map<BannerResponseDto>(banner);

            return CreatedAtAction(nameof(GetBanner), new { id = banner.BannerId }, bannerResponse);
        }

        // GET: api/Banners
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BannerResponseDto>>> GetBanners()
        {
            var banners = await _context.Banners.ToListAsync();
            var bannerDtos = _mapper.Map<List<BannerResponseDto>>(banners);
            return Ok(bannerDtos);
        }

        // GET: api/Banners/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<BannerWithProductsDto>> GetBanner(int id)
        {
            var banner = await _context.Banners
                .Include(b => b.Products)
                .FirstOrDefaultAsync(b => b.BannerId == id);

            if (banner == null)
            {
                return NotFound();
            }

            var bannerResponse = _mapper.Map<BannerWithProductsDto>(banner);
            return Ok(bannerResponse);
        }

        // POST: api/AdminProduct/GetProductsByBannerFilter
        [HttpPost("GetProductsByBannerFilter")]
        public async Task<ActionResult<GetProductsByFilterResponse>> GetProductsByBannerFilter(GetProductsByBannerFilterRequestDto request)
        {

            Expression<Func<Product, bool>>? bannerFilter = product => product.IsShown && product.Banners.Any(b => b.BannerId == request.BannerId);

            var query = await _productQueryService.ApplyFilterAndSortAsync(new GetProductsByAdminFilterRequestDto()
            {
                CategoryIds = request.CategoryIds,
                SelectedCharacteristics = request.SelectedCharacteristics,
                MinPrice = request.MinPrice,
                MaxPrice = request.MaxPrice,
                Start = request.Start,
                Amount = request.Amount,
                SortOption = request.SortBy,
                IsShown = true

            },
            bannerFilter);

            var totalCount = await query.CountAsync();

            var products = await query
                .Skip(request.Start)
                .Take(request.Amount)
                .ToListAsync();

            var productDtos = _mapper.Map<List<ProductCardDto>>(products);

            var response = new GetProductsByFilterResponse
            {
                Products = productDtos,
                TotalCount = totalCount
            };

            return Ok(response);
        }

        // DELETE: api/Banners/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBanner(int id)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner == null)
            {
                return NotFound();
            }

            // Delete associated images
            await _imageService.DeleteImageAsync(banner.DesktopImageUrl);
            await _imageService.DeleteImageAsync(banner.LaptopImageUrl);
            await _imageService.DeleteImageAsync(banner.TabletImageUrl);
            await _imageService.DeleteImageAsync(banner.PhoneImageUrl);

            _context.Banners.Remove(banner);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // PUT: api/Banners/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBanner(int id, [FromForm] BannerUpdateDto bannerUpdateDto)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner == null)
            {
                return NotFound();
            }

            if (bannerUpdateDto.Title != null)
            {
                banner.Title = bannerUpdateDto.Title;
            }
            if (bannerUpdateDto.Description != null)
            {
                banner.Description = bannerUpdateDto.Description;
            }
            if(bannerUpdateDto.DesktopAltText != null)
            {
                banner.DesktopAltText = bannerUpdateDto.DesktopAltText;
            }
            if(bannerUpdateDto.LaptopAltText != null)
            {
                banner.LaptopAltText = bannerUpdateDto.LaptopAltText;
            }
            if(bannerUpdateDto.TabletAltText != null)
            {
                banner.TabletAltText = bannerUpdateDto.TabletAltText;
            }
            if(bannerUpdateDto.PhoneAltText != null)
            {
                banner.PhoneAltText = bannerUpdateDto.PhoneAltText;
            }
            if (bannerUpdateDto.ProductIds != null && bannerUpdateDto.ProductIds.Count != 0)
            {
                var products = await _context.Products
                    .Where(p => bannerUpdateDto.ProductIds.Contains(p.ProductId))
                    .ToListAsync();

                banner.Products = products;
            }
            // Delete old images if new ones are provided
            if (bannerUpdateDto.DesktopImage != null)
            {
                await _imageService.DeleteImageAsync(banner.DesktopImageUrl);
                banner.DesktopImageUrl = await UploadImageAsync(bannerUpdateDto.DesktopImage);
            }

            if (bannerUpdateDto.LaptopImage != null)
            {
                await _imageService.DeleteImageAsync(banner.LaptopImageUrl);
                banner.LaptopImageUrl = await UploadImageAsync(bannerUpdateDto.LaptopImage);
            }

            if (bannerUpdateDto.TabletImage != null)
            {
                await _imageService.DeleteImageAsync(banner.TabletImageUrl);
                banner.TabletImageUrl = await UploadImageAsync(bannerUpdateDto.TabletImage);
            }

            if (bannerUpdateDto.PhoneImage != null)
            {
                await _imageService.DeleteImageAsync(banner.PhoneImageUrl);
                banner.PhoneImageUrl = await UploadImageAsync(bannerUpdateDto.PhoneImage);
            }

            _context.Entry(banner).State = EntityState.Modified;
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
