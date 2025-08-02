namespace ServerAspNetCoreAPIMakePC.Application.DTOs.Feedback
{
    public class PlatformFeedbackDto
    {
        /// <summary>
        /// The unique identifier of the platform feedback.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The unique identifier of the user who provided the feedback.
        /// </summary>
        public Guid UserId { get; set; }
        
        /// <summary>
        /// The name of the user who provided the feedback.
        /// </summary>
        public string UserName { get; set; } = null!;

        /// <summary>
        /// The rating value given for the platform.
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// The comment included with the feedback.
        /// </summary>
        public string Comment { get; set; } = string.Empty;

        /// <summary>
        /// The date and time when the feedback was given.
        /// </summary>
        public DateTime DateGiven { get; set; }
    }
}
