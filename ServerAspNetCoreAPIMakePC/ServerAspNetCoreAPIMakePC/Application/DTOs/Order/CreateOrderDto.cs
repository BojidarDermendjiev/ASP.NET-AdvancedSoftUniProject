namespace ServerAspNetCoreAPIMakePC.Application.DTOs.Order
{
    using System.ComponentModel.DataAnnotations;

    using static Domain.ErrorMessages.ErrorMessages;
    using static Domain.Constants.OrderValidationConstants;

    public class CreateOrderDto
    {
        /// <summary>
        /// The unique identifier of the user placing the order.
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// The shipping address for the order.
        /// </summary>
        [Required]
        [StringLength(OrderShippingAddressMaxLength, MinimumLength = OrderShippingAddressMinLength, ErrorMessage = InvalidShippingAddress)]
        public string ShippingAddress { get; set; } = null!;

        /// <summary>
        /// The payment status of the order.
        /// </summary>
        [Required]
        [StringLength(OrderPaymentStatusMaxLength, MinimumLength = OrderPaymentStatusMinLength, ErrorMessage = InvalidPaymentStatus)]
        public string PaymentStatus { get; set; } = null!;

        /// <summary>
        /// The total price of the order.
        /// </summary>

        [Required]
        [Range(OrderTotalPriceMinValue, OrderTotalPriceMaxValue, ErrorMessage = OrderTotalPriceErrorMessage)]
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// The collection of items included in the order.
        /// </summary>
        [Required]
        public ICollection<CreateOrderItemDto> Items { get; set; } = new List<CreateOrderItemDto>();
    }
}
