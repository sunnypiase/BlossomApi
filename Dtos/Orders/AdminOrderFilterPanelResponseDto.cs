namespace BlossomApi.Dtos.Orders
{
    public class AdminOrderFilterPanelResponseDto
    {
        public List<OrderStatusOptionDto> StatusOptions { get; set; }
        public DateTime MinOrderDate { get; set; }
        public DateTime MaxOrderDate { get; set; }
        public decimal MinTotalPrice { get; set; }
        public decimal MaxTotalPrice { get; set; }
    }
}
