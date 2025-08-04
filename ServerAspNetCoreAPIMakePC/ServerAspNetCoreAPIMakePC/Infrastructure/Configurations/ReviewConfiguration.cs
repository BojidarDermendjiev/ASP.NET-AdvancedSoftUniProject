namespace ServerAspNetCoreAPIMakePC.Infrastructure.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Domain.Entities;
    using Domain.ValueObjects;
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Rating)
                .HasConversion(
                    r => r.Value,
                    v => new Rating(v))
                .IsRequired();

            builder.Property(r => r.Comment)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(r => r.Date)
                .IsRequired();
        }
    }
}
