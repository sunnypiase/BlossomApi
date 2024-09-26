namespace BlossomApi.Dtos.Orders
{
    public class UserOrderSummaryDto
    {
        public int OrderId { get; set; }
        public decimal TotalPriceWithDiscount { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
