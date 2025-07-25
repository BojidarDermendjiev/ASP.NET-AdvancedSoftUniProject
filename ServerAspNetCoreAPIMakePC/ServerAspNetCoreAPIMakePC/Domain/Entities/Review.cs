namespace ServerAspNetCoreAPIMakePC.Domain.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public int? ProductId { get; set; }
        public Product? Product { get; set; } 
        public int? TutorialId { get; set; }
        public Tutorial? Tutorial { get; set; }
        public int Rating { get; set; } 
        public string Comment { get; set; } = null!;
        public DateTime Date { get; set; }
    }
}
