namespace ServerAspNetCoreAPIMakePC.Application.DTOs.Review
{
    using System.ComponentModel.DataAnnotations;

    using static Domain.ErrorMessages.ErrorMessages;
    using static Domain.Constants.ReviewValidationConstants;
    public class UpdateReviewDto
    {
        [Required]
        public int Id { get; set; }


        [Required]
        [Range(ReviewRatingMinValue, ReviewRatingMaxValue, ErrorMessage = InvalidReviewRating)]
        public int Rating { get; set; }

        [Required]
        [StringLength(ReviewCommentMaxLength, MinimumLength = ReviewCommentMinLength, ErrorMessage = InvalidReviewComment)]
        public string Comment { get; set; } = null!;
    }
}
