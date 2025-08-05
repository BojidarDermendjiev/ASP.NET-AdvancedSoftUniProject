namespace ServerAspNetCoreAPIMakePC.Infrastructure.Data.DbSeed
{
    public class DbSeed
    {
        public static void Seed(MakePCDbContext context)
        {
            if (!context.Brands.Any())
            {
                var brands = BrandSeed.GetBrands();
                context.Brands.AddRange(brands);
            }

            if (!context.Categories.Any())
            {
                var categories = CategorySeed.GetCategories();
                context.Categories.AddRange(categories);
            }

            if (!context.Users.Any())
            {
                var users = UserSeed.GetUsers();
                context.Users.AddRange(users);
            }

            if (!context.Products.Any())
            {
                var brands = context.Brands.ToList();
                var categories = context.Categories.ToList();
                var products = DesktopProductSeed.GetDesktopProducts(brands, categories);
                context.Products.AddRange(products);
            }
           
            if (!context.PlatformFeedbacks.Any())
            {
                var users = context.Users.ToList();
                var feedbacks = PlatformFeedbackSeed.GetPlatformFeedbacks(users);
                context.PlatformFeedbacks.AddRange(feedbacks);
            }

            if (!context.Reviews.Any())
            {
                var products = context.Products.ToList();
                var users = context.Users.ToList();
                var reviews = ReviewSeed.GetReviews(users, products);
                context.Reviews.AddRange(reviews);
            }
            context.SaveChanges();
        }
    }
}
