namespace ServerAspNetCoreAPIMakePC.Application.DTOs.Basket
{
    using System.ComponentModel.DataAnnotations;

    public class CreateBasketDto
    {
        /// <summary>
        /// The unique identifier of the user creating the basket.
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// The collection of items to add to the basket.
        /// </summary>
        [Required]
        public ICollection<CreateBasketItemDto> Items { get; set; } = new List<CreateBasketItemDto>();

    }
}
