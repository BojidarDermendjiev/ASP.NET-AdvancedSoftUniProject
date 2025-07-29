namespace ServerAspNetCoreAPIMakePC.Application.DTOs.Review
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public Guid? ProductId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = null!;
        public DateTime Date { get; set; }
        public string? UserName { get; set; }
        public string? ProductName { get; set; }
    }
}
