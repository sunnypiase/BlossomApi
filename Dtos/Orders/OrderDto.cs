using BlossomApi.Models;

namespace BlossomApi.Dtos.Orders
{
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

}