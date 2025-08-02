namespace ServerAspNetCoreAPIMakePC.Application.DTOs.Feedback
{
    using System.ComponentModel.DataAnnotations;

    using static Domain.ErrorMessages.ErrorMessages;
    using static Domain.Constants.PlatformFeedbackValidationConstants;
    public class CreatePlatformFeedbackDto
    {
        /// <summary>
        /// The unique identifier of the user providing the feedback.
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// The rating value provided for the platform, within the allowed range.
        /// </summary>
        [Required]
        [Range(PlatformFeedbackRatingMinValue, PlatformFeedbackRatingMaxValue, ErrorMessage = InvalidFeedbackRating)]
        public int Rating { get; set; }

        /// <summary>
        /// The optional comment provided with the feedback.
        /// </summary>
        [StringLength(PlatformFeedbackCommentMaxLength, MinimumLength = PlatformFeedbackCommentMinLength, ErrorMessage = InvalidFeedbackComment)]
        public string Comment { get; set; } = string.Empty;
    }
}
