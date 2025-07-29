namespace ServerAspNetCoreAPIMakePC.Application.DTOs.Order
{
    using System.ComponentModel.DataAnnotations;

    using static Domain.ErrorMessages.ErrorMessages;
    using static Domain.Constants.OrderValidationConstants;
    public class UpdateOrderDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        [StringLength(OrderShippingAddressMaxLength, MinimumLength = OrderShippingAddressMinLength, ErrorMessage = InvalidShippingAddress)]
        public string ShippingAddress { get; set; } = null!;

        [Required]
        [StringLength(OrderPaymentStatusMaxLength, MinimumLength = OrderPaymentStatusMinLength, ErrorMessage = InvalidPaymentStatus)]
        public string PaymentStatus { get; set; } = null!;

        [Required]
        [Range(OrderTotalPriceMinValue, OrderTotalPriceMaxValue, ErrorMessage = OrderTotalPriceErrorMessage)]
        public decimal TotalPrice { get; set; }

        [Required]
        public ICollection<CreateOrderItemDto> Items { get; set; } = new List<CreateOrderItemDto>();
    }
}
