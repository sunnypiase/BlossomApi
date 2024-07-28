using System.ComponentModel.DataAnnotations;
using BlossomApi.AttributeValidations;
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
    public class ShoppingCartOrderController : ControllerBase
    {
        private readonly BlossomContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ShoppingCartOrderController(BlossomContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("CreateOrderForGuest")]
        public async Task<IActionResult> CreateOrderForGuest([FromBody] GuestOrderRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await CreateOrderAsync(request, null);
        }

        [Authorize]
        [HttpPost("CreateOrderForUser")]
        public async Task<IActionResult> CreateOrderForUser([FromBody] AuthenticatedOrderRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var siteUser = await GetCurrentUserAsync();
            if (siteUser == null)
            {
                return Unauthorized();
            }

            return await CreateOrderAsync(request, siteUser);
        }

        private async Task<IActionResult> CreateOrderAsync(OrderBaseRequest request, SiteUser? siteUser)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                ShoppingCart? shoppingCart;
                if (siteUser == null && request is GuestOrderRequest guestRequest)
                {
                    shoppingCart = await AddProductsToShoppingCart(new ShoppingCart { CreatedDate = DateTime.UtcNow }, guestRequest.Products);
                }
                else if (siteUser != null)
                {
                    shoppingCart = await GetActiveShoppingCartAsync(siteUser.UserId);
                }
                else
                {
                    throw new InvalidOperationException("Invalid user context.");
                }

                if (shoppingCart == null || !shoppingCart.ShoppingCartProducts.Any())
                {
                    return BadRequest("No active shopping cart found or the cart is empty.");
                }

                var order = CreateOrderFromRequest(request, shoppingCart.ShoppingCartId);

                var promocodeError = await ValidateAndApplyPromocode(request.UsedPromocode, order);
                if (!string.IsNullOrEmpty(promocodeError))
                {
                    return BadRequest(promocodeError);
                }

                CalculateTotalPrice(order, shoppingCart, order.Promocode);
                UpdateProductStock(shoppingCart);

                await SaveOrderAndCommitTransaction(order, siteUser);

                await transaction.CommitAsync();

                return Ok(new { OrderId = order.OrderId });
            }
            catch (InvalidOperationException ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "An error occurred while creating the order.");
            }
        }

        private async Task SaveOrderAndCommitTransaction(Order order, SiteUser? siteUser)
        {
            _context.Orders.Add(order);
            if (siteUser != null)
            {
                _context.ShoppingCarts.Add(new ShoppingCart { SiteUserId = siteUser.UserId, CreatedDate = DateTime.UtcNow });
            }

            await _context.SaveChangesAsync();
        }

        private async Task<ShoppingCart?> AddProductsToShoppingCart(ShoppingCart shoppingCart, List<OrderProductRequest> products)
        {
            var addedShoppingCart = _context.ShoppingCarts.Add(shoppingCart).Entity;
            await _context.SaveChangesAsync();

            var shoppingCartProducts = new List<ShoppingCartProduct>();

            foreach (var productRequest in products)
            {
                var product = await _context.Products.FindAsync(productRequest.ProductId);
                ValidateProductAvailability(product, productRequest.ProductAmount);

                var shoppingCartProduct = new ShoppingCartProduct
                {
                    ShoppingCartId = addedShoppingCart.ShoppingCartId,
                    ProductId = product!.ProductId,
                    Quantity = productRequest.ProductAmount
                };

                shoppingCartProducts.Add(shoppingCartProduct);
            }

            await _context.ShoppingCartProducts.AddRangeAsync(shoppingCartProducts);
            await _context.SaveChangesAsync();

            // Reload the ShoppingCart with the products to ensure the relationships are correctly established
            addedShoppingCart = await _context.ShoppingCarts
                .Include(sc => sc.ShoppingCartProducts)
                .ThenInclude(scp => scp.Product)
                .FirstOrDefaultAsync(sc => sc.ShoppingCartId == addedShoppingCart.ShoppingCartId);

            return addedShoppingCart;
        }

        private void ValidateProductAvailability(Product? product, int requestedAmount)
        {
            if (product == null)
            {
                throw new InvalidOperationException("Product not found.");
            }

            if (!product.InStock || product.AvailableAmount < requestedAmount)
            {
                throw new InvalidOperationException("Product not available or insufficient stock.");
            }
        }

        private Product UpdateProductStockAfterAdd(Product product, int requestedAmount)
        {
            product.AvailableAmount -= requestedAmount;

            product.InStock = product.AvailableAmount switch
            {
                0 => false,
                < 0 => throw new InvalidOperationException("Insufficient stock."),
                _ => product.InStock
            };

            return product;
        }

        private void UpdateProductStock(ShoppingCart shoppingCart)
        {
            var productUpdates = new List<Product>();
            foreach (var cartProduct in shoppingCart.ShoppingCartProducts)
            {
                productUpdates.Add(UpdateProductStockAfterAdd(cartProduct.Product, cartProduct.Quantity));
            }

            _context.Products.UpdateRange(productUpdates);
        }

        private async Task<SiteUser?> GetCurrentUserAsync()
        {
            var identityUserId = _userManager.GetUserId(User);
            return await _context.SiteUsers.FirstOrDefaultAsync(u => u.IdentityUserId == identityUserId);
        }

        private async Task<ShoppingCart?> GetActiveShoppingCartAsync(int userId)
        {
            return await _context.ShoppingCarts
                .Include(sc => sc.ShoppingCartProducts)
                .ThenInclude(scp => scp.Product)
                .FirstOrDefaultAsync(sc => sc.SiteUserId == userId && sc.Order == null);
        }

        private async Task<string> ValidateAndApplyPromocode(string usedPromocode, Order order)
        {
            if (string.IsNullOrEmpty(usedPromocode))
            {
                return string.Empty;
            }

            var promocode = await _context.Promocodes.FirstOrDefaultAsync(p => p.Code == usedPromocode);
            if (promocode == null)
            {
                return "Промокод не знайдено.";
            }

            if (promocode.ExpirationDate < DateTime.UtcNow)
            {
                return "Промокод прострочений.";
            }

            if (promocode.UsageLeft <= 0)
            {
                return "Промокод більше не доступний.";
            }

            order.PromocodeId = promocode.PromocodeId;
            order.Promocode = promocode;
            promocode.UsageLeft--;
            _context.Promocodes.Update(promocode);

            return string.Empty;
        }

        private Order CreateOrderFromRequest(OrderBaseRequest request, int shoppingCartId)
        {
            var order = new Order
            {
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Created,
                Username = request.UserInfo.Name,
                Surname = request.UserInfo.Surname,
                Email = request.UserInfo.Email,
                PhoneNumber = request.UserInfo.Phone,
                DontCallMe = request.AdditionalInfo.DontCallMe,
                EcoPackaging = request.AdditionalInfo.EcoPackaging,
                ShoppingCartId = shoppingCartId
            };

            var deliveryInfo = new DeliveryInfo
            {
                City = request.DeliveryInfo.City,
                DepartmentNumber = request.DeliveryInfo.Department,
                Order = order
            };

            _context.DeliveryInfos.Add(deliveryInfo);

            return order;
        }

        private void CalculateTotalPrice(Order order, ShoppingCart shoppingCart, Promocode? promocode)
        {
            decimal totalPrice = 0;
            decimal discountFromProductAction = 0;

            foreach (var shoppingCartProduct in shoppingCart.ShoppingCartProducts)
            {
                var product = shoppingCartProduct.Product;
                var productTotalPrice = product.Price * shoppingCartProduct.Quantity;
                var productDiscountValue = productTotalPrice * product.Discount / 100;

                totalPrice += productTotalPrice;
                discountFromProductAction += productDiscountValue;
            }

            decimal discountFromPromocode = 0;
            if (promocode != null)
            {
                discountFromPromocode = (totalPrice - discountFromProductAction) * promocode.Discount / 100;
            }

            order.TotalPrice = totalPrice;
            order.TotalDiscount = discountFromProductAction + discountFromPromocode;
            order.DiscountFromPromocode = discountFromPromocode;
            order.DiscountFromProductAction = discountFromProductAction;
            order.TotalPriceWithDiscount = totalPrice - order.TotalDiscount;
        }

        public class GuestOrderRequest : OrderBaseRequest
        {
            [Required] public List<OrderProductRequest> Products { get; set; }
        }

        public class AuthenticatedOrderRequest : OrderBaseRequest
        {
            // No additional properties needed for authenticated requests
        }

        public abstract class OrderBaseRequest
        {
            public string UsedPromocode { get; set; }
            [Required] public UserInfoRequest UserInfo { get; set; }
            [Required] public DeliveryInfoRequest DeliveryInfo { get; set; }
            public AdditionalInfoRequest AdditionalInfo { get; set; }
        }

        public class OrderProductRequest
        {
            [Required] public int ProductId { get; set; }
            [Required] public int ProductAmount { get; set; }
        }

        public class UserInfoRequest
        {
            [Required] public string Name { get; set; }
            [Required] [PhoneNumber] public string Phone { get; set; }
            [Required] public string Surname { get; set; }
            [EmailAddress] public string Email { get; set; }
        }

        public class DeliveryInfoRequest
        {
            [Required] public string City { get; set; }
            [Required] public string Department { get; set; }
        }

        public class AdditionalInfoRequest
        {
            public bool DontCallMe { get; set; }
            public bool EcoPackaging { get; set; }
        }
    }
}