namespace ServerAspNetCoreAPIMakePC.Infrastructure.Data.DbSeed
{
    using Domain.Entities;
    using Domain.ValueObjects;

    public class PlatformFeedbackSeed
    {
        public static List<PlatformFeedback> GetPlatformFeedbacks(List<User> users)
        {
            User User(string email) => users.First(u => u.Email.Value == email);

            return new List<PlatformFeedback>
            {
                new PlatformFeedback
                {
                    UserId = User("user@example.com").Id,
                    User = User("user@example.com"),
                    Rating = new Rating(5),
                    Comment = new FeedbackComment("Amazing platform! The UI is very intuitive and fast."),
                    DateGiven = DateTime.UtcNow.AddDays(-14)
                },
                new PlatformFeedback
                {
                    UserId = User("admin@example.com").Id,
                    User = User("admin@example.com"),
                    Rating = new Rating(4),
                    Comment = new FeedbackComment(
                        "Very useful for building custom PCs. Would love to see more payment options."),
                    DateGiven = DateTime.UtcNow.AddDays(-11)
                },
                new PlatformFeedback
                {
                    UserId = User("user@example.com").Id,
                    User = User("user@example.com"),
                    Rating = new Rating(3),
                    Comment = new FeedbackComment("Good, but sometimes the product filters are slow."),
                    DateGiven = DateTime.UtcNow.AddDays(-6)
                },
                new PlatformFeedback
                {
                    UserId = User("admin@example.com").Id,
                    User = User("admin@example.com"),
                    Rating = new Rating(5),
                    Comment = new FeedbackComment("Support team was very helpful and quick!"),
                    DateGiven = DateTime.UtcNow.AddDays(-3)
                },
                new PlatformFeedback
                {
                    UserId = User("user@example.com").Id,
                    User = User("user@example.com"),
                    Rating = new Rating(4),
                    Comment = new FeedbackComment("Great deals and discounts on hardware. Navigation can be improved."),
                    DateGiven = DateTime.UtcNow.AddDays(-1)
                }
            };
        }
    }
}
