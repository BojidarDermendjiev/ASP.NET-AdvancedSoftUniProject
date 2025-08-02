namespace ServerAspNetCoreAPIMakePC.Application.DTOs.ShoppingCart
{
    using System.ComponentModel.DataAnnotations;

    public class RemoveBasketItemDto
    {

        /// <summary>
        /// The unique identifier of the user removing the item.
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// The unique identifier of the product to be removed from the basket.
        /// </summary>
        [Required]
        public Guid ProductId { get; set; }
    }
}
