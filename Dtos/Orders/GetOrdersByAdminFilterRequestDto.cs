using BlossomApi.Models;
using System.ComponentModel;

namespace BlossomApi.Dtos.Orders
{
    public class GetOrdersByAdminFilterRequestDto
    {
        public string? SearchTerm { get; set; } // Search by Username or Surname
        public List<OrderStatus>? Statuses { get; set; }
        public DateTime? MinOrderDate { get; set; }
        public DateTime? MaxOrderDate { get; set; }
        public decimal? MinTotalPrice { get; set; }
        public decimal? MaxTotalPrice { get; set; }
        public string? SortOption { get; set; } // Sorting options like date_asc, date_desc, price_asc, price_desc
        public int Start { get; set; } // Pagination start
        [DefaultValue(10)]
        public int Amount { get; set; } // Pagination amount
    }

}
