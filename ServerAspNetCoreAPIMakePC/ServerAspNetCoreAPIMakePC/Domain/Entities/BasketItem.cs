namespace ServerAspNetCoreAPIMakePC.Domain.Entities
{
    using Microsoft.EntityFrameworkCore;
    
    using ValueObjects;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class BasketItem
    {
      
        [Required]
        [Comment("Unique identifier for the basket item.")]
        public int Id { get; set; }

        [Required]
        [Comment("Foreign key referencing the associated basket.")]
        public int BasketId { get; set; }

        [Required]
        [Comment("Navigation property referencing the associated basket entity.")]
        [ForeignKey(nameof(BasketId))]
        public  Basket Basket { get; set; } = null!;

        [Required]
        [Comment("Foreign key referencing the associated product.")]
        public Guid ProductId { get; set; }

        [Required]
        [Comment("Navigation property referencing the associated product entity.")]
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;

        [Required]
        [Comment("The number of units of the product in the basket.")]
        public Quantity Quantity { get; set; } = null!;
    }
}
