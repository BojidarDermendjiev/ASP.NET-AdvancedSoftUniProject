namespace ServerAspNetCoreAPIMakePC.Domain.Entities
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static ErrorMessages.ErrorMessages;
    using static Constants.PlatformFeedbackValidationConstants;

    public class PlatformFeedback
    {
        [Required]
        [Comment("Unique identifier for the feedback entry.")]
        public int Id { get; set; }

        [Required]
        [Comment("Identifier for the user who provided the feedback.")]
        public Guid UserId { get; set; }

        [Required]
        [Comment("Navigation property to the user who provided the feedback.")]
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        [Required]
        [Comment("Identifier for the platform associated with the feedback.")]
        [Range(PlatformFeedbackRatingMinValue, PlatformFeedbackRatingMaxValue, ErrorMessage = InvalidFeedbackRating)]
        public int Rating { get; set; }

        [Comment("Text content of the feedback provided by the user.")]
        [StringLength(PlatformFeedbackCommentMaxLength, MinimumLength = PlatformFeedbackCommentMinLength, ErrorMessage = InvalidFeedbackComment)]
        public string Comment { get; set; } = string.Empty;

        [Required]
        [Comment("The date and time when the feedback was given.")]
        public DateTime DateGiven { get; set; } = DateTime.UtcNow;
    }
}
