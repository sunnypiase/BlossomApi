using BlossomApi.DB;
using BlossomApi.Dtos.Orders;
using BlossomApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlossomApi.Services
{
    public class OrderFilterPanelService
    {
        private readonly BlossomContext _context;

        public OrderFilterPanelService(BlossomContext context)
        {
            _context = context;
        }

        public async Task<AdminOrderFilterPanelResponseDto> GetFilterPanelDataAsync()
        {
            // Fetch all orders first
            var orders = await _context.Orders.ToListAsync();

            // Get all statuses from the enum
            var allStatuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>();

            // Generate status options, including all statuses from the enum
            var statusOptions = allStatuses.Select(status => new OrderStatusOptionDto
            {
                Status = (int)status,
                StatusName = status.ToUkrainianName(),
                OrdersCount = orders.Count(o => o.Status == status)
            }).ToList();

            // Determine the minimum and maximum order date
            var minOrderDate = orders.Any() ? orders.Min(o => o.OrderDate) : default;
            var maxOrderDate = orders.Any() ? orders.Max(o => o.OrderDate) : default;

            // Determine the minimum and maximum total price
            var minTotalPrice = orders.Any() ? orders.Min(o => o.TotalPrice) : default;
            var maxTotalPrice = orders.Any() ? orders.Max(o => o.TotalPrice) : default;

            return new AdminOrderFilterPanelResponseDto
            {
                StatusOptions = statusOptions,
                MinOrderDate = minOrderDate,
                MaxOrderDate = maxOrderDate,
                MinTotalPrice = minTotalPrice,
                MaxTotalPrice = maxTotalPrice
            };
        }
    }
}
