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
    public class OrdersController : ControllerBase
    {
        private readonly BlossomContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public OrdersController(BlossomContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
                .Where(o => o.ShoppingCart.SiteUserId == siteUser.UserId)
                .Select(o => ToOrderDto(o))
                .ToListAsync();

            return Ok(orders);
        }

        // GET: api/orders/statuses
        [Authorize(Roles = "Admin")]
        [HttpGet("statuses")]
        public async Task<IActionResult> GetOrdersByStatuses([FromQuery] List<OrderStatus> statuses)
        {
            var orders = await _context.Orders
                .Where(o => statuses.Contains(o.Status))
                .Select(o => ToOrderDto(o))
                .ToListAsync();

            return Ok(orders);
        }

        // GET: api/orders
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _context.Orders
                .Select(o => ToOrderDto(o))
                .ToListAsync();

            return Ok(orders);
        }

        // GET: api/orders/user/{userId}
        [Authorize(Roles = "Admin")]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUserId(int userId)
        {
            var orders = await _context.Orders
                .Where(o => o.ShoppingCart.SiteUserId == userId)
                .Select(o => ToOrderDto(o))
                .ToListAsync();

            return Ok(orders);
        }

        // PUT: api/orders/{orderId}/status
        [Authorize(Roles = "Admin")]
        [HttpPut("{orderId}/status")]
        public async Task<IActionResult> ChangeOrderStatus(int orderId, [FromBody] ChangeOrderStatusRequest request)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return NotFound("Order not found.");
            }

            order.Status = request.Status;
            await _context.SaveChangesAsync();

            return Ok(ToOrderDto(order));
        }

        private async Task<SiteUser?> GetCurrentUserAsync()
        {
            var identityUserId = _userManager.GetUserId(User);
            return await _context.SiteUsers.FirstOrDefaultAsync(u => u.IdentityUserId == identityUserId);
        }

        private static OrderDto ToOrderDto(Order order)
        {
            return new OrderDto
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                Status = order.Status,
                Username = order.Username,
                Surname = order.Surname,
                Email = order.Email,
                PhoneNumber = order.PhoneNumber,
                DontCallMe = order.DontCallMe,
                EcoPackaging = order.EcoPackaging,
                TotalPrice = order.TotalPrice,
                TotalDiscount = order.TotalDiscount,
                TotalPriceWithDiscount = order.TotalPriceWithDiscount,
                DiscountFromPromocode = order.DiscountFromPromocode,
                DiscountFromProductAction = order.DiscountFromProductAction,
                ShoppingCartId = order.ShoppingCartId,
                PromocodeId = order.PromocodeId,
                DeliveryInfo = order.DeliveryInfo != null ? new DeliveryInfoDto
                {
                    City = order.DeliveryInfo.City,
                    DepartmentNumber = order.DeliveryInfo.DepartmentNumber
                } : null
            };
        }
    }

    public class ChangeOrderStatusRequest
    {
        [Required] public OrderStatus Status { get; set; }
    }

    public class OrderDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public string Username { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool DontCallMe { get; set; }
        public bool EcoPackaging { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalPriceWithDiscount { get; set; }
        public decimal DiscountFromPromocode { get; set; }
        public decimal DiscountFromProductAction { get; set; }
        public int ShoppingCartId { get; set; }
        public int? PromocodeId { get; set; }
        public DeliveryInfoDto DeliveryInfo { get; set; }
    }

    public class DeliveryInfoDto
    {
        public string City { get; set; }
        public string DepartmentNumber { get; set; }
    }
}