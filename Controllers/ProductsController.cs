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
            IMapper mapper)
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
                CategoryId = request.CategoryId,
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
                .ProjectTo<ProductResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var response = new GetProductsByFilterResponse
            {
                Products = products,
                TotalCount = totalCount
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ProductResponseDto>> PostProduct([FromBody] ProductCreateDto productDto)
        {
            var product = new Product();
            await UpdateProductAsync(product, productDto, null);

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var productResponse = _mapper.Map<ProductResponseDto>(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, productResponse);
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
        public async Task<IActionResult> PutProduct(int id, [FromBody] ProductCreateDto productDto)
        {
            var product = await _context.Products.Include(p => p.Categories).FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            await UpdateProductAsync(product, productDto, null);

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
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

        private async Task UpdateProductAsync(Product product, ProductCreateDto productDto, List<IFormFile>? imageFiles)
        {
            product.Name = productDto?.Name ?? product.Name;
            product.NameEng = productDto?.NameEng ?? product.NameEng;
            product.Brand = productDto?.Brand ?? product.Brand;
            product.Price = productDto?.Price ?? product.Price;
            product.Discount = productDto?.Discount ?? product.Discount;
            product.IsNew = productDto?.IsNew ?? product.IsNew;
            product.InStock = (productDto?.AvailableAmount ?? product.AvailableAmount) > 0;
            product.AvailableAmount = productDto?.AvailableAmount ?? product.AvailableAmount;
            product.Article = productDto?.Article ?? product.Article;
            product.DieNumbers = productDto?.DieNumbers ?? product.DieNumbers;
            product.Description = productDto?.Description ?? product.Description;

            if (imageFiles != null && imageFiles.Count > 0)
            {
                var uploadedImageUrls = new List<string>();

                foreach (var imageFile in imageFiles)
                {
                    if (imageFile.Length == 0)
                    {
                        continue; // Skip empty files
                    }

                    using (var stream = new MemoryStream())
                    {
                        await imageFile.CopyToAsync(stream);
                        stream.Position = 0;

                        var imageUrl = await _imageService.UploadImageAsync(imageFile.FileName, stream);
                        uploadedImageUrls.Add(imageUrl);
                    }
                }

                product.Images = uploadedImageUrls; // This will automatically serialize to ImagesSerialized
            }
            else
            {
                product.Images ??= new(); // Ensure Images is not null
            }

            if (productDto?.CategoryIds != null)
            {
                product.Categories.Clear();
                foreach (var categoryId in productDto.CategoryIds)
                {
                    var category = _context.Categories.Find(categoryId);
                    if (category != null)
                    {
                        product.Categories.Add(category);
                    }
                }
            }
        }
    }

    public class FileModel
    {
        public IFormFile ExcelFile { get; set; }
    }
}
