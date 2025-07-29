namespace ServerAspNetCoreAPIMakePC.Application.DTOs.Category
{
    using System.ComponentModel.DataAnnotations;

    using static Domain.ErrorMessages.ErrorMessages;
    using static Domain.Constants.CategoryValidationConstants;
    public class UpdateCategoryDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(CategoryNameMaxLength, MinimumLength = CategoryNameMinLength, ErrorMessage = InvalidCategoryName)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(CategoryDescriptionMaxLength, MinimumLength = CategoryDescriptionMinLength, ErrorMessage = InvalidCategoryDescriptionName)]
        public string Description { get; set; } = null!;
    }
}
