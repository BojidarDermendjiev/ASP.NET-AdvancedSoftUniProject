namespace ServerAspNetCoreAPIMakePC.Domain.Entities
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string LogoUrl { get; set; } = null!;
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
