namespace ServerAspNetCoreAPIMakePC.Domain.Entities
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using ValueObjects;
    using static Constants.OrderValidationConstants;
    using static ErrorMessages.ErrorMessages;

    public class Order
    {
        [Required]
        [Comment("Unique identifier for the order.")]
        public int Id { get; set; }

        [Required]
        [Comment("Identifier for the user who placed the order.")]
        public Guid UserId { get; set; }

        [Required]
        [Comment("Navigation property to the user associated with the order.")]
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        [Required]
        [Comment("The date the order was placed.")]
        public DateTime OrderDate { get; set; }

        [Required]
        [Comment("The shipping address for the order.")]
        [StringLength(OrderShippingAddressMaxLength, MinimumLength = OrderShippingAddressMinLength, ErrorMessage = InvalidShippingAddress)]
        public ShippingAddress ShippingAddress { get; set; } = null!;

        [Required]
        [Comment("The payment status of the order.")]
        [StringLength(OrderPaymentStatusMaxLength, MinimumLength = OrderPaymentStatusMinLength, ErrorMessage = InvalidPaymentStatus)]
        public string PaymentStatus { get; set; } = null!;

        [Required]
        [Comment("The total price of the order.")]
        public decimal TotalPrice { get; set; }

        [Comment("Collection of items included in the order.")]
        public virtual ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
