using AutoMapper;
using BlossomApi.DB;
using BlossomApi.Dtos.Orders;
using BlossomApi.Models;
using BlossomApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BlossomApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly BlossomContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public OrdersController(BlossomContext context, UserManager<IdentityUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        // GET: api/orders/user
        [Authorize]
        [HttpGet("user")]
        public async Task<IActionResult> GetOrdersByUser()
        {
            var siteUser = await GetCurrentUserAsync();
            if (siteUser == null)
            {
                return Unauthorized();
            }

            var orders = await _context.Orders
                .Include(o => o.Promocode)
                .Include(o => o.DeliveryInfo)
                .Include(o => o.ShoppingCart)
                    .ThenInclude(sc => sc.ShoppingCartProducts)
                        .ThenInclude(scp => scp.Product)
                .Where(o => o.ShoppingCart.SiteUserId == siteUser.UserId)
                .ToListAsync();

            var orderDtos = _mapper.Map<List<OrderDto>>(orders);

            return Ok(orderDtos);
        }
        // GET: api/orders/user/history
        [Authorize]
        [HttpGet("user/history")]
        public async Task<IActionResult> GetUserOrderHistory()
        {
            var siteUser = await GetCurrentUserAsync();
            if (siteUser == null)
            {
                return Unauthorized();
            }

            var orders = await _context.Orders
                .Where(o => o.ShoppingCart.SiteUserId == siteUser.UserId)
                .ToListAsync();

            var orderDtos = _mapper.Map<List<UserOrderSummaryDto>>(orders);

            return Ok(orderDtos);
        }
        // GET: api/orders/{orderId}/products
        [Authorize]
        [HttpGet("{orderId}/products")]
        public async Task<IActionResult> GetOrderProducts(int orderId)
        {
            var siteUser = await GetCurrentUserAsync();
            if (siteUser == null)
            {
                return Unauthorized();
            }

            var order = await _context.Orders
                .Include(o => o.ShoppingCart)
                    .ThenInclude(sc => sc.ShoppingCartProducts)
                        .ThenInclude(scp => scp.Product)
                            .ThenInclude(p => p.Categories)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            // Ensure that the user is authorized to view this order
            if (order.ShoppingCart.SiteUserId != siteUser.UserId)
            {
                return Forbid();
            }

            var products = order.ShoppingCart.ShoppingCartProducts;

            var productDtos = _mapper.Map<List<ProductInOrderDetailDto>>(products);

            return Ok(productDtos);
        }


        // GET: api/orders/statuses?statuses=Created&statuses=Processing
        [Authorize(Roles = "Admin")]
        [HttpGet("statuses")]
        public async Task<IActionResult> GetOrdersByStatuses([FromQuery] List<OrderStatus> statuses)
        {
            var orders = await _context.Orders
                .Include(o => o.Promocode)
                .Include(o => o.DeliveryInfo)
                .Include(o => o.ShoppingCart)
                    .ThenInclude(sc => sc.ShoppingCartProducts)
                        .ThenInclude(scp => scp.Product)
                .Where(o => statuses.Contains(o.Status))
                .ToListAsync();

            var orderDtos = _mapper.Map<List<OrderDto>>(orders);

            return Ok(orderDtos);
        }

        // GET: api/orders
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.Promocode)
                .Include(o => o.DeliveryInfo)
                .Include(o => o.ShoppingCart)
                    .ThenInclude(sc => sc.ShoppingCartProducts)
                        .ThenInclude(scp => scp.Product)
                .ToListAsync();

            var orderDtos = _mapper.Map<List<OrderDto>>(orders);

            return Ok(orderDtos);
        }

        // GET: api/orders/user/{userId}
        [Authorize(Roles = "Admin")]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUserId(int userId)
        {
            var orders = await _context.Orders
                .Include(o => o.Promocode)
                .Include(o => o.DeliveryInfo)
                .Include(o => o.ShoppingCart)
                    .ThenInclude(sc => sc.ShoppingCartProducts)
                        .ThenInclude(scp => scp.Product)
                .Where(o => o.ShoppingCart.SiteUserId == userId)
                .ToListAsync();

            var orderDtos = _mapper.Map<List<OrderDto>>(orders);

            return Ok(orderDtos);
        }

        // PUT: api/orders/{orderId}/status
        [Authorize(Roles = "Admin")]
        [HttpPut("{orderId}/status")]
        public async Task<IActionResult> ChangeOrderStatus(int orderId, [FromBody] ChangeOrderStatusRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Розпочинаємо транзакцію
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var order = await _context.Orders
                    .Include(o => o.ShoppingCart)
                        .ThenInclude(sc => sc.ShoppingCartProducts)
                            .ThenInclude(scp => scp.Product)
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);

                if (order == null)
                {
                    return NotFound("Замовлення не знайдено.");
                }

                // Перевірка дозволених переходів статусів
                if (!IsValidStatusTransition(order.Status, request.Status))
                {
                    return BadRequest($"Неможливо змінити статус з '{order.Status}' на '{request.Status}'.");
                }

                // Якщо новий статус Declined або Refund, повертаємо кількість товарів на склад
                if (request.Status == OrderStatus.Declined || request.Status == OrderStatus.Refund)
                {
                    foreach (var cartProduct in order.ShoppingCart.ShoppingCartProducts)
                    {
                        cartProduct.Product.AvailableAmount += cartProduct.Quantity;
                        _context.Products.Update(cartProduct.Product);
                    }
                }

                // Змінюємо статус замовлення
                order.Status = request.Status;
                _context.Orders.Update(order);

                // Зберігаємо всі зміни
                await _context.SaveChangesAsync();

                // Підтверджуємо транзакцію
                await transaction.CommitAsync();

                var orderDto = _mapper.Map<OrderDto>(order);
                return Ok(orderDto);
            }
            catch (InvalidOperationException ex)
            {
                // Відкат транзакції при відомих помилках
                await transaction.RollbackAsync();
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Відкат транзакції при невідомих помилках
                await transaction.RollbackAsync();
                return StatusCode(500, "Сталася помилка під час зміни статусу замовлення.");
            }
        }

        private bool IsValidStatusTransition(OrderStatus currentStatus, OrderStatus newStatus)
        {
            var allowedTransitions = new Dictionary<OrderStatus, List<OrderStatus>>
            {
                { OrderStatus.Created, new List<OrderStatus> { OrderStatus.Accepted, OrderStatus.Declined } },
                { OrderStatus.Accepted, new List<OrderStatus> { OrderStatus.NeedToShip, OrderStatus.Declined } },
                { OrderStatus.NeedToShip, new List<OrderStatus> { OrderStatus.Shipped, OrderStatus.Declined } },
                { OrderStatus.Shipped, new List<OrderStatus> { OrderStatus.Completed, OrderStatus.Refund } },
                { OrderStatus.Completed, new List<OrderStatus>() }, // Завершений статус - подальші зміни не дозволені
                { OrderStatus.Declined, new List<OrderStatus>() }, // Відхилений статус - подальші зміни не дозволені
                { OrderStatus.Refund, new List<OrderStatus>() } // Повернення - подальші зміни не дозволені
            };

            if (allowedTransitions.ContainsKey(currentStatus))
            {
                return allowedTransitions[currentStatus].Contains(newStatus);
            }

            return false;
        }


        // GET: api/orders/{orderId}
        [Authorize]
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.Promocode)
                .Include(o => o.DeliveryInfo)
                .Include(o => o.ShoppingCart)
                    .ThenInclude(sc => sc.ShoppingCartProducts)
                        .ThenInclude(scp => scp.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound("Замовлення не знайдено.");
            }

            var orderDetailsDto = _mapper.Map<OrderDetailsDto>(order);

            return Ok(orderDetailsDto);
        }

        private async Task<SiteUser?> GetCurrentUserAsync()
        {
            var identityUserId = _userManager.GetUserId(User);
            return await _context.SiteUsers
                .Include(u => u.IdentityUser)
                .FirstOrDefaultAsync(u => u.IdentityUserId == identityUserId);
        }
    }
}
