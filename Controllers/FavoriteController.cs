using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlossomApi.DB;
using BlossomApi.Models;
using BlossomApi.Dtos;
using BlossomApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AutoMapper;

namespace BlossomApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController : ControllerBase
    {
        private readonly BlossomContext _context;
        private readonly IShownProductRepository _shownProductRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public FavoriteController(
            BlossomContext context,
            IShownProductRepository shownProductRepository,
            UserManager<IdentityUser> userManager,
            IMapper mapper)
        {
            _context = context;
            _shownProductRepository = shownProductRepository;
            _userManager = userManager;
            _mapper = mapper;
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
        [HttpDelete("Remove/{productId}")]
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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetFavoriteProducts()
        {
            var userId = (await GetCurrentUserAsync())?.UserId;
            var user = await _context.SiteUsers
                .Include(u => u.FavoriteProducts)
                .FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null) return NotFound("User not found");

            var favoriteProducts = _mapper.Map<List<ProductResponseDto>>(user.FavoriteProducts);

            return Ok(favoriteProducts);
        }

        private async Task<SiteUser?> GetCurrentUserAsync()
        {
            var identityUserId = _userManager.GetUserId(User);
            return await _context.SiteUsers.FirstOrDefaultAsync(u => u.IdentityUserId == identityUserId);
        }
    }
}
