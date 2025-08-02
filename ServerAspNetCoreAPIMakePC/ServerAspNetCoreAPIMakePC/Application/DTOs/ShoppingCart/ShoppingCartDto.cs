namespace ServerAspNetCoreAPIMakePC.Application.DTOs.ShoppingCart
{
    public class ShoppingCartDto
    {
        /// <summary>
        /// The unique identifier of the shopping cart.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The unique identifier of the user who owns the shopping cart.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The date and time when the shopping cart was created.
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// The collection of items contained in the shopping cart.
        /// </summary>
        public ICollection<BasketItemDto> Items { get; set; } = new List<BasketItemDto>();
    }
}
