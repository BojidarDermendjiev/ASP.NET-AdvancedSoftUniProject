namespace ServerAspNetCoreAPIMakePC.Application.DTOs.Order
{
    using System.ComponentModel.DataAnnotations;

    using static Domain.ErrorMessages.ErrorMessages;
    using static Domain.Constants.OrderValidationConstants;
    public class UpdateOrderDto
    {

        /// <summary>
        /// The unique identifier of the order to be updated.
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// The unique identifier of the user who placed the order.
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// The updated shipping address for the order.
        /// </summary>
        [Required]
        [StringLength(OrderShippingAddressMaxLength, MinimumLength = OrderShippingAddressMinLength, ErrorMessage = InvalidShippingAddress)]
        public string ShippingAddress { get; set; } = null!;

        /// <summary>
        /// The updated payment status of the order.
        /// </summary>
        [Required]
        [StringLength(OrderPaymentStatusMaxLength, MinimumLength = OrderPaymentStatusMinLength, ErrorMessage = InvalidPaymentStatus)]
        public string PaymentStatus { get; set; } = null!;

        /// <summary>
        /// The updated total price of the order.
        /// </summary>
        [Required]
        [Range(OrderTotalPriceMinValue, OrderTotalPriceMaxValue, ErrorMessage = OrderTotalPriceErrorMessage)]
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// The updated collection of items in the order.
        /// </summary>
        [Required]
        public ICollection<CreateOrderItemDto> Items { get; set; } = new List<CreateOrderItemDto>();
    }
}
