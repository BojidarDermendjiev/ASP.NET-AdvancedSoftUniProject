namespace ServerAspNetCoreAPIMakePC.Application.DTOs.Feedback
{
    using System.ComponentModel.DataAnnotations;

    using static Domain.ErrorMessages.ErrorMessages;
    using static Domain.Constants.PlatformFeedbackValidationConstants;
    public class CreatePlatformFeedbackDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        [Range(PlatformFeedbackRatingMinValue, PlatformFeedbackRatingMaxValue, ErrorMessage = InvalidFeedbackRating)]
        public int Rating { get; set; }

        [StringLength(PlatformFeedbackCommentMaxLength, MinimumLength = PlatformFeedbackCommentMinLength, ErrorMessage = InvalidFeedbackComment)]
        public string Comment { get; set; } = string.Empty;
    }
}
