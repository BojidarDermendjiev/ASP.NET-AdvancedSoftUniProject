namespace ServerAspNetCoreAPIMakePC.Infrastructure.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Domain.Entities;
    using Domain.ValueObjects;
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.ShippingAddress)
                .HasConversion(
                    address => address.Value,
                    value => new ShippingAddress(value))
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(o => o.PaymentStatus)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(o => o.TotalPrice)
                .HasColumnType("decimal(18,4)")
                .IsRequired();

            builder.HasMany(o => o.Items)
                .WithOne(oi => oi.Order) 
                .HasForeignKey(oi => oi.OrderId) 
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
