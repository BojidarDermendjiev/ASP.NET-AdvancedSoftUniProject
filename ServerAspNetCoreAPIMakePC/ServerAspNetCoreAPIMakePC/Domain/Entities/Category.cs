namespace ServerAspNetCoreAPIMakePC.Domain.Entities
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;

    using static ErrorMessages.ErrorMessages;
    using static Constants.CategoryValidationConstants;
    public class Category
    {
      
        [Required]
        [Comment("Primary key for the Category entity.")]
        public int Id { get; set; }

        [Required]
        [Comment("Name of the category.")]
        [StringLength(CategoryNameMaxLength, MinimumLength = CategoryNameMinLength, ErrorMessage = InvalidCategoryName)]
        public string Name { get; set; } = null!;

        [Required]
        [Comment("Description of the category.")]
        [StringLength(CategoryDescriptionMaxLength,MinimumLength = CategoryDescriptionMinLength, ErrorMessage = InvalidCategoryDescriptionName)]
        public string Description { get; set; } = null!;

        [Comment("Products belonging to the category.")]
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
