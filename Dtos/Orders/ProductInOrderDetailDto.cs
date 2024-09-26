namespace BlossomApi.Dtos.Orders
{
    public class ProductInOrderDetailDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public decimal ProductByAmountPrice { get; set; } // Unit Price * Amount
        public string ImageUrl { get; set; }
        public CategoryResponseDto MainCategory { get; set; }
    }
}
