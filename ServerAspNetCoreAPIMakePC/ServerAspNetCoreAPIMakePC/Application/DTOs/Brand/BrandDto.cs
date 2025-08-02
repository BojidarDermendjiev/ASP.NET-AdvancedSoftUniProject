namespace ServerAspNetCoreAPIMakePC.Application.DTOs.Brand
{
    public class BrandDto
    {
        /// <summary>
        /// The unique identifier of the brand.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the brand.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// The description of the brand.
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// The URL of the brand's logo.
        /// </summary>
        public string LogoUrl { get; set; } = null!;
    }
}
