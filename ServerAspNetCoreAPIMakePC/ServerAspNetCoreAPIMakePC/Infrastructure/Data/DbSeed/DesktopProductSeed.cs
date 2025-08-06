namespace ServerAspNetCoreAPIMakePC.Infrastructure.Data.DbSeed
{
    using Domain.Entities;
    using Domain.ValueObjects;
    public class DesktopProductSeed
    {
        public static List<Product> GetDesktopProducts(List<Brand> brands, List<Category> categories)
        {
            Brand Brand(string name) => brands.FirstOrDefault(b => b.Name.Value == name)
                                        ?? throw new InvalidOperationException($"Brand '{name}' not found in seed list.");
            Category Category(string name) => categories.FirstOrDefault(c => c.Name.Value == name)
                                              ?? throw new InvalidOperationException($"Category '{name}' not found in seed list.");

            return new List<Product>
            {
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = new ProductName("Intel Core i9-14900K"),
                    Type = "CPU",
                    Brand = Brand("Intel"),
                    Price = 649.99m,
                    Stock = 20,
                    Description = "High-performance desktop processor with 24 cores for gaming and productivity.",
                    Specs = new ProductSpecs("24 Cores, 32 Threads, Base 3.2GHz, Turbo 5.8GHz, LGA1700"),
                    ImageUrl = "https://example.com/intel-i9-14900k.png",
                    Category = Category("Processors"),
                    CategoryId = Category("Processors").Id
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = new ProductName("AMD Ryzen 9 7950X"),
                    Type = "CPU",
                    Brand = Brand("AMD"),
                    Price = 599.99m,
                    Stock = 15,
                    Description = "Flagship 16-core processor for high-end desktops.",
                    Specs = new ProductSpecs("16 Cores, 32 Threads, Base 4.5GHz, Turbo 5.7GHz, AM5"),
                    ImageUrl = "https://example.com/amd-ryzen-9-7950x.png",
                    Category = Category("Processors"),
                    CategoryId = Category("Processors").Id
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = new ProductName("NVIDIA GeForce RTX 4090"),
                    Type = "GPU",
                    Brand = Brand("Nvidia"),
                    Price = 1999.99m,
                    Stock = 8,
                    Description = "Ultimate gaming GPU with AI-powered graphics.",
                    Specs = new ProductSpecs("24GB GDDR6X, 16384 CUDA Cores, PCIe 4.0"),
                    ImageUrl = "https://example.com/rtx4090.png",
                    Category = Category("Graphics Cards"),
                    CategoryId = Category("Graphics Cards").Id
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = new ProductName("AMD Radeon RX 7900 XTX"),
                    Type = "GPU",
                    Brand = Brand("AMD"),
                    Price = 1099.99m,
                    Stock = 10,
                    Description = "High-end AMD graphics card for 4K gaming.",
                    Specs = new ProductSpecs("24GB GDDR6, 6144 Stream Processors, PCIe 4.0"),
                    ImageUrl = "https://example.com/rx7900xtx.png",
                    Category = Category("Graphics Cards"),
                    CategoryId = Category("Graphics Cards").Id
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = new ProductName("Corsair Vengeance RGB Pro 32GB DDR5"),
                    Type = "RAM",
                    Brand = Brand("Corsair"),
                    Price = 189.99m,
                    Stock = 25,
                    Description = "High-speed DDR5 memory with customizable RGB lighting.",
                    Specs = new ProductSpecs("32GB (2x16GB), DDR5-6000, CL36, 1.35V"),
                    ImageUrl = "https://example.com/corsair-vengeance-ddr5.png",
                    Category = Category("Memory"),
                    CategoryId = Category("Memory").Id
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = new ProductName("Samsung 990 PRO 2TB NVMe SSD"),
                    Type = "SSD",
                    Brand = Brand("Samsung"),
                    Price = 169.99m,
                    Stock = 30,
                    Description = "Ultra-fast PCIe Gen4 NVMe SSD for desktops.",
                    Specs = new ProductSpecs("2TB, PCIe 4.0 x4, up to 7,450 MB/s read, 6,900 MB/s write"),
                    ImageUrl = "https://example.com/samsung-990pro-2tb.png",
                    Category = Category("Storage"),
                    CategoryId = Category("Storage").Id
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = new ProductName("ASUS ROG STRIX Z790-E GAMING WIFI"),
                    Type = "Motherboard",
                    Brand = Brand("ASUS"),
                    Price = 499.99m,
                    Stock = 12,
                    Description = "Premium Z790 motherboard for Intel 13th/14th Gen CPUs.",
                    Specs = new ProductSpecs("ATX, LGA1700, DDR5, PCIe 5.0, WiFi 6E"),
                    ImageUrl = "https://example.com/asus-z790-e.png",
                    Category = Category("Motherboards"),
                    CategoryId = Category("Motherboards").Id
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = new ProductName("MSI MPG A850G PCIE5 Power Supply"),
                    Type = "PSU",
                    Brand = Brand("MSI"),
                    Price = 149.99m,
                    Stock = 20,
                    Description = "850W fully modular PSU with PCIe 5.0 support.",
                    Specs = new ProductSpecs("850W, 80+ Gold, Fully Modular, ATX 3.0"),
                    ImageUrl = "https://example.com/msi-a850g.png",
                    Category = Category("Power Supplies"),
                    CategoryId = Category("Power Supplies").Id
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = new ProductName("NZXT H7 Flow Mid Tower Case"),
                    Type = "Case",
                    Brand = Brand("NZXT"),
                    Price = 129.99m,
                    Stock = 18,
                    Description = "Airflow-optimized mid-tower case with sleek design.",
                    Specs = new ProductSpecs("ATX, Tempered Glass, White, 3x F Series Case Fans"),
                    ImageUrl = "https://example.com/nzxt-h7-flow.png",
                    Category = Category("Cases"),
                    CategoryId = Category("Cases").Id
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = new ProductName("Dell UltraSharp U2723QE 27\" 4K Monitor"),
                    Type = "Monitor",
                    Brand = Brand("Dell"),
                    Price = 529.99m,
                    Stock = 14,
                    Description = "27-inch 4K UHD monitor with USB-C and excellent color accuracy.",
                    Specs = new ProductSpecs("27\", 3840x2160, IPS Black, 60Hz, USB-C Hub"),
                    ImageUrl = "https://example.com/dell-u2723qe.png",
                    Category = Category("Monitors"),
                    CategoryId = Category("Monitors").Id
                }
            };
        }
    }
}
