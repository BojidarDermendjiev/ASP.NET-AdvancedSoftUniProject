namespace ServerAspNetCoreAPIMakePC.Application.DTOs.Category
{
    using System.ComponentModel.DataAnnotations;

    using static Domain.ErrorMessages.ErrorMessages;
    using static Domain.Constants.CategoryValidationConstants;
    public class UpdateCategoryDto
    {
        /// <summary>
        /// The unique identifier of the category to be updated.
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// The updated name of the category.
        /// </summary>
        [Required]
        [StringLength(CategoryNameMaxLength, MinimumLength = CategoryNameMinLength, ErrorMessage = InvalidCategoryName)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// The updated description of the category.
        /// </summary>
        [Required]
        [StringLength(CategoryDescriptionMaxLength, MinimumLength = CategoryDescriptionMinLength, ErrorMessage = InvalidCategoryDescriptionName)]
        public string Description { get; set; } = null!;
    }
}
