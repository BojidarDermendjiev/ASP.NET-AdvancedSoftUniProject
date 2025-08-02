namespace ServerAspNetCoreAPIMakePC.Application.DTOs.Brand
{
    using System.ComponentModel.DataAnnotations;

    using static Domain.ErrorMessages.ErrorMessages;
    using static Domain.Constants.BrandValidationConstants;
    public class UpdateBrandDto
    {
        /// <summary>
        /// The unique identifier of the brand to be updated.
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// The updated name of the brand.
        /// </summary>
        [Required]
        [StringLength(BrandNameMaxLength, MinimumLength = BrandNameMinLength, ErrorMessage = InvalidBrandName)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// The updated description of the brand.
        /// </summary>
        [Required]
        [StringLength(BrandDescriptionMaxLength, MinimumLength = BrandDescriptionMinLength, ErrorMessage = InvalidBrandDescription)]
        public string Description { get; set; } = null!;

        /// <summary>
        /// The updated URL of the brand's logo.
        /// </summary>
        [Required]
        [MaxLength(BrandLogoUrlMaxLength, ErrorMessage = InvalidBrandLogoUrl)]
        public string LogoUrl { get; set; } = null!;
    }
}
