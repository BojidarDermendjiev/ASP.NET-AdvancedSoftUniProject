namespace ServerAspNetCoreAPIMakePC.Domain.Entities
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;

    using ValueObjects;
    using static Constants.BrandValidationConstants;
    using static ErrorMessages.ErrorMessages;

    public class Brand
    {
        [Required]
        [Comment("Unique identifier for the brand.")]
        public int Id { get; set; }

        [Required]
        [Comment("Name of the brand.")]
        [StringLength(BrandNameMaxLength, MinimumLength = BrandNameMinLength, ErrorMessage = InvalidBrandName)]
        public BrandName Name { get; set; } = null!;

        [Required]
        [Comment("Description of the brand.")]
        [StringLength(BrandDescriptionMaxLength, MinimumLength = BrandDescriptionMinLength, ErrorMessage = InvalidBrandDescription)]
        public string Description { get; set; } = null!;

        [Required]
        [Comment("URL to the brand's logo image.")]
        [MaxLength(BrandLogoUrlMaxLength, ErrorMessage = InvalidBrandLogoUrl)]
        public string LogoUrl { get; set; } = null!;

        [Comment("Collection of products associated with this brand.")]
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
