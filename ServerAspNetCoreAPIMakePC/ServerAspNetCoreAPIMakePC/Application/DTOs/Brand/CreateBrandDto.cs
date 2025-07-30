namespace ServerAspNetCoreAPIMakePC.Application.DTOs.Brand
{
    using System.ComponentModel.DataAnnotations;

    using static Domain.ErrorMessages.ErrorMessages;
    using static Domain.Constants.BrandValidationConstants;
    public class CreateBrandDto
    {

        [Required]
        [StringLength(BrandNameMaxLength, MinimumLength = BrandNameMinLength, ErrorMessage = InvalidBrandName)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(BrandDescriptionMaxLength, MinimumLength = BrandDescriptionMinLength, ErrorMessage = InvalidBrandDescription)]
        public string Description { get; set; } = null!;

        [Required]
        [MaxLength(BrandLogoUrlMaxLength, ErrorMessage = InvalidBrandLogoUrl)]
        public string LogoUrl { get; set; } = null!;
    }
}
