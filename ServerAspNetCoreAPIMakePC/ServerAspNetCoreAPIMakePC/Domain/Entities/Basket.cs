
namespace ServerAspNetCoreAPIMakePC.Domain.Entities
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Basket
    {
        [Required]
        [Comment("Unique identifier for the basket.")]
        public int Id { get; set; }

        [Required]
        [Comment("Identifier for the user who owns the basket.")]
        public Guid UserId { get; set; }

        [Required]
        [Comment("Navigation property to the user associated with the basket.")]
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        [Required]
        [Comment("The date the basket was created.")]
        public DateTime DateCreated { get; set; }

        [Comment("The items contained within the basket.")]
        public ICollection<BasketItem> Items { get; set; } = new HashSet<BasketItem>();
    }
}
