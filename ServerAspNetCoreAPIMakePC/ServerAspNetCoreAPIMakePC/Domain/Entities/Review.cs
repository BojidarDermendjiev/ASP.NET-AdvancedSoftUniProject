namespace ServerAspNetCoreAPIMakePC.Domain.Entities
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static ErrorMessages.ErrorMessages;
    using static Constants.ReviewValidationConstants;

    public class Review
    {
        [Required]
        [Comment("Unique identifier for the review.")]
        public int Id { get; set; }

        [Required]
        [Comment("Identifier for the user who wrote the review.")]
        public Guid UserId { get; set; }

        [Required]
        [Comment("Navigation property to the user who wrote the review.")]
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        [Comment("Identifier for the product being reviewed.")]
        public int? ProductId { get; set; }

        [Comment("Navigation property to the product being reviewed.")]
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; } 

        [Required]
        [Comment("Rating given in the review, typically on a scale of 1 to 5.")]
        [Range(ReviewRatingMinValue, ReviewRatingMaxValue, ErrorMessage = InvalidReviewRating)]
        public int Rating { get; set; }

        [Required]
        [Comment("Text content of the review.")]
        [StringLength(ReviewCommentMaxLength, MinimumLength = ReviewCommentMinLength, ErrorMessage = InvalidReviewComment)]
        public string Comment { get; set; } = null!;
        public DateTime Date { get; set; }
    }
}
