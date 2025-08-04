namespace ServerAspNetCoreAPIMakePC.Infrastructure.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Domain.Entities;
    using Domain.ValueObjects;
    public class PlatformFeedbackConfiguration : IEntityTypeConfiguration<PlatformFeedback>
    {
        public void Configure(EntityTypeBuilder<PlatformFeedback> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Rating)
                .HasConversion(
                    r => r.Value,
                    v => new Rating(v))
                .IsRequired();

            builder.Property(f => f.Comment)
                .HasConversion(
                    c => c.Value,
                    v => new FeedbackComment(v))
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(f => f.DateGiven)
                .IsRequired();
        }
    }
}
