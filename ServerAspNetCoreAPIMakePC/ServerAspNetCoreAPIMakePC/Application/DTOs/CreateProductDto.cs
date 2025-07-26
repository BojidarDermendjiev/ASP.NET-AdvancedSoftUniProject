namespace ServerAspNetCoreAPIMakePC.Application.DTOs
{
    using System.ComponentModel.DataAnnotations;

    using static Domain.ErrorMessages.ErrorMessages;
    using static Domain.Constants.ProductValidationConstants;

    public class CreateProductDto
    {
        /// <summary>
        /// The name of the product.
        /// </summary>
        [Required]
        [StringLength(ProductNameMaxLength, MinimumLength = ProductNameMinLength, ErrorMessage = InvalidProductName)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// The description of the product.
        /// </summary>
        [Required]
        [StringLength(ProductDescriptionMaxLength, MinimumLength = ProductDescriptionMinLength, ErrorMessage = InvalidProductDescription)]
        public string Description { get; set; } = null!;

        /// <summary>
        /// The price of the product.
        /// </summary>
        [Required]
        [Range(ProductPriceMinValue, ProductPriceMaxValue, ErrorMessage = InvalidProductPrice)]
        public decimal Price { get; set; }

        /// <summary>
        /// The brand identifier for the product.
        /// </summary>
        [Required]
        public int BrandId { get; set; }

        /// <summary>
        /// The category identifier for the product.
        /// </summary>
        [Required]
        public int CategoryId { get; set; }

        /// <summary>
        /// The quantity of the product in stock (optional).
        /// </summary>
        public int? StockQuantity { get; set; }
    }
}
