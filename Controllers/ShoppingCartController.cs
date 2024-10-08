using System.ComponentModel.DataAnnotations;
using BlossomApi.DB;
using BlossomApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlossomApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly BlossomContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ShoppingCartController(BlossomContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProductToCart([FromBody] AddProductRequest request)
        {
            var siteUser = await GetCurrentUserAsync();
            if (siteUser == null)
            {
                return Unauthorized();
            }

            var product = await _context.Products.FindAsync(request.ProductId);
            if (product is not { InStock: true } || product.AvailableAmount < 1)
            {
                return BadRequest("Товар недоступний або відсутній на складі.");
            }

            var shoppingCart = await GetOrCreateActiveShoppingCartAsync(siteUser.UserId);
            var existingProduct = shoppingCart.ShoppingCartProducts.FirstOrDefault(p => p.ProductId == request.ProductId);

            if (existingProduct != null)
            {
                existingProduct.Quantity += 1;
            }
            else
            {
                var shoppingCartProduct = new ShoppingCartProduct
                {
                    ShoppingCartId = shoppingCart.ShoppingCartId,
                    ProductId = product.ProductId,
                    Quantity = 1
                };

                shoppingCart.ShoppingCartProducts.Add(shoppingCartProduct);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize]
        [HttpPost("ChangeProductAmount")]
        public async Task<IActionResult> ChangeProductAmountInCart([FromBody] ChangeProductAmountRequest request)
        {
            if (request.Quantity <= 0)
            {
                return BadRequest("Кількість повинна бути більше 0.");
            }

            var siteUser = await GetCurrentUserAsync();
            if (siteUser == null)
            {
                return Unauthorized();
            }

            var shoppingCart = await GetActiveShoppingCartAsync(siteUser.UserId);
            if (shoppingCart == null)
            {
                return BadRequest("Не знайдено активного кошика покупок.");
            }

            var shoppingCartProduct = shoppingCart.ShoppingCartProducts.FirstOrDefault(p => p.ProductId == request.ProductId);
            if (shoppingCartProduct == null)
            {
                return BadRequest("Товар не знайдено у кошику.");
            }

            var product = await _context.Products.FindAsync(request.ProductId);
            if (product == null || !product.InStock || product.AvailableAmount < request.Quantity)
            {
                return BadRequest("Товар недоступний або відсутній на складі.");
            }

            shoppingCartProduct.Quantity = request.Quantity;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize]
        [HttpPost("RemoveProduct")]
        public async Task<IActionResult> RemoveProductFromCart([FromBody] RemoveProductRequest request)
        {
            var siteUser = await GetCurrentUserAsync();
            if (siteUser == null)
            {
                return Unauthorized();
            }

            var shoppingCart = await GetActiveShoppingCartAsync(siteUser.UserId);
            if (shoppingCart == null)
            {
                return BadRequest("Не знайдено активного кошика покупок.");
            }

            var shoppingCartProduct = await _context.ShoppingCartProducts
                .FirstOrDefaultAsync(p => p.ShoppingCartId == shoppingCart.ShoppingCartId && p.ProductId == request.ProductId);

            if (shoppingCartProduct == null)
            {
                return BadRequest("Товар не знайдено у кошику.");
            }

            _context.ShoppingCartProducts.Remove(shoppingCartProduct);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize]
        [HttpGet("GetUserShoppingCart")]
        public async Task<IActionResult> GetUserShoppingCart()
        {
            var siteUser = await GetCurrentUserAsync();
            if (siteUser == null)
            {
                return Unauthorized();
            }

            var shoppingCart = await GetOrCreateActiveShoppingCartAsync(siteUser.UserId);

            return Ok(shoppingCart.ShoppingCartProducts.Select(p => new
            {
                p.ProductId,
                p.Product.Name,
                p.Quantity,
                p.Product.Price,
                p.Product.Discount,
                p.Product.Images
            }));
        }

        private async Task<SiteUser?> GetCurrentUserAsync()
        {
            var identityUserId = _userManager.GetUserId(User);
            return await _context.SiteUsers.FirstOrDefaultAsync(u => u.IdentityUserId == identityUserId);
        }

        private async Task<ShoppingCart> GetOrCreateActiveShoppingCartAsync(int userId)
        {
            var shoppingCart = await _context.ShoppingCarts
                .Include(sc => sc.ShoppingCartProducts)
                .ThenInclude(scp => scp.Product)
                .FirstOrDefaultAsync(sc => sc.SiteUserId == userId && sc.Order == null);

            if (shoppingCart != null)
            {
                return shoppingCart;
            }

            shoppingCart = new ShoppingCart
            {
                SiteUserId = userId,
                CreatedDate = DateTime.UtcNow
            };
            _context.ShoppingCarts.Add(shoppingCart);
            await _context.SaveChangesAsync();

            return shoppingCart;
        }

        private async Task<ShoppingCart?> GetActiveShoppingCartAsync(int userId)
        {
            return await _context.ShoppingCarts
                .Include(sc => sc.ShoppingCartProducts)
                .ThenInclude(scp => scp.Product)
                .FirstOrDefaultAsync(sc => sc.SiteUserId == userId && sc.Order == null);
        }

        public class AddProductRequest
        {
            [Required(ErrorMessage = "ID товару є обов'язковим.")]
            public int ProductId { get; set; }
        }

        public class ChangeProductAmountRequest
        {
            [Required(ErrorMessage = "ID товару є обов'язковим.")]
            public int ProductId { get; set; }

            [Required(ErrorMessage = "Кількість є обов'язковою.")]
            [Range(1, int.MaxValue, ErrorMessage = "Кількість повинна бути більшою за 0.")]
            public int Quantity { get; set; }
        }

        public class RemoveProductRequest
        {
            [Required(ErrorMessage = "ID товару є обов'язковим.")]
            public int ProductId { get; set; }
        }
    }
}
