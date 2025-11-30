using Domain.Orders;
using Domain.Customers;
using Domain.Weapons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");
        
        builder.HasKey(o => o.Id);
        
        builder.Property(o => o.OrderNumber)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.HasIndex(o => o.OrderNumber)
            .IsUnique();
        
        builder.Property(o => o.CustomerId)
            .IsRequired();
            
        builder.Property(o => o.WeaponId)
            .IsRequired();
        
        builder.Property(o => o.OrderDate)
            .IsRequired();
        
        builder.Property(o => o.Status)
            .IsRequired();
        
        builder.Property(o => o.TotalAmount)
            .HasColumnType("decimal(18,2)");
        
        builder.Property(o => o.Notes)
            .HasMaxLength(1000);
        
        builder.Property(o => o.CreatedAt)
            .IsRequired();
        
        builder.Property(o => o.UpdatedAt);
        
        builder.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
            
        builder.HasOne<Weapon>()
            .WithMany()
            .HasForeignKey(o => o.WeaponId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}