namespace ServerAspNetCoreAPIMakePC.Application.DTOs
{
    using System.ComponentModel.DataAnnotations;

    using static Domain.ErrorMessages.ErrorMessages;
    using static Domain.Constants.ProductValidationConstants;

    public class UpdateProductDto
    {
        /// <summary>
        /// The name of the product.
        /// </summary>
        [StringLength(ProductNameMaxLength, MinimumLength = ProductNameMinLength, ErrorMessage = InvalidProductName)]
        public string? Name { get; set; }

        /// <summary>
        /// The type or category of the product.
        /// </summary>
        [StringLength(ProductTypeMaxLength, MinimumLength = ProductTypeMinLength, ErrorMessage = InvalidProductType)]
        public string? Type { get; set; }

        /// <summary>
        /// The brand associated with the product.
        /// </summary>
        [StringLength(ProductBrandNameMaxLength, MinimumLength = ProductBrandNameMinLength, ErrorMessage = InvalidProductBrand)]
        public string? Brand { get; set; }

        /// <summary>
        /// The price of the product.
        /// </summary>
        [Range(ProductPriceMinValue, ProductPriceMaxValue, ErrorMessage = InvalidProductPrice)]
        public decimal? Price { get; set; }

        /// <summary>
        /// The available stock quantity of the product.
        /// </summary>
        [StringLength(ProductStockMaxValue, MinimumLength = ProductStockMinValue, ErrorMessage = InvalidProductStock)]
        public int? Stock { get; set; }

        /// <summary>
        /// A detailed description of the product.
        /// </summary>
        [StringLength(ProductDescriptionMaxLength, MinimumLength = ProductDescriptionMinLength, ErrorMessage = InvalidProductDescription)]
        public string? Description { get; set; }


        /// <summary>
        /// Technical specifications of the product.
        /// </summary>
        [StringLength(ProductSpecsMaxLength,MinimumLength = ProductSpecsMinLength, ErrorMessage = InvalidProductSpecs)]
        public string? Specs { get; set; }

        /// <summary>
        /// The URL of the product's image.
        /// </summary>
        [Url(ErrorMessage = InvalidProductImageURL)]
        public string? ImageUrl { get; set; }

        /// <summary>
        /// The unique identifier of the category to which the product belongs.
        /// </summary>
        public int? CategoryId { get; set; }
    }
}
