namespace ServerAspNetCoreAPIMakePC.Domain.Entities
{
    public class Tutorial
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public Guid? AuthorId { get; set; }
        public User Author { get; set; } = null!;
        public string Difficulty { get; set; } = null!;
        public TimeSpan? EstimatedTime { get; set; }
        public virtual ICollection<TutorialStep> Steps { get; set; } = null!;

    }
}
