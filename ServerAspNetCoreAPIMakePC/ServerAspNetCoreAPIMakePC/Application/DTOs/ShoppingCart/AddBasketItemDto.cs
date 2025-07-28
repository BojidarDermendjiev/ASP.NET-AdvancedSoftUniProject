namespace ServerAspNetCoreAPIMakePC.Application.DTOs.ShoppingCart
{
    using System.ComponentModel.DataAnnotations;

    public class AddBasketItemDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
