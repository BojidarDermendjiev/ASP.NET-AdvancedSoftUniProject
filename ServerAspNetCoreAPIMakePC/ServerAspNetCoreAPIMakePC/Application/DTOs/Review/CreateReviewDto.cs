namespace ServerAspNetCoreAPIMakePC.Application.DTOs.Review
{
    using System.ComponentModel.DataAnnotations;

    using static Domain.ErrorMessages.ErrorMessages;
    using static Domain.Constants.ReviewValidationConstants;

    public class CreateReviewDto
    {
        /// <summary>
        /// The unique identifier of the review.
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// The unique identifier of the user who creates the review.
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// The unique identifier of the product being reviewed.
        /// Optional field.
        /// </summary>
        public Guid? ProductId { get; set; }

        /// <summary>
        /// The rating given in the review. Must be within the allowed range.
        /// </summary>
        [Required]
        [Range(ReviewRatingMinValue, ReviewRatingMaxValue, ErrorMessage = InvalidReviewRating)]
        public int Rating { get; set; }

        /// <summary>
        /// The comment or message provided in the review.
        /// Must be within the allowed length range.
        /// </summary>
        [Required]
        [StringLength(ReviewCommentMaxLength, MinimumLength = ReviewCommentMinLength, ErrorMessage = InvalidReviewComment)]
        public string Comment { get; set; } = null!;
    }
}
