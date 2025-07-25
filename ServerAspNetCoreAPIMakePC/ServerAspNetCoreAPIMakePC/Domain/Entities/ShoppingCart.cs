namespace ServerAspNetCoreAPIMakePC.Domain.Entities
{
    public class ShoppingCart
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public DateTime DateCreated { get; set; }
        public virtual ICollection<BasketItem> Items { get; set; } = new List<BasketItem>();
    }
}
