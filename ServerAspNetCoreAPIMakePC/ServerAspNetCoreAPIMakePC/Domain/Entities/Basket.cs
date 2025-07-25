namespace ServerAspNetCoreAPIMakePC.Domain.Entities
{
    public class Basket
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public DateTime DateCreated { get; set; }
        public ICollection<BasketItem> Items { get; set; } = new HashSet<BasketItem>();
    }
}
