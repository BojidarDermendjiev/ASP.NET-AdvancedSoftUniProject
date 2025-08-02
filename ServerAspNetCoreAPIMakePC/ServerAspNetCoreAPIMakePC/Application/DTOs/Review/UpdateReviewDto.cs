namespace ServerAspNetCoreAPIMakePC.Application.DTOs.Review
{
    using System.ComponentModel.DataAnnotations;

    using static Domain.ErrorMessages.ErrorMessages;
    using static Domain.Constants.ReviewValidationConstants;
    public class UpdateReviewDto
    {

        /// <summary>
        /// The unique identifier of the review to be updated.
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// The updated rating value for the review. Must be within the allowed range.
        /// </summary>
        [Required]
        [Range(ReviewRatingMinValue, ReviewRatingMaxValue, ErrorMessage = InvalidReviewRating)]
        public int Rating { get; set; }

        /// <summary>
        /// The updated comment for the review. Must be within the allowed length.
        /// </summary>
        [Required]
        [StringLength(ReviewCommentMaxLength, MinimumLength = ReviewCommentMinLength, ErrorMessage = InvalidReviewComment)]
        public string Comment { get; set; } = null!;
    }
}
