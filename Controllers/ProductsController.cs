using AutoMapper;
using BlossomApi.DB;
using BlossomApi.Dtos;
using BlossomApi.Dtos.Product;
using BlossomApi.Models;
using BlossomApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;

namespace BlossomApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly BlossomContext _context;
        private readonly ImageService _imageService;
        private readonly ProductQueryService _productQueryService;
        private readonly ProductImageService _productImageService;
        private readonly ProductImportService _productImportService;
        private readonly ProductRecommendationService _productRecommendationService;
        private readonly ProductUpdateService _productUpdateService;
        private readonly ProductCreateService _productCreateService;
        private readonly IMapper _mapper;

        public ProductController(
            BlossomContext context,
            ImageService imageService,
            ProductQueryService productQueryService,
            ProductImageService productImageService,
            ProductImportService productImportService,
            ProductRecommendationService productRecommendationService,
            IMapper mapper,
            ProductUpdateService productUpdateService,
            ProductCreateService productCreateService)
        {
            _context = context;
            _imageService = imageService;
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
                .Include(x => x.Brand)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound("Продукт не знайдено.");
            }

            product.NumberOfViews += 1;
            _context.Products.Update(product);

            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<ProductResponseDto>(product));
        }

        [HttpGet("ByIds")]
        public ActionResult<IEnumerable<ProductResponseDto>> GetProductByIds([FromQuery] List<int> ids)
        {
            var products = _context.Products
                .Include(x => x.Categories)
                .Include(x => x.Reviews)
                .Include(x => x.Characteristics)
                .Include(x => x.Brand)
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
            Expression<Func<Product, bool>>? categoryFilter = product => product.IsShown && product.Categories.Any(b => b.CategoryId == request.CategoryId);

            var query = await _productQueryService.ApplyFilterAndSortAsync(new GetProductsByAdminFilterRequestDto
            {
                CategoryIds = request.CategoryIds,
                SortOption = request.SortBy,
                Amount = request.Amount,
                Start = request.Start,
                IsHit = request.IsHit,
                IsNew = request.IsNew,
                HasDiscount = request.HasDiscount,
                MaxPrice = request.MaxPrice,
                MinPrice = request.MinPrice,
                SelectedCharacteristics = request.SelectedCharacteristics,
                IsShown = true
            }, categoryFilter);

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


        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreateDto productCreateDto)
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
            try
            {
                await _productImageService.AddProductImagesAsync(product.Id, productCreateDto.Images);
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/images")]
        public async Task<IActionResult> AddProductImages(int id, [FromForm] IFormFileCollection imageFiles)
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
            if (fileModel.ExcelFile == null || fileModel.ExcelFile.Length == 0)
            {
                return BadRequest("Invalid file.");
            }

            using var stream = fileModel.ExcelFile.OpenReadStream();
            var (isSuccess, errorMessage, products) = await _productImportService.ImportProductsFromExcelAsync(stream);

            if (!isSuccess)
            {
                return BadRequest(new { Message = "Failed to import products", Errors = errorMessage });
            }

            return Ok(new { Message = "Products imported successfully", Products = products });

        }

        // POST: api/AdminProduct/RemoveImage/{productId}
        [HttpPost("RemoveImage/{productId}")]
        public async Task<IActionResult> RemoveProductImage(int productId, [FromBody] string imageUrl)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(imageUrl) || !product.Images.Contains(imageUrl))
            {
                return BadRequest("Image URL is not valid or does not exist in the product's images.");
            }
            var remainingImages = product.Images.Where(i => i != imageUrl).ToList();
            // Remove the image from the product's image list
            product.Images = remainingImages;
            _context.Entry(product).State = EntityState.Modified;

            // Extract the filename from the URL
            var fileName = imageUrl.Split('/').Last();

            try
            {
                // Delete the image from BunnyCDN
                await _imageService.DeleteImageAsync(fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to delete image: {ex.Message}");
            }

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Image removed successfully." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductUpdateDto productUpdateDto)
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

        [HttpDelete("DeleteAll")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAllProducts()
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var products = await _context.Products
                    .Include(p => p.Reviews)
                    .Include(p => p.ShoppingCartProducts)
                    .Include(p => p.UsersWhoFavorited)
                    .Include(p => p.Banners)
                    .Include(p => p.Blogs)
                    .ToListAsync();

                // Remove related entities
                foreach (var product in products)
                {
                    // Remove images from storage
                    foreach (var fileName in product.Images.Select(imageUrl => imageUrl.Split('/').Last()))
                    {
                        try
                        {
                            await _imageService.DeleteImageAsync(fileName);
                        }
                        catch (Exception ex)
                        {
                            // Log the exception if needed
                        }
                    }

                    // Remove reviews
                    _context.Reviews.RemoveRange(product.Reviews);

                    // Remove ShoppingCartProducts
                    _context.ShoppingCartProducts.RemoveRange(product.ShoppingCartProducts);

                    // Remove associations with UsersWhoFavorited
                    product.UsersWhoFavorited.Clear();

                    // Remove associations with Banners and Blogs
                    product.Banners.Clear();
                    product.Blogs.Clear();
                }

                // Remove all products
                _context.Products.RemoveRange(products);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                // Log the exception if needed
                return StatusCode(500, "An error occurred while deleting products.");
            }
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
