namespace ServerAspNetCoreAPIMakePC.Infrastructure.Data.DbSeed
{
    using Domain.Entities;
    using Domain.ValueObjects;

    public class ReviewSeed
    {
        public static List<Review> GetReviews(List<User> users, List<Product> products)
        {
            User User(string email) => users.First(u => u.Email.Value == email);
            Product Product(string name) => products.First(p => p.Name.Value == name);

            return new List<Review>
            {
                new Review
                {
                    UserId = User("user@example.com").Id,
                    User = User("user@example.com"),
                    ProductId = Product("NVIDIA GeForce RTX 4090").Id,
                    Product = Product("NVIDIA GeForce RTX 4090"),
                    Rating = new Rating(5),
                    Comment = "Absolutely mind-blowing performance. Runs every game at max settings!",
                    Date = DateTime.UtcNow.AddDays(-12)
                },
                new Review
                {
                    UserId = User("admin@example.com").Id,
                    User = User("admin@example.com"),
                    ProductId = Product("Intel Core i9-14900K").Id,
                    Product = Product("Intel Core i9-14900K"),
                    Rating = new Rating(4),
                    Comment = "Great CPU for gaming and productivity, but runs a bit hot.",
                    Date = DateTime.UtcNow.AddDays(-8)
                },
                new Review
                {
                    UserId = User("user@example.com").Id,
                    User = User("user@example.com"),
                    ProductId = Product("Samsung 990 PRO 2TB NVMe SSD").Id,
                    Product = Product("Samsung 990 PRO 2TB NVMe SSD"),
                    Rating = new Rating(5),
                    Comment = "Extremely fast storage. Loads Windows in seconds!",
                    Date = DateTime.UtcNow.AddDays(-6)
                },
                new Review
                {
                    UserId = User("admin@example.com").Id,
                    User = User("admin@example.com"),
                    ProductId = Product("Corsair Vengeance RGB Pro 32GB DDR5").Id,
                    Product = Product("Corsair Vengeance RGB Pro 32GB DDR5"),
                    Rating = new Rating(5),
                    Comment = "Looks great in my build and has top-tier performance.",
                    Date = DateTime.UtcNow.AddDays(-2)
                },
                new Review
                {
                    UserId = User("user@example.com").Id,
                    User = User("user@example.com"),
                    ProductId = Product("ASUS ROG STRIX Z790-E GAMING WIFI").Id,
                    Product = Product("ASUS ROG STRIX Z790-E GAMING WIFI"),
                    Rating = new Rating(4),
                    Comment = "Excellent motherboard, but a bit pricey.",
                    Date = DateTime.UtcNow.AddDays(-1)
                }
            };
        }
    }
}
