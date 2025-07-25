namespace ServerAspNetCoreAPIMakePC.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Role { get; set; } = null!;
        public ShoppingCart ShoppingCart { get; set; } = null!;
        public  ICollection<Order> Orders { get; set; } = new List<Order>();
        public  ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
