namespace ServerAspNetCoreAPIMakePC.Application.DTOs.Basket
{
    public class BasketItemDto
    {
        public int Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
