namespace ServerAspNetCoreAPIMakePC.Infrastructure.Data.DbSeed
{
    using Domain.Entities;
    using Domain.ValueObjects;
    public class CategorySeed
    {
        public static List<Category> GetCategories()
        {
            return new List<Category>
            {
                new Category { Name = new CategoryName("Graphics Cards"), Description = "GPUs and related products" },
                new Category { Name = new CategoryName("Processors"), Description = "CPUs and related products" },
                new Category { Name = new CategoryName("Motherboards"), Description = "Motherboards for all platforms" }
            };
        }
    }
}
