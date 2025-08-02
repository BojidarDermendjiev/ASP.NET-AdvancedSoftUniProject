namespace ServerAspNetCoreAPIMakePC.Application.DTOs.ShoppingCart
{
    public class BasketItemDto
    {
        /// <summary>
        /// The unique identifier of the basket item.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The unique identifier of the associated product.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// The name of the product.
        /// </summary>
        public string ProductName { get; set; } = null!;

        /// <summary>
        /// The quantity of the product in the basket.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// The price of the product.
        /// </summary>
        public decimal Price { get; set; }
    }
}
