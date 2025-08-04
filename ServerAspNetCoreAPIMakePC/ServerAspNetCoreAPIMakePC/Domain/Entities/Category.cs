namespace ServerAspNetCoreAPIMakePC.Domain.Entities
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;

    using ValueObjects;
    using static Constants.CategoryValidationConstants;
    using static ErrorMessages.ErrorMessages;

    public class Category
    {
      
        [Required]
        [Comment("Primary key for the Category entity.")]
        public int Id { get; set; }

        [Required]
        [Comment("Name of the category.")]
        [StringLength(CategoryNameMaxLength, MinimumLength = CategoryNameMinLength, ErrorMessage = InvalidCategoryName)]
        public CategoryName Name { get; set; } = null!;

        [Required]
        [Comment("Description of the category.")]
        [StringLength(CategoryDescriptionMaxLength,MinimumLength = CategoryDescriptionMinLength, ErrorMessage = InvalidCategoryDescriptionName)]
        public string Description { get; set; } = null!;

        [Comment("Products belonging to the category.")]
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
