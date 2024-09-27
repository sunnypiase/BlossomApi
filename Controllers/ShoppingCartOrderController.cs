using System.ComponentModel.DataAnnotations;
using BlossomApi.DB;
using BlossomApi.Models;
using BlossomApi.Services;
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
        // Новий метод для створення Offline замовлень, доступний лише адміністраторам
        [Authorize(Roles = "Admin")]
        [HttpPost("CreateOfflineOrder")]
        public async Task<IActionResult> CreateOfflineOrder([FromBody] OfflineOrderRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Розпочинаємо транзакцію
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Створюємо новий кошик для Offline замовлення без прив'язки до користувача
                var shoppingCart = await AddProductsToShoppingCart(new ShoppingCart { CreatedDate = DateTime.UtcNow }, request.Products);

                if (shoppingCart == null || !shoppingCart.ShoppingCartProducts.Any())
                {
                    return BadRequest("Не вдалося створити кошик або кошик порожній.");
                }

                // Створюємо замовлення з офлайн параметрами
                var order = CreateOfflineOrderFromRequest(request, shoppingCart.ShoppingCartId);

                // Додаємо замовлення до контексту
                _context.Orders.Add(order);

                // Додаємо інформацію про доставку з офлайн параметрами
                var deliveryInfo = new DeliveryInfo
                {
                    City = "Козин",
                    DepartmentNumber = "Офлайн",
                    Order = order
                };
                _context.DeliveryInfos.Add(deliveryInfo);

                // Отримуємо або створюємо кешбек, якщо вказано
                var cashback = await _cashbackService.GetOrCreateCashbackAsync(null, request.PhoneNumber ?? string.Empty);

                // Перевіряємо та застосовуємо кешбек, якщо вказано
                if (request.CashbackToUse > 0)
                {
                    var cashbackError = _cashbackService.ValidateAndApplyCashback(request.CashbackToUse, order, cashback);
                    if (!string.IsNullOrEmpty(cashbackError))
                    {
                        return BadRequest(cashbackError);
                    }
                }

                // Перевіряємо та застосовуємо промокод, якщо вказано
                if (!string.IsNullOrEmpty(request.UsedPromocode))
                {
                    var promocodeError = await _promocodeService.ValidateAndApplyPromocode(request.UsedPromocode, order);
                    if (!string.IsNullOrEmpty(promocodeError))
                    {
                        return BadRequest(promocodeError);
                    }
                }

                // Розраховуємо загальну ціну з урахуванням кешбеку та промокоду
                CalculateTotalPrice(order, shoppingCart, order.Promocode, cashback);

                // Оновлюємо залишки товарів
                UpdateProductStock(shoppingCart);

                // Оновлюємо баланс кешбеку після замовлення
                _cashbackService.UpdateCashbackBalance(order, cashback);

                // Зберігаємо всі зміни до бази даних в одному виклику
                await _context.SaveChangesAsync();

                // Підтверджуємо транзакцію
                await transaction.CommitAsync();

                return Ok(new { OrderId = order.OrderId });
            }
            catch (InvalidOperationException ex)
            {
                // Відкат транзакції при виникненні відомих помилок
                await transaction.RollbackAsync();
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                // Відкат транзакції при виникненні невідомих помилок
                await transaction.RollbackAsync();
                return StatusCode(500, "Сталася помилка під час створення офлайн замовлення.");
            }
        }

        private async Task<IActionResult> CreateOrderAsync(OrderBaseRequest request, SiteUser? siteUser)
        {
            // Розпочинаємо транзакцію
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                ShoppingCart? shoppingCart;

                // Визначаємо, чи користувач є гостьовим або авторизованим
                if (siteUser == null && request is GuestOrderRequest guestRequest)
                {
                    // Створюємо новий кошик для гостей та додаємо продукти
                    shoppingCart = await AddProductsToShoppingCart(new ShoppingCart { CreatedDate = DateTime.UtcNow }, guestRequest.Products);
                }
                else if (siteUser != null)
                {
                    // Отримуємо активний кошик для авторизованих користувачів
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

                // Створюємо замовлення з запиту
                var order = CreateOrderFromRequest(request, shoppingCart.ShoppingCartId);

                // Додаємо замовлення до контексту
                _context.Orders.Add(order);

                // Додаємо інформацію про доставку до контексту
                var deliveryInfo = new DeliveryInfo
                {
                    City = request.DeliveryInfo.City,
                    DepartmentNumber = request.DeliveryInfo.Department,
                    Order = order
                };
                _context.DeliveryInfos.Add(deliveryInfo);

                // Отримуємо або створюємо кешбек
                string phoneNumber = request.UserInfo.Phone;
                var cashback = await _cashbackService.GetOrCreateCashbackAsync(siteUser, phoneNumber);

                // Перевіряємо та застосовуємо кешбек
                var cashbackError = _cashbackService.ValidateAndApplyCashback(request.CashbackToUse, order, cashback);
                if (!string.IsNullOrEmpty(cashbackError))
                {
                    return BadRequest(cashbackError);
                }

                // Перевіряємо та застосовуємо промокод
                var promocodeError = await _promocodeService.ValidateAndApplyPromocode(request.UsedPromocode, order);
                if (!string.IsNullOrEmpty(promocodeError))
                {
                    return BadRequest(promocodeError);
                }

                // Розраховуємо загальну ціну з урахуванням кешбеку та промокоду
                CalculateTotalPrice(order, shoppingCart, order.Promocode, cashback);

                // Оновлюємо залишки товарів
                UpdateProductStock(shoppingCart);

                // Якщо користувач авторизований, створюємо новий кошик для майбутніх замовлень
                if (siteUser != null)
                {
                    var newShoppingCart = new ShoppingCart { SiteUserId = siteUser.UserId, CreatedDate = DateTime.UtcNow };
                    _context.ShoppingCarts.Add(newShoppingCart);
                }

                // Оновлюємо баланс кешбеку після замовлення
                _cashbackService.UpdateCashbackBalance(order, cashback);

                // Зберігаємо всі зміни до бази даних в одному виклику
                await _context.SaveChangesAsync();

                // Підтверджуємо транзакцію
                await transaction.CommitAsync();

                return Ok(new { OrderId = order.OrderId });
            }
            catch (InvalidOperationException ex)
            {
                // Відкат транзакції при виникненні відомих помилок
                await transaction.RollbackAsync();
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                // Відкат транзакції при виникненні невідомих помилок
                await transaction.RollbackAsync();
                return StatusCode(500, "Сталася помилка під час створення замовлення.");
            }
        }

        private async Task<ShoppingCart?> AddProductsToShoppingCart(ShoppingCart shoppingCart, List<OrderProductRequest> products)
        {
            // Додаємо новий кошик до контексту
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

            // Додаємо всі продукти до кошика
            await _context.ShoppingCartProducts.AddRangeAsync(shoppingCartProducts);
            await _context.SaveChangesAsync();

            // Перезавантажуємо кошик з продуктами для забезпечення коректних зв'язків
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
            product.NumberOfPurchases++;
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

            // Додаємо DeliveryInfo до контексту
            _context.DeliveryInfos.Add(deliveryInfo);

            return order;
        }
        private Order CreateOfflineOrderFromRequest(OfflineOrderRequest request, int shoppingCartId)
        {
            var order = new Order
            {
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Offline,
                Username = string.IsNullOrEmpty(request.Name) ? "Не вказано" : request.Name,
                Surname = string.IsNullOrEmpty(request.Surname) ? "Не вказано" : request.Surname,
                Email = string.IsNullOrEmpty(request.Email) ? "Не вказано" : request.Email,
                PhoneNumber = string.IsNullOrEmpty(request.PhoneNumber) ? "Не вказано" : request.PhoneNumber,
                DontCallMe = false, // Можна встановити за потребою
                EcoPackaging = false, // Можна встановити за потребою
                ShoppingCartId = shoppingCartId
            };

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

        // Моделі
        public class GuestOrderRequest : OrderBaseRequest
        {
            [Required(ErrorMessage = "Список товарів є обов'язковим.")]
            public List<OrderProductRequest> Products { get; set; }

        }

        public class AuthenticatedOrderRequest : OrderBaseRequest
        {
        }

        public abstract class OrderBaseRequest
        {
            public string? UsedPromocode { get; set; }

            [Required(ErrorMessage = "Інформація про користувача є обов'язковою.")]
            public UserInfoRequest UserInfo { get; set; }

            [Required(ErrorMessage = "Інформація про доставку є обов'язковою.")]
            public DeliveryInfoRequest DeliveryInfo { get; set; }

            public AdditionalInfoRequest? AdditionalInfo { get; set; }
            public decimal CashbackToUse { get; set; } // Сума кешбеку, яку користувач хоче використати

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
        public class OfflineOrderRequest
        {
            public string? Name { get; set; } // Ім'я
            public string? Surname { get; set; } // Прізвище
            [Phone(ErrorMessage = "Неправильний формат номера телефону.")]
            public string? PhoneNumber { get; set; } // Номер телефону
            [EmailAddress(ErrorMessage = "Неправильний формат електронної пошти.")]
            public string? Email { get; set; } // Електронна пошта
            public string? UsedPromocode { get; set; } // Промокод
            public decimal CashbackToUse { get; set; } = 0; // Кешбек для використання
            [Required(ErrorMessage = "Список товарів є обов'язковим.")]
            public List<OrderProductRequest> Products { get; set; } // Список товарів
        }
    }
}
