namespace ServerAspNetCoreAPIMakePC.Application.DTOs.Order
{
    public class OrderDto
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public string ShippingAddress { get; set; } = null!;
        public string PaymentStatus { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        public ICollection<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    }
}
