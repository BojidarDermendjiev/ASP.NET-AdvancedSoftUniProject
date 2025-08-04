namespace ServerAspNetCoreAPIMakePC.Infrastructure.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Domain.Entities;
    using Domain.ValueObjects;
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Email)
                .HasConversion(
                    email => email.Value,
                    value => new Email(value))
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(u => u.FullName)
                .HasConversion(
                    name => name.Value,
                    value => new FullName(value))
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(u => u.PasswordHash)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(u => u.ConfirmPassword)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(u => u.Role)
                .HasMaxLength(30)
                .IsRequired();
        }
    }
}
