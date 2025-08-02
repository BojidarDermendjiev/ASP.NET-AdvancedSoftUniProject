namespace ServerAspNetCoreAPIMakePC.Application.DTOs.Basket
{
    public class BasketItemDto
    {
        /// <summary>
        /// The unique identifier of the basket item.
        /// </summary
        public int Id { get; set; }

        /// <summary>
        /// The unique identifier of the product in the basket item.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// The name of the product in the basket item.
        /// </summary>
        public string ProductName { get; set; } = null!;

        /// <summary>
        /// The quantity of the product in the basket item.
        /// </summary>
        public int Quantity { get; set; }
    }
}
