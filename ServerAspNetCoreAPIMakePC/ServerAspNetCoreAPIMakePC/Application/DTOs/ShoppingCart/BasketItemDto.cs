namespace ServerAspNetCoreAPIMakePC.Application.DTOs.ShoppingCart
{
    public class BasketItemDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
