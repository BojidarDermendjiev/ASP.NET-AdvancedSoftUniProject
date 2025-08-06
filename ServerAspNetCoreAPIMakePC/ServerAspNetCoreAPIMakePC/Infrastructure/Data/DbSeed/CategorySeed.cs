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
                new Category { Name = new CategoryName("Motherboards"), Description = "Motherboards for all platforms" },
                new Category { Name = new CategoryName("Memory"), Description = "RAM and memory modules" },
                new Category { Name = new CategoryName("Storage"), Description = "SSDs, HDDs, and storage devices" },
                new Category { Name = new CategoryName("Power Supplies"), Description = "Power supply units" },
                new Category { Name = new CategoryName("Cases"), Description = "PC cases" },
                new Category { Name = new CategoryName("Monitors"), Description = "Computer monitors" }
            };
        }
    }
}