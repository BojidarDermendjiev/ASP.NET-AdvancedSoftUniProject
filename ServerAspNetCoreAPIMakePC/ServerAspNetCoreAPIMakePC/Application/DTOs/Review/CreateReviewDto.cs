namespace ServerAspNetCoreAPIMakePC.Application.DTOs.Review
{
    using System.ComponentModel.DataAnnotations;

    using static Domain.ErrorMessages.ErrorMessages;
    using static Domain.Constants.ReviewValidationConstants;

    public class CreateReviewDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public Guid UserId { get; set; }
        public Guid? ProductId { get; set; }

        [Required]
        [Range(ReviewRatingMinValue, ReviewRatingMaxValue, ErrorMessage = InvalidReviewRating)]
        public int Rating { get; set; }

        [Required]
        [StringLength(ReviewCommentMaxLength, MinimumLength = ReviewCommentMinLength, ErrorMessage = InvalidReviewComment)]
        public string Comment { get; set; } = null!;
    }
}
