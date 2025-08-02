namespace ServerAspNetCoreAPIMakePC.Application.DTOs.Feedback
{
    using System.ComponentModel.DataAnnotations;

    using static Domain.ErrorMessages.ErrorMessages;
    using static Domain.Constants.PlatformFeedbackValidationConstants;
    public class UpdatePlatformFeedbackDto
    {
        /// <summary>
        /// The unique identifier of the platform feedback to be updated.
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// The unique identifier of the user who provided the feedback.
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// The updated rating value for the platform.
        /// </summary>
        [Required]
        [Range(PlatformFeedbackRatingMinValue, PlatformFeedbackRatingMaxValue, ErrorMessage = InvalidFeedbackRating)]
        public int Rating { get; set; }

        /// <summary>
        /// The updated comment for the feedback.
        /// </summary>
        [StringLength(PlatformFeedbackCommentMaxLength, MinimumLength = PlatformFeedbackCommentMinLength, ErrorMessage = InvalidFeedbackComment)]
        public string Comment { get; set; } = string.Empty;
    }
}
