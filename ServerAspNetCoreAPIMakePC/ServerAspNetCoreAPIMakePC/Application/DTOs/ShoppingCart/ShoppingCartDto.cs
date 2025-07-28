namespace ServerAspNetCoreAPIMakePC.Application.DTOs.ShoppingCart
{
    public class ShoppingCartDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime DateCreated { get; set; }
        public ICollection<BasketItemDto> Items { get; set; }
    }
}
