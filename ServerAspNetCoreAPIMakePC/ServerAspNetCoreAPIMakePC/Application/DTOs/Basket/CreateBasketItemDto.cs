namespace ServerAspNetCoreAPIMakePC.Application.DTOs.Basket
{
    using System.ComponentModel.DataAnnotations;

    using static Domain.ErrorMessages.ErrorMessages;
    using static Domain.Constants.BasketValidationConstants;
    public class CreateBasketItemDto
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        [Range(BasketItemQuantityMin, BasketItemQuantityMax, ErrorMessage = InvalidBasketQuantity)]
        public int Quantity { get; set; }
    }
}
