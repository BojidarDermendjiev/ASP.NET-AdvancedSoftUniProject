namespace ServerAspNetCoreAPIMakePC.Application.DTOs.ShoppingCart
{
    using System.ComponentModel.DataAnnotations;

    public class RemoveBasketItemDto
    {

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid ProductId { get; set; }
    }
}
