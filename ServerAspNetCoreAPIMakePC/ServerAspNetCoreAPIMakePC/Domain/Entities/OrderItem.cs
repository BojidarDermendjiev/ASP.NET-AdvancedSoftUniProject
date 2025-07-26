namespace ServerAspNetCoreAPIMakePC.Domain.Entities
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class OrderItem
    {
        [Required]
        [Comment("Unique identifier for the order item.")]
        public int Id { get; set; }

        [Required]
        [Comment("Foreign key referencing the associated order.")]
        public int OrderId { get; set; }

        [Required]
        [Comment("Navigation property referencing the associated order entity.")]
        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; } = null!;

        [Required]
        [Comment("Foreign key referencing the associated product.")]
        public Guid ProductId { get; set; }

        [Required]
        [Comment("Navigation property referencing the associated product entity.")]
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;

        [Required]
        [Comment("The number of units of the product in the order item.")]
        public int Quantity { get; set; }

        [Required]
        [Comment("The price per unit of the product in the order item.")]
        public decimal UnitPrice { get; set; }
    }
}
