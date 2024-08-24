namespace BlossomApi.Dtos.Orders
{
    public class GetOrdersByFilterResponse
    {
        public List<OrderDto> Orders { get; set; }
        public int TotalCount { get; set; }
    }
}
