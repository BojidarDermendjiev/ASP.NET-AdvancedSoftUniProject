namespace ServerAspNetCoreAPIMakePC.Application.DTOs.Basket
{
    using System.ComponentModel.DataAnnotations;

    public class UpdateBasketDto
    {
        /// <summary>
        /// The unique identifier of the basket to update.
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// The unique identifier of the user who owns the basket.
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// The collection of items to update in the basket.
        /// </summary>
        [Required]
        public ICollection<CreateBasketItemDto> Items { get; set; } = new List<CreateBasketItemDto>();
    }
}
