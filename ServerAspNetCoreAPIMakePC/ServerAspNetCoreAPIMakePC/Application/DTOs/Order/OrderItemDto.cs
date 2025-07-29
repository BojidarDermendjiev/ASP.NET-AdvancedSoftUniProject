﻿namespace ServerAspNetCoreAPIMakePC.Application.DTOs.Order
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
