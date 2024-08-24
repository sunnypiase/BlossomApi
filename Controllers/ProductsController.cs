using AutoMapper;
using AutoMapper.QueryableExtensions;
using BlossomApi.DB;
using BlossomApi.Dtos;
using BlossomApi.Models;
using BlossomApi.Repositories;
using BlossomApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BlossomApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly BlossomContext _context;
        private readonly CategoryService _categoryService;
        private readonly ImageService _imageService;
        private readonly ProductQueryService _productQueryService;
        private readonly IShownProductRepository _shownProductRepository;
        private readonly IMemoryCache _cache;
        private readonly ProductImageService _productImageService;
        private readonly ProductImportService _productImportService;
        private readonly ProductRecommendationService _productRecommendationService;
        private readonly ProductUpdateService _productUpdateService;
        private readonly ProductCreateService _productCreateService;
        private readonly IMapper _mapper;

        public ProductController(
            BlossomContext context,
            CategoryService categoryService,
            ImageService imageService,
            IShownProductRepository shownProductRepository,
            IMemoryCache cache,
            ProductQueryService productQueryService,
            ProductImageService productImageService,
            ProductImportService productImportService,
            ProductRecommendationService productRecommendationService,
            IMapper mapper,
            ProductUpdateService productUpdateService,
            ProductCreateService productCreateService)
        {
            _context = context;
            _categoryService = categoryService;
            _imageService = imageService;
            _shownProductRepository = shownProductRepository;
            _cache = cache;
            _productQueryService = productQueryService;
            _productImageService = productImageService;
            _productImportService = productImportService;
            _productRecommendationService = productRecommendationService;
            _mapper = mapper;
            _productUpdateService = productUpdateService;
            _productCreateService = productCreateService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<ProductResponseDto>>(products));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductResponseDto>> GetProduct(int id)
        {
            var product = await _context.Products
                .Include(x => x.Categories)
                .Include(x => x.Reviews)
                .Include(x => x.Characteristics)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ProductResponseDto>(product));
        }

        [HttpGet("ByIds")]
        public ActionResult<IEnumerable<ProductResponseDto>> GetProductByIds([FromQuery] List<int> ids)
        {
            var products = _context.Products
                .Include(x => x.Categories)
                .Include(x => x.Reviews)
                .Include(x => x.Characteristics)
                .Where(x => ids.Contains(x.ProductId))
                .ToList();

            if (products == null || products.Count == 0)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<ProductResponseDto>>(products));
        }

        [HttpGet("AlsoBought/{id:int}")]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetAlsoBoughtProducts(int id)
        {
            var products = await _productRecommendationService.GetAlsoBoughtProductsAsync(id);
            if (products == null || !products.Any())
            {
                return NotFound();
            }

            return Ok(products);
        }

        [HttpPost("GetProductsByFilter")]
        public async Task<ActionResult<GetProductsByFilterResponse>> GetProductsByFilter(GetProductsByFilterRequestDto request)
        {
            var query = await _productQueryService.ApplyFilterAndSortAsync(new GetProductsByAdminFilterRequestDto
            {
                CategoryIds = request.CategoryId.HasValue ? [request.CategoryId.Value] : [],
                SortOption = request.SortBy == "popularity" ? "popularity_dsc" : request.SortBy,
                Amount = request.Amount,
                Start = request.Start,
                MaxPrice = request.MaxPrice,
                MinPrice = request.MinPrice,
                SelectedCharacteristics = request.SelectedCharacteristics,
                IsShown = true
            });

            var totalCount = await query.CountAsync();

            var products = await query
                .Skip(request.Start)
                .Take(request.Amount)
                .ToListAsync();

            var productDtos = _mapper.Map<List<ProductResponseDto>>(products);

            var response = new GetProductsByFilterResponse
            {
                Products = productDtos,
                TotalCount = totalCount
            };

            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDto productCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (isSuccess, errorMessage, product) = await _productCreateService.CreateProductAsync(productCreateDto);

            if (!isSuccess)
            {
                return BadRequest(errorMessage);
            }

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPost("{id}/images")]
        public async Task<IActionResult> AddProductImages(int id, [FromForm] List<IFormFile> imageFiles)
        {
            try
            {
                var uploadedImageUrls = await _productImageService.AddProductImagesAsync(id, imageFiles);
                return Ok(new { Message = "Images uploaded successfully", ImageUrls = uploadedImageUrls });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ImportFromExcel")]
        public async Task<ActionResult<IEnumerable<int>>> ImportFromExcel([FromForm] FileModel fileModel)
        {
            try
            {
                var productIds = await _productImportService.ImportFromExcelAsync(fileModel.ExcelFile);
                return Ok(productIds);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateDto productUpdateDto)
        {
            var success = await _productUpdateService.UpdateProductAsync(id, productUpdateDto);
            if (!success)
            {
                return NotFound("Product not found.");
            }

            return Ok("Product updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }


        public class FileModel
        {
            public IFormFile ExcelFile { get; set; }
        }
    }
}
