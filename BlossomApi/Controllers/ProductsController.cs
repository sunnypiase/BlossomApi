using BlossomApi.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlossomApi.Models;
using BlossomApi.Dtos;

namespace BlossomApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly BlossomContext _context;

        public ProductController(BlossomContext context)
        {
            _context = context;
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
                    Image = p.Image,
                    Brand = p.Brand,
                    Price = p.Price,
                    Discount = p.Discount,
                    IsNew = p.IsNew,
                    Rating = p.Rating,
                    InStock = p.InStock,
                    AvailableAmount = p.AvailableAmount,
                    Description = p.Description,
                    Categories = p.Categories.Select(c => c.Name).ToList()
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
                    Image = p.Image,
                    Brand = p.Brand,
                    Price = p.Price,
                    Discount = p.Discount,
                    IsNew = p.IsNew,
                    Rating = p.Rating,
                    InStock = p.InStock,
                    AvailableAmount = p.AvailableAmount,
                    Description = p.Description,
                    Categories = p.Categories.Select(c => c.Name).ToList()
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
            product.Image = productDto.Image;
            product.Brand = productDto.Brand;
            product.Price = productDto.Price;
            product.Discount = productDto.Discount;
            product.IsNew = productDto.IsNew;
            product.Rating = productDto.Rating;
            product.InStock = productDto.InStock;
            product.AvailableAmount = productDto.AvailableAmount;
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
                    Image = productDto.Image,
                    Brand = productDto.Brand,
                    Price = productDto.Price,
                    Discount = productDto.Discount,
                    IsNew = productDto.IsNew,
                    Rating = productDto.Rating,
                    InStock = productDto.InStock,
                    AvailableAmount = productDto.AvailableAmount,
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
                    Image = product.Image,
                    Brand = product.Brand,
                    Price = product.Price,
                    Discount = product.Discount,
                    IsNew = product.IsNew,
                    Rating = product.Rating,
                    InStock = product.InStock,
                    AvailableAmount = product.AvailableAmount,
                    Description = product.Description,
                    Categories = product.Categories.Select(c => c.Name).ToList()
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
        public async Task<ActionResult<IEnumerable<GetProductsByFilterResponseDto>>> GetProductsByFilter(GetProductsByFilterRequestDto request)
        {
            var query = _context.Products.AsQueryable();

            if (request.Categories is { Count: > 0 })
            {
                query = query.Where(p => p.Categories.Any(c => request.Categories.Contains(c.Name)));
            }

            if (request.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= request.MinPrice.Value);
            }

            if (request.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= request.MaxPrice.Value);
            }

            if (!string.IsNullOrEmpty(request.Search))
            {
                var searchPattern = $"%{request.Search}%";
                query = query.Where(p => EF.Functions.Like(p.Name, searchPattern));
            }

            query = request.SortBy switch
            {
                "popularity" => query.OrderByDescending(p => p.Rating), // Assuming popularity is determined by rating
                "price_asc" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                _ => query.OrderBy(p => p.Name)
            };

            var products = await query
                .Skip(request.Start)
                .Take(request.Amount)
                .Select(p => new GetProductsByFilterResponseDto
                {
                    Id = p.ProductId,
                    Name = p.Name,
                    NameEng = p.NameEng,
                    Image = p.Image,
                    Brand = p.Brand,
                    Price = p.Price,
                    Discount = p.Discount,
                    IsNew = p.IsNew,
                    Rating = p.Rating,
                    InStock = p.InStock,
                    Categories = p.Categories.Select(c => c.Name).ToList()
                })
                .ToListAsync();

            return Ok(products);
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
