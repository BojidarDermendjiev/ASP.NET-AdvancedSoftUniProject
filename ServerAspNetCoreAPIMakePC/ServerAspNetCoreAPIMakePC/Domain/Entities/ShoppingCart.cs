namespace ServerAspNetCoreAPIMakePC.Domain.Entities
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ShoppingCart
    {
        [Required]
        [Comment("Unique identifier for the shopping cart.")]
        public Guid Id { get; set; }

        [Required]
        [Comment("Identifier for the user who owns the shopping cart.")]
        public Guid UserId { get; set; }

        [Required]
        [Comment("Navigation property to the user associated with the shopping cart.")]
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        [Required]
        [Comment("Total price of the items in the shopping cart.")]
        public DateTime DateCreated { get; set; }

        [Comment("Total price of the items in the shopping cart.")]
        public virtual ICollection<BasketItem> Items { get; set; } = new List<BasketItem>();
    }
}
