namespace BlossomApi.Dtos.Orders
{
    public class OrderStatusOptionDto
    {
        public int Status { get; set; }
        public string StatusName { get; set; } // This will hold the Ukrainian name
        public int OrdersCount { get; set; }
    }
}
