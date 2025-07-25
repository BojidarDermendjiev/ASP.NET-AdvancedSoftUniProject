namespace ServerAspNetCoreAPIMakePC.Domain.Entities
{
    public class BasketItem
    {
        public int Id { get; set; }
        public int BasketId { get; set; }
        public Basket Basket { get; set; } = null!;
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
