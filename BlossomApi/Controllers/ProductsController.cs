using System.Linq.Expressions;
using BlossomApi.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlossomApi.Models;
using BlossomApi.Dtos;
using System.Text.Json;
using BlossomApi.Services;

namespace BlossomApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly BlossomContext _context;
        private readonly CategoryService _categoryService;

        public ProductController(BlossomContext context, CategoryService categoryService)
        {
            _context = context;
            _categoryService = categoryService;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetProducts()
        {
            return await _context.Products
                .Select(p => new ProductResponseDto
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
                    Options = p.Options,
                    Categories = p.Categories.Select(c => c.Name).ToList(),
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
                })
                .ToListAsync();
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponseDto>> GetProduct(int id)
        {
            var product = await _context.Products
                .Select(p => new ProductResponseDto
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
                    Options = p.Options,
                    Categories = p.Categories.Select(c => c.Name).ToList(),
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
                })
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Product/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductCreateDto productDto)
        {
            var product = await _context.Products.Include(p => p.Categories).FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            product.Name = productDto.Name;
            product.NameEng = productDto.NameEng;
            product.Images = productDto.Images;
            product.Brand = productDto.Brand;
            product.Price = productDto.Price;
            product.Discount = productDto.Discount;
            product.IsNew = productDto.IsNew;
            product.Rating = productDto.Rating;
            product.InStock = productDto.InStock;
            product.AvailableAmount = productDto.AvailableAmount;
            product.NumberOfReviews = productDto.NumberOfReviews;
            product.NumberOfPurchases = productDto.NumberOfPurchases;
            product.NumberOfViews = productDto.NumberOfViews;
            product.Article = productDto.Article;
            product.Options = productDto.Options;
            product.DieNumbers = productDto.DieNumbers;
            product.Description = productDto.Description;

            // Update categories
            product.Categories.Clear();
            foreach (var categoryId in productDto.CategoryIds)
            {
                var category = await _context.Categories.FindAsync(categoryId);
                if (category != null)
                {
                    product.Categories.Add(category);
                }
            }

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
        public async Task<ActionResult<ProductResponseDto>> PostProduct(ProductCreateDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                NameEng = productDto.NameEng,
                ImagesSerialized = JsonSerializer.Serialize(productDto.Images),
                Brand = productDto.Brand,
                Price = productDto.Price,
                Discount = productDto.Discount,
                IsNew = productDto.IsNew,
                Rating = productDto.Rating,
                InStock = productDto.InStock,
                AvailableAmount = productDto.AvailableAmount,
                NumberOfReviews = productDto.NumberOfReviews,
                NumberOfPurchases = productDto.NumberOfPurchases,
                NumberOfViews = productDto.NumberOfViews,
                Article = productDto.Article,
                OptionsSerialized = JsonSerializer.Serialize(productDto.Options),
                DieNumbersSerialized = JsonSerializer.Serialize(productDto.DieNumbers),
                Description = productDto.Description,
            };

            foreach (var categoryId in productDto.CategoryIds)
            {
                var category = await _context.Categories.FindAsync(categoryId);
                if (category != null)
                {
                    product.Categories.Add(category);
                }
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var productResponse = new ProductResponseDto
            {
                Id = product.ProductId,
                Name = product.Name,
                NameEng = product.NameEng,
                Amount = product.AvailableAmount,
                Images = JsonSerializer.Deserialize<List<string>>(product.ImagesSerialized) ?? new List<string>(),
                Brand = product.Brand,
                Price = product.Price,
                Discount = product.Discount,
                IsNew = product.IsNew,
                Rating = product.Rating,
                NumberOfReviews = product.NumberOfReviews,
                NumberOfPurchases = product.NumberOfPurchases,
                NumberOfViews = product.NumberOfViews,
                Article = product.Article,
                Options = JsonSerializer.Deserialize<List<string>>(product.OptionsSerialized) ?? new List<string>(),
                Categories = product.Categories.Select(c => c.Name).ToList(),
                DieNumbers = JsonSerializer.Deserialize<List<int>>(product.DieNumbersSerialized) ?? new List<int>(),
                Reviews = product.Reviews.Select(r => new ReviewDto
                {
                    Name = r.Name,
                    Review = r.ReviewText,
                    Rating = r.Rating,
                    Date = r.Date.ToString("dd.MM.yyyy")
                }).ToList(),
                Characteristics = product.Characteristics.Select(c => new CharacteristicDto
                {
                    Title = c.Title,
                    Desc = c.Desc
                }).ToList(),
                Description = product.Description,
                InStock = product.InStock
            };

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

        // POST: api/Product/GetProductsByFilter
        [HttpPost("GetProductsByFilter")]
        public async Task<ActionResult<GetProductsByFilterResponse>> GetProductsByFilter(GetProductsByFilterRequestDto request)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(request.CategoryName))
            {
                var rootCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name.ToLower() == request.CategoryName.ToLower());

                if (rootCategory != null)
                {
                    var allCategories = new List<Category> { rootCategory };
                    allCategories.AddRange(await _categoryService.GetAllChildCategoriesAsync(rootCategory.CategoryId));

                    var categoryNames = allCategories.Select(c => c.Name).ToList();
                    query = query.Where(p => p.Categories.Any(c => categoryNames.Contains(c.Name)));
                }
            }

            if (request.SelectedCharacteristics != null && request.SelectedCharacteristics.Count != 0)
            {
                Expression<Func<Product, bool>> predicate = p => false;

                predicate = request
                    .SelectedCharacteristics
                    .Aggregate(predicate, (current, temp) => current.Or(p => p.Characteristics.Any(c => c.Desc == temp)));

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

            query = request.SortBy switch
            {
                "popularity" => query.OrderByDescending(p => p.Rating), // Assuming popularity is determined by rating
                "price_asc" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                _ => query.OrderBy(p => p.Name)
            };

            var totalCount = await query.CountAsync();

            var products = await query
                .Skip(request.Start)
                .Take(request.Amount)
                .Select(p => new GetProductsByFilterResponseDto
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
                    Options = p.Options,
                    Categories = p.Categories.Select(c => c.Name).ToList(),
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
                })
                .ToListAsync();

            var response = new GetProductsByFilterResponse
            {
                Products = products,
                TotalCount = totalCount
            };

            return Ok(response);
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
