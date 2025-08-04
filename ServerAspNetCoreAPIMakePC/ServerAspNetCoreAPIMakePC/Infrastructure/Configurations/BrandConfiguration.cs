namespace ServerAspNetCoreAPIMakePC.Infrastructure.Configurations
{
    
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Domain.Entities;
    using Domain.ValueObjects;

    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Name)
                .HasConversion(
                    brandName => brandName.Value,
                    value => new BrandName(value))
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(b => b.Description)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(b => b.LogoUrl)
                .HasMaxLength(300)
                .IsRequired();
        }
    }
}
