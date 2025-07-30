namespace ServerAspNetCoreAPIMakePC.Application.DTOs.Feedback
{
    public class PlatformFeedbackDto
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; } = null!;
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime DateGiven { get; set; }
    }
}
