
using ServerAspNetCoreAPIMakePC.Domain.Entities;

namespace ServerAspNetCoreAPIMakePC.Infrastructure.Data
{
    using Microsoft.EntityFrameworkCore;
    public class MakePCDbContext : DbContext
    {
        public MakePCDbContext(DbContextOptions<MakePCDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<PlatformFeedback> PlatformFeedbacks { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // You can configure relationships or property constraints here if needed
        }
    }
}
