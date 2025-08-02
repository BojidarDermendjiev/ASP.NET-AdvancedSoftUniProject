namespace ServerAspNetCoreAPIMakePC.Application.DTOs.Basket
{
    using System.ComponentModel.DataAnnotations;

    using static Domain.ErrorMessages.ErrorMessages;
    using static Domain.Constants.BasketValidationConstants;
    public class CreateBasketItemDto
    {
        /// <summary>
        /// The unique identifier of the product to add to the basket.
        /// </summary>
        [Required]
        public Guid ProductId { get; set; }

        /// <summary>
        /// The quantity of the product to add to the basket.
        /// </summary>
        [Required]
        [Range(BasketItemQuantityMin, BasketItemQuantityMax, ErrorMessage = InvalidBasketQuantity)]
        public int Quantity { get; set; }
    }
}
