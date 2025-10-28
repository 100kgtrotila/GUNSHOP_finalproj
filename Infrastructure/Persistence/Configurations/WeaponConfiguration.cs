using Domain.Weapons;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class WeaponConfiguration : IEntityTypeConfiguration<Weapon>
{
    public void Configure(EntityTypeBuilder<Weapon> builder)
    {
        builder.ToTable("weapons");

        builder.HasKey(w => w.Id);

        builder.Property(w => w.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(w => w.Manufacturer)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(w => w.Model)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(w => w.Caliber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(w => w.SerialNumber)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(w => w.SerialNumber)
            .IsUnique();

        builder.Property(w => w.Price)
            .HasColumnType("decimal(18,2)");

        builder.Property(w => w.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(w => w.Category)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(w => w.CreatedAt)
            .IsRequired()
            .HasConversion<DateTimeUtcConverter>();

        builder.Property(w => w.UpdatedAt)
            .HasConversion<DateTimeUtcConverter>();
    }
}