using System.ComponentModel.DataAnnotations;
using BlossomApi.DB;
using BlossomApi.Models;
using BlossomApi.Services; // Додано для використання сервісів
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
        private readonly CashbackService _cashbackService;
        private readonly PromocodeService _promocodeService;

        public ShoppingCartOrderController(
            BlossomContext context,
            UserManager<IdentityUser> userManager,
            CashbackService cashbackService,
            PromocodeService promocodeService)
        {
            _context = context;
            _userManager = userManager;
            _cashbackService = cashbackService;
            _promocodeService = promocodeService;
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
                    throw new InvalidOperationException("Невірний контекст користувача.");
                }

                if (shoppingCart == null || !shoppingCart.ShoppingCartProducts.Any())
                {
                    return BadRequest("Не знайдено активного кошика покупок або кошик порожній.");
                }

                var order = CreateOrderFromRequest(request, shoppingCart.ShoppingCartId);

                // Отримання або створення запису кешбеку
                string phoneNumber = request.UserInfo.Phone;
                var cashback = await _cashbackService.GetOrCreateCashbackAsync(siteUser, phoneNumber);

                // Перевірка та застосування кешбеку
                var cashbackError = _cashbackService.ValidateAndApplyCashback(request, order, cashback);
                if (!string.IsNullOrEmpty(cashbackError))
                {
                    return BadRequest(cashbackError);
                }

                // Перевірка та застосування промокоду
                var promocodeError = await _promocodeService.ValidateAndApplyPromocode(request.UsedPromocode, order);
                if (!string.IsNullOrEmpty(promocodeError))
                {
                    return BadRequest(promocodeError);
                }

                // Розрахунок загальної ціни з урахуванням кешбеку та промокоду
                CalculateTotalPrice(order, shoppingCart, order.Promocode, cashback);

                // Оновлення залишків товарів
                UpdateProductStock(shoppingCart);

                // Збереження замовлення та оновлення балансу кешбеку
                await SaveOrderAndCommitTransaction(order, siteUser);

                // Оновлення балансу кешбеку після оформлення замовлення
                _cashbackService.UpdateCashbackBalance(order, cashback);

                await transaction.CommitAsync();

                return Ok(new { OrderId = order.OrderId });
            }
            catch (InvalidOperationException ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "Сталася помилка під час створення замовлення.");
            }
        }


        private async Task SaveOrderAndCommitTransaction(Order order, SiteUser? siteUser)
        {
            _context.Orders.Add(order);
            if (siteUser != null)
            {
                // Створення нового кошика покупок для користувача
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

            // Перезавантаження кошика з продуктами для забезпечення коректних зв'язків
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
                throw new InvalidOperationException("Товар не знайдено.");
            }

            if (!product.InStock || product.AvailableAmount < requestedAmount)
            {
                throw new InvalidOperationException("Товар недоступний або недостатня кількість на складі.");
            }
        }

        private Product UpdateProductStockAfterAdd(Product product, int requestedAmount)
        {
            int availableAmount = product.AvailableAmount - requestedAmount;
            if (availableAmount < 0)
            {
                throw new InvalidOperationException("Недостатня кількість товару на складі.");
            }

            product.AvailableAmount = availableAmount;

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
                DontCallMe = request.AdditionalInfo?.DontCallMe ?? false,
                EcoPackaging = request.AdditionalInfo?.EcoPackaging ?? false,
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

        private void CalculateTotalPrice(Order order, ShoppingCart shoppingCart, Promocode? promocode, Cashback cashback)
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

            // Загальна знижка з урахуванням кешбеку
            order.TotalPrice = totalPrice;
            order.DiscountFromProductAction = discountFromProductAction;
            order.DiscountFromPromocode = discountFromPromocode;
            order.TotalDiscount = discountFromProductAction + discountFromPromocode + order.DiscountFromCashback;
            order.TotalPriceWithDiscount = totalPrice - order.TotalDiscount;
        }

        // Модель запиту для гостьових замовлень, включає використання кешбеку
        public class GuestOrderRequest : OrderBaseRequest
        {
            [Required(ErrorMessage = "Список товарів є обов'язковим.")]
            public List<OrderProductRequest> Products { get; set; }

            public decimal CashbackToUse { get; set; } // Сума кешбеку, яку гість хоче використати
        }

        // Модель запиту для замовлень авторизованих користувачів, включає використання кешбеку
        public class AuthenticatedOrderRequest : OrderBaseRequest
        {
            public decimal CashbackToUse { get; set; } // Сума кешбеку, яку користувач хоче використати
        }

        public abstract class OrderBaseRequest
        {
            public string? UsedPromocode { get; set; }

            [Required(ErrorMessage = "Інформація про користувача є обов'язковою.")]
            public UserInfoRequest UserInfo { get; set; }

            [Required(ErrorMessage = "Інформація про доставку є обов'язковою.")]
            public DeliveryInfoRequest DeliveryInfo { get; set; }

            public AdditionalInfoRequest? AdditionalInfo { get; set; }
        }

        public class OrderProductRequest
        {
            [Required(ErrorMessage = "ID товару є обов'язковим.")]
            public int ProductId { get; set; }

            [Required(ErrorMessage = "Кількість товару є обов'язковою.")]
            [Range(1, int.MaxValue, ErrorMessage = "Кількість товару повинна бути більшою за 0.")]
            public int ProductAmount { get; set; }
        }

        public class UserInfoRequest
        {
            [Required(ErrorMessage = "Ім'я є обов'язковим.")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Номер телефону є обов'язковим.")]
            [Phone(ErrorMessage = "Неправильний формат номера телефону.")]
            public string Phone { get; set; }

            [Required(ErrorMessage = "Прізвище є обов'язковим.")]
            public string Surname { get; set; }

            [EmailAddress(ErrorMessage = "Неправильний формат електронної пошти.")]
            public string Email { get; set; }
        }

        public class DeliveryInfoRequest
        {
            [Required(ErrorMessage = "Місто є обов'язковим.")]
            public string City { get; set; }

            [Required(ErrorMessage = "Номер відділення є обов'язковим.")]
            public string Department { get; set; }
        }

        public class AdditionalInfoRequest
        {
            public bool DontCallMe { get; set; } = false;
            public bool EcoPackaging { get; set; } = false;
        }
    }
}
