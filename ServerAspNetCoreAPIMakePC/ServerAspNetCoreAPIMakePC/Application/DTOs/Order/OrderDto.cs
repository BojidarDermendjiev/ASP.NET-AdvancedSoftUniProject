namespace ServerAspNetCoreAPIMakePC.Application.DTOs.Order
{
    public class OrderDto
    {
        /// <summary>
        /// The unique identifier of the order.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The unique identifier of the user who placed the order.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The name of the user who placed the order.
        /// </summary>
        public string UserName { get; set; } = null!;

        /// <summary>
        /// The date and time when the order was placed.
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// The shipping address for the order.
        /// </summary>
        public string ShippingAddress { get; set; } = null!;


        /// <summary>
        /// The payment status of the order.
        /// </summary>
        public string PaymentStatus { get; set; } = null!;

        /// <summary>
        /// The total price of the order.
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// The collection of items included in the order.
        /// </summary>
        public ICollection<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    }
}
