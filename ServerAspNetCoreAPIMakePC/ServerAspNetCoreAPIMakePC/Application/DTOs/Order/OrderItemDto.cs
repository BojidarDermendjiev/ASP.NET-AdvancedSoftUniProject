namespace ServerAspNetCoreAPIMakePC.Application.DTOs.Order
{
    public class OrderItemDto
    {
        /// <summary>
        /// The unique identifier of the order item.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The unique identifier of the product in the order item.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// The name of the product in the order item.
        /// </summary>
        public string ProductName { get; set; } = null!;

        /// <summary>
        /// The quantity of the product in the order item.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// The unit price of the product in the order item.
        /// </summary>
        public decimal UnitPrice { get; set; }
    }
}
