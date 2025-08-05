namespace ServerAspNetCoreAPIMakePC.Infrastructure.Data.DbSeed
{
    using Domain.Entities;
    using Domain.ValueObjects;
    public class BrandSeed
    {
        public static List<Brand> GetBrands()
        {
            return new List<Brand>
            {
                new Brand { Name = new BrandName("Nvidia"), Description = "Leading GPU manufacturer.", LogoUrl = "https://example.com/nvidia-logo.png" },
                new Brand { Name = new BrandName("AMD"), Description = "Innovative CPUs and GPUs for gamers and professionals.", LogoUrl = "https://example.com/amd-logo.png" },
                new Brand { Name = new BrandName("Intel"), Description = "World leader in CPU manufacturing and computing solutions.", LogoUrl = "https://example.com/intel-logo.png" },
                new Brand { Name = new BrandName("ASUS"), Description = "Renowned manufacturer of motherboards, GPUs, and laptops.", LogoUrl = "https://example.com/asus-logo.png" },
                new Brand { Name = new BrandName("Corsair"), Description = "High-performance memory, cases, and power supplies.", LogoUrl = "https://example.com/corsair-logo.png" },
                new Brand { Name = new BrandName("MSI"), Description = "Innovative hardware and gaming solutions.", LogoUrl = "https://example.com/msi-logo.png" },
                new Brand { Name = new BrandName("Gigabyte"), Description = "Motherboards, graphics cards, and computer hardware.", LogoUrl = "https://example.com/gigabyte-logo.png" },
                new Brand { Name = new BrandName("Samsung"), Description = "Global leader in SSDs and memory solutions.", LogoUrl = "https://example.com/samsung-logo.png" },
                new Brand { Name = new BrandName("Kingston"), Description = "Trusted provider of memory and storage products.", LogoUrl = "https://example.com/kingston-logo.png" },
                new Brand { Name = new BrandName("EVGA"), Description = "Graphics cards, power supplies, and enthusiast PC hardware.", LogoUrl = "https://example.com/evga-logo.png" }
            };
        }
    }
}
