using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlossomApi.DB;
using BlossomApi.Models;
using BlossomApi.Dtos;
using BlossomApi.Services;
using System.Text.Json;
using System.Linq.Expressions;
using OfficeOpenXml;

namespace BlossomApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly BlossomContext _context;
        private readonly CategoryService _categoryService;
        private readonly ImageService _imageService;

        public ProductController(BlossomContext context, CategoryService categoryService, ImageService imageService)
        {
            _context = context;
            _categoryService = categoryService;
            _imageService = imageService;
        }

        // POST: api/Product/GetProductsByFilter
        [HttpPost("GetProductsByFilter")]
        public async Task<ActionResult<GetProductsByFilterResponse>> GetProductsByFilter(GetProductsByFilterRequestDto request)
        {
            var query = _context.Products.AsQueryable();

            if (request.CategoryId != null)
            {
                var rootCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.CategoryId == request.CategoryId);

                if (rootCategory != null)
                {
                    var allCategories = new List<Category> { rootCategory };
                    allCategories.AddRange(await _categoryService.GetAllChildCategoriesAsync(rootCategory.CategoryId));

                    var categoryIds = allCategories.Select(c => c.CategoryId).ToList();
                    query = query.Where(p => p.Categories.Any(c => categoryIds.Contains(c.CategoryId)));
                }
            }

            if (request.SelectedCharacteristics != null && request.SelectedCharacteristics.Count != 0)
            {
                var characteristicIds = request.SelectedCharacteristics;
                Expression<Func<Product, bool>> predicate = p => false;

                predicate = characteristicIds
                    .Aggregate(predicate, (current, temp) => current.Or(p => p.Characteristics.Any(c => c.CharacteristicId == temp)));

                query = query.Where(predicate);
            }

            if (request.MinPrice.HasValue && request.MinPrice.Value != 0)
            {
                query = query.Where(p => p.Price >= request.MinPrice.Value);
            }

            if (request.MaxPrice.HasValue && request.MaxPrice.Value != 0)
            {
                query = query.Where(p => p.Price <= request.MaxPrice.Value);
            }

            query = query.OrderByDescending(p => p.InStock);
            // Apply sorting and then ensure products out of stock are at the end
            query = request.SortBy switch
            {
                "popularity" => ((IOrderedQueryable<Product>)query).ThenByDescending(p => p.Rating),
                "price_asc" => ((IOrderedQueryable<Product>)query).ThenBy(p => p.Price),
                "price_desc" => ((IOrderedQueryable<Product>)query).ThenByDescending(p => p.Price),
                _ => ((IOrderedQueryable<Product>)query).ThenBy(p => p.Name)
            };


            var totalCount = await query.CountAsync();

            var products = await query
                .Skip(request.Start)
                .Take(request.Amount)
                .Select(p => MapToProductResponseDto(p))
                .ToListAsync();

            var response = new GetProductsByFilterResponse
            {
                Products = products,
                TotalCount = totalCount
            };

            return Ok(response);
        }


        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetProducts()
        {
            return await _context.Products
                .Select(p => MapToProductResponseDto(p))
                .ToListAsync();
        }

        // GET: api/Product/{id}
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

            return Ok(MapToProductResponseDto(product));
        }

        // GET: api/Product/ByIds
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

            return Ok(products.Select(MapToProductResponseDto));
        }

        // PUT: api/Product/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, [FromForm] ProductCreateDto productDto, [FromForm] List<IFormFile>? imageFiles)
        {
            var product = await _context.Products.Include(p => p.Categories).FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            await UpdateProductAsync(product, productDto, imageFiles);

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

            return NoContent();
        }

        // POST: api/Product
        [HttpPost]
        public async Task<ActionResult<ProductResponseDto>> PostProduct([FromBody] ProductCreateDto productDto)
        {
            var product = new Product();
            await UpdateProductAsync(product, productDto, null);

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var productResponse = MapToProductResponseDto(product);

            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, productResponse);
        }

        // DELETE: api/Product/5
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

        // POST: api/Product/{id}/images
        [HttpPost("{id}/images")]
        public async Task<IActionResult> AddProductImages(int id, List<IFormFile> imageFiles)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound("Product not found");
            }

            if (imageFiles == null || imageFiles.Count == 0)
            {
                return BadRequest("No image files provided");
            }

            var uploadedImageUrls = new List<string>();

            foreach (var imageFile in imageFiles)
            {
                if (imageFile.Length == 0)
                {
                    return BadRequest($"File {imageFile.FileName} is empty");
                }

                using (var stream = new MemoryStream())
                {
                    await imageFile.CopyToAsync(stream);
                    stream.Position = 0;

                    var imageUrl = await _imageService.UploadImageAsync(imageFile.FileName, stream);
                    uploadedImageUrls.Add(imageUrl);
                }
            }

            product.Images = uploadedImageUrls;

            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Images uploaded successfully", ImageUrls = uploadedImageUrls });
        }

        [HttpPost("ImportFromExcel")]
        public async Task<ActionResult<IEnumerable<int>>> ImportFromExcel([FromForm] FileModel fileModel)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                return BadRequest("No file uploaded or file is empty.");
            }

            var productIds = new List<int>();

            try
            {
                using (var stream = new MemoryStream())
                {
                    await excelFile.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;

                        var products = new List<Product>();

                        for (int row = 2; row <= rowCount; row++) // Assuming the first row is headers
                        {
                            var productDto = new ProductCreateDto
                            {
                                Name = worksheet.Cells[row, 1].Value?.ToString(),
                                NameEng = worksheet.Cells[row, 2].Value?.ToString(),
                                Brand = worksheet.Cells[row, 3].Value?.ToString(),
                                Price = decimal.Parse(worksheet.Cells[row, 4].Value?.ToString() ?? "0"),
                                Discount = decimal.Parse(worksheet.Cells[row, 5].Value?.ToString() ?? "0"),
                                IsNew = bool.Parse(worksheet.Cells[row, 6].Value?.ToString() ?? "false"),
                                AvailableAmount = int.Parse(worksheet.Cells[row, 7].Value?.ToString() ?? "0"),
                                Article = worksheet.Cells[row, 8].Value?.ToString(),
                                Description = worksheet.Cells[row, 9].Value?.ToString(),
                                CategoryIds = worksheet.Cells[row, 10].Value?.ToString()
                                    ?.Split(',')
                                    .Select(int.Parse)
                                    .ToList() ?? new List<int>(),
                            };

                            var product = new Product();
                            await UpdateProductAsync(product, productDto, null);
                            products.Add(product);
                        }

                        // Add all products in one transaction
                        _context.Products.AddRange(products);
                        await _context.SaveChangesAsync();

                        // Retrieve the generated IDs
                        productIds = products.Select(p => p.ProductId).ToList();
                    }
                }

                return Ok(productIds);
            }
            catch (Exception ex)
            {
                // Log the exception (implement logging as necessary)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
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
                product.Images ??= []; // Ensure Images is not null
            }

            // Update categories
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


        private static ProductResponseDto MapToProductResponseDto(Product p)
        {
            return new ProductResponseDto
            {
                Id = p.ProductId,
                Name = p.Name,
                NameEng = p.NameEng,
                Amount = p.AvailableAmount,
                Images = p.Images,
                Brand = p.Brand,
                Price = p.Price,
                Discount = p.Discount,
                IsNew = p.IsNew,
                Rating = p.Rating,
                NumberOfReviews = p.NumberOfReviews,
                NumberOfPurchases = p.NumberOfPurchases,
                NumberOfViews = p.NumberOfViews,
                Article = p.Article,
                Categories = p.Categories.Select(c => new CategoryResponseDto { CategoryId = c.CategoryId, Name = c.Name, ParentCategoryId = c.ParentCategoryId }).ToList(),
                DieNumbers = p.DieNumbers,
                Reviews = p.Reviews.Select(r => new ReviewDto
                {
                    Name = r.Name,
                    Review = r.ReviewText,
                    Rating = r.Rating,
                    Date = r.Date.ToString("dd.MM.yyyy")
                }).ToList(),
                Characteristics = p.Characteristics.Select(c => new CharacteristicDto
                {
                    Title = c.Title,
                    Desc = c.Desc
                }).ToList(),
                Description = p.Description,
                InStock = p.InStock
            };
        }
    }
    public class FileModel
    {
        public IFormFile ExcelFile { get; set; }
    }
}
