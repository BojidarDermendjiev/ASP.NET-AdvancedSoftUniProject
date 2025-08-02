namespace ServerAspNetCoreAPIMakePC.Application.DTOs.ShoppingCart
{
    using System.ComponentModel.DataAnnotations;

    public class AddBasketItemDto
    {

        /// <summary>
        /// The unique identifier of the user adding the item.
        /// </summary>
        [Required]
        public Guid UserId { get; set; }


        /// <summary>
        /// The unique identifier of the product to be added to the basket.
        /// </summary>
        [Required]
        public Guid ProductId { get; set; }

        /// <summary>
        /// The quantity of the product to add. Must be at least 1.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
