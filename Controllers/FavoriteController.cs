using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlossomApi.DB;
using BlossomApi.Models;
using BlossomApi.Dtos;
using BlossomApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BlossomApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController : ControllerBase
    {
        private readonly BlossomContext _context;
        private readonly IShownProductRepository _shownProductRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public FavoriteController(BlossomContext context, IShownProductRepository shownProductRepository, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _shownProductRepository = shownProductRepository;
            _userManager = userManager;
        }

        [Authorize]
        // POST: api/Favorite/Add/{productId}
        [HttpPost("Add/{productId}")]
        public async Task<IActionResult> AddFavoriteProduct(int productId)
        {
            var userId = (await GetCurrentUserAsync())?.UserId;
            var user = await _context.SiteUsers
                .Include(u => u.FavoriteProducts)
                .FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null) return NotFound("User not found");

            var product = await _shownProductRepository.GetProducts().FirstOrDefaultAsync(x => x.ProductId == productId);
            if (product == null) return NotFound("Product not found");

            if (user.FavoriteProducts.Any(p => p.ProductId == productId))
            {
                return BadRequest("Product is already in favorites");
            }

            user.FavoriteProducts.Add(product);
            await _context.SaveChangesAsync();

            return Ok("Product added to favorites");
        }

        [Authorize]
        // DELETE: api/Favorite/Remove/{productId}
        [HttpDelete("/Remove/{productId}")]
        public async Task<IActionResult> RemoveFavoriteProduct(int productId)
        {
            var userId = (await GetCurrentUserAsync())?.UserId;
            var user = await _context.SiteUsers
                .Include(u => u.FavoriteProducts)
                .FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null) return NotFound("User not found");

            var product = user.FavoriteProducts.FirstOrDefault(p => p.ProductId == productId);
            if (product == null) return NotFound("Product not found in favorites");

            user.FavoriteProducts.Remove(product);
            await _context.SaveChangesAsync();

            return Ok("Product removed from favorites");
        }

        [Authorize]
        // GET: api/Favorite
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetFavoriteProducts()
        {
            var userId = (await GetCurrentUserAsync())?.UserId;
            var user = await _context.SiteUsers
                .Include(u => u.FavoriteProducts)
                .FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null) return NotFound("User not found");

            var favoriteProducts = user.FavoriteProducts.Select(p => MapToProductResponseDto(p)).ToList();

            return Ok(favoriteProducts);
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
        private async Task<SiteUser?> GetCurrentUserAsync()
        {
            var identityUserId = _userManager.GetUserId(User);
            return await _context.SiteUsers.FirstOrDefaultAsync(u => u.IdentityUserId == identityUserId);
        }
    }
}
