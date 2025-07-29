namespace ServerAspNetCoreAPIMakePC.Application.DTOs.Order
{
    using System.ComponentModel.DataAnnotations;

    using static Domain.ErrorMessages.ErrorMessages;
    using static Domain.Constants.OrderValidationConstants;
    public class CreateOrderItemDto
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        [Range(OrderQuantityMinValue, OrderQuantityMaxValue, ErrorMessage = InvalidQuantity)]
        public int Quantity { get; set; }

        [Required]
        [Range(OrderUnitPriceMinValue, OrderUnitPriceMaxValue, ErrorMessage = InvalidUnitPrice)]
        public decimal UnitPrice { get; set; }
    }
}
