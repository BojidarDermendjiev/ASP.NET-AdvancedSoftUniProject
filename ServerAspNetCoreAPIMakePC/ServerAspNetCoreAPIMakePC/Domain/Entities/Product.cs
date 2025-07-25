namespace ServerAspNetCoreAPIMakePC.Domain.Entities
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static ErrorMessages.ErrorMessages;
    using static Constants.ProductValidationConstants;

    public class Product
    {
        [Required]
        [Comment("Unique identifier for the product.")]
        public Guid Id { get; set; }

        [Required]
        [Comment("Name of the product.")]
        [StringLength(ProductNameMaxLength, MinimumLength = ProductNameMinLength, ErrorMessage = InvalidProductName)]
        public string Name { get; set; } = null!;

        [Required]
        [Comment("Type of the product (e.g., CPU, GPU, etc.).")]
        [StringLength(ProductTypeMaxLength, MinimumLength = ProductTypeMinLength, ErrorMessage = InvalidProductType)]
        public string Type { get; set; } = null!;

        [Required]
        [Comment("Brand of the product.")]
        [StringLength(ProductBrandMaxLength, MinimumLength = ProductBrandMinLength, ErrorMessage = InvalidProductBrand)]
        public string Brand { get; set; } = null!;

        [Required]
        [Comment("Price of the product.")]
        public decimal Price { get; set; }

        [Required]
        [Comment("Stock quantity of the product.")]
        [Range(ProductStockMinValue, ProductStockMaxValue, ErrorMessage = InvalidProductStock)]
        public int Stock { get; set; }

        [Required]
        [Comment("Description of the product.")]
        [StringLength(ProductDescriptionMaxLength, MinimumLength = ProductDescriptionMinLength, ErrorMessage = InvalidProductDescription)]
        public string Description { get; set; } = null!;

        [Required]
        [Comment("Specifications of the product.")]
        [StringLength(ProductSpecsMaxLength, MinimumLength = ProductSpecsMinLength, ErrorMessage = InvalidProductSpecs)]
        public string Specs { get; set; } = null!;

        [Required]
        [Comment("Image URL of the product.")]
        public string ImageUrl { get; set; } = null!;

        [Required]
        [Comment("Foreign key referencing the category of the product.")]
        public int CategoryId { get; set; }

        [Required]
        [Comment("Navigation property to the category of the product.")]
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;

        [Comment("Collection of reviews associated with the product.")]
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
