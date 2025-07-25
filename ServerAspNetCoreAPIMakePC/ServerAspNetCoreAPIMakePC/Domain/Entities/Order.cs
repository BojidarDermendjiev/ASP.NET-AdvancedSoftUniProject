namespace ServerAspNetCoreAPIMakePC.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public string ShippingAddress { get; set; } = null!;
        public string PaymentStatus { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        public virtual ICollection<OrderItem> Items { get; set; } = null!;
    }
}
