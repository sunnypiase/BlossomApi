﻿namespace BlossomApi.Dtos.Orders
{
    public class ProductInOrderDto
    {
        public string ProductName { get; set; }
        public int ProductId { get; set; }
        public string Article { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
    }

}