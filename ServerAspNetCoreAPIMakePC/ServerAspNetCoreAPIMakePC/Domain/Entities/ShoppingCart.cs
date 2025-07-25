namespace ServerAspNetCoreAPIMakePC.Domain.Entities
{
    public class ShoppingCart
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public ICollection<BasketItem> Items { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
