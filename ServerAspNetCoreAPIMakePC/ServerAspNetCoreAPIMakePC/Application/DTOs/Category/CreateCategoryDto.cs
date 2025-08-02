namespace ServerAspNetCoreAPIMakePC.Application.DTOs.Category
{
    using System.ComponentModel.DataAnnotations;

    using static Domain.ErrorMessages.ErrorMessages;
    using static Domain.Constants.CategoryValidationConstants;
    public class CreateCategoryDto
    {
        /// <summary>
        /// The name of the category.
        /// </summary>
        [Required]
        [StringLength(CategoryNameMaxLength, MinimumLength = CategoryNameMinLength, ErrorMessage = InvalidCategoryName)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// The description of the category.
        /// </summary>
        [Required]
        [StringLength(CategoryDescriptionMaxLength, MinimumLength = CategoryDescriptionMinLength, ErrorMessage = InvalidCategoryDescriptionName)]
        public string Description { get; set; } = null!;
    }
}
