using Domain.Customers;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("customers");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(c => c.Email)
            .IsUnique();

        builder.Property(c => c.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(c => c.LicenseNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(c => c.LicenseNumber)
            .IsUnique();

        builder.Property(c => c.IsVerified)
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .IsRequired()
            .HasConversion<DateTimeUtcConverter>();

        builder.Property(c => c.UpdatedAt)
            .HasConversion<DateTimeUtcConverter>();
    }
}