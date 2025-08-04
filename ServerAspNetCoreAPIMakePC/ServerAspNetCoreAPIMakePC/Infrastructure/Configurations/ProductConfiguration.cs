namespace ServerAspNetCoreAPIMakePC.Infrastructure.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    
    using Domain.Entities;
    using Domain.ValueObjects;
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .HasConversion(
                    name => name.Value,
                    value => new ProductName(value))
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.Specs)
                .HasConversion(
                    specs => specs.Value,
                    value => new ProductSpecs(value))
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(p => p.Type)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,4)")
                .IsRequired();

            builder.Property(p => p.Description)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(p => p.ImageUrl)
                .IsRequired();

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            builder.HasOne(p => p.Brand)
                .WithMany(b => b.Products);

            builder.HasMany(p => p.Reviews)
                .WithOne(r => r.Product)
                .HasForeignKey(r => r.ProductId);
        }
    }
}
