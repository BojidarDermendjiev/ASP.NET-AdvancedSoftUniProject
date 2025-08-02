namespace ServerAspNetCoreAPIMakePC.Application.DTOs.Order
{
    using System.ComponentModel.DataAnnotations;

    using static Domain.ErrorMessages.ErrorMessages;
    using static Domain.Constants.OrderValidationConstants;
    public class CreateOrderItemDto
    {
        /// <summary>
        /// The unique identifier of the product in the order item.
        /// </summary>
        [Required]
        public Guid ProductId { get; set; }

        /// <summary>
        /// The quantity of the product in the order item.
        /// </summary>
        [Required]
        [Range(OrderQuantityMinValue, OrderQuantityMaxValue, ErrorMessage = InvalidQuantity)]
        public int Quantity { get; set; }

        /// <summary>
        /// The unit price of the product in the order item.
        /// </summary>
        [Required]
        [Range(OrderUnitPriceMinValue, OrderUnitPriceMaxValue, ErrorMessage = InvalidUnitPrice)]
        public decimal UnitPrice { get; set; }
    }
}
