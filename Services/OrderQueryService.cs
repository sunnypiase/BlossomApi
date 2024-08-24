using BlossomApi.DB;
using BlossomApi.Dtos.Orders;
using BlossomApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace BlossomApi.Services
{
    public class OrderQueryService
    {
        private readonly BlossomContext _context;

        public OrderQueryService(BlossomContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<Order>> ApplyFilterAndSortAsync(GetOrdersByAdminFilterRequestDto request)
        {
            var query = _context.Orders.AsQueryable();

            // Case-insensitive search by Username, Surname, PhoneNumber, Email, or City
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                var lowerSearchTerm = request.SearchTerm.ToLower();
                query = query.Where(o =>
                    o.Username.ToLower().Contains(lowerSearchTerm) ||
                    o.Surname.ToLower().Contains(lowerSearchTerm) ||
                    o.PhoneNumber.ToLower().Contains(lowerSearchTerm) ||
                    o.Email.ToLower().Contains(lowerSearchTerm) ||
                    (o.DeliveryInfo != null && o.DeliveryInfo.City.ToLower().Contains(lowerSearchTerm))
                );
            }

            // Filter by statuses
            if (request.Statuses != null && request.Statuses.Count > 0)
            {
                query = query.Where(o => request.Statuses.Contains(o.Status));
            }

            // Filter by order date
            if (request.MinOrderDate.HasValue)
            {
                query = query.Where(o => o.OrderDate >= request.MinOrderDate.Value);
            }

            if (request.MaxOrderDate.HasValue)
            {
                query = query.Where(o => o.OrderDate <= request.MaxOrderDate.Value);
            }

            // Filter by total price
            if (request.MinTotalPrice.HasValue)
            {
                query = query.Where(o => o.TotalPrice >= request.MinTotalPrice.Value);
            }

            if (request.MaxTotalPrice.HasValue)
            {
                query = query.Where(o => o.TotalPrice <= request.MaxTotalPrice.Value);
            }

            // Sorting logic
            if (!string.IsNullOrEmpty(request.SortOption))
            {
                var sortOption = request.SortOption.Split('_');
                if (sortOption.Length == 2)
                {
                    var sortBy = sortOption[0];
                    var sortDirection = sortOption[1];
                    bool sortDescending = sortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase);

                    query = sortBy switch
                    {
                        "date" => sortDescending ? query.OrderByDescending(o => o.OrderDate) : query.OrderBy(o => o.OrderDate),
                        "price" => sortDescending ? query.OrderByDescending(o => o.TotalPrice) : query.OrderBy(o => o.TotalPrice),
                        _ => query
                    };
                }
            }

            return query;
        }
    }
}
