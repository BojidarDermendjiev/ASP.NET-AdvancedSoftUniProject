namespace ServerAspNetCoreAPIMakePC.Application.DTOs.Brand
{
    public class BrandDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string LogoUrl { get; set; } = null!;
    }
}
