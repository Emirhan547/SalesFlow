using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesFlow.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.DataAccess.Configurations
{
    public class DealConfiguration : IEntityTypeConfiguration<Deal>
    {
        public void Configure(EntityTypeBuilder<Deal> builder)
        {
            builder.Property(x => x.Title)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(x => x.Description)
                   .HasMaxLength(1000);

            builder.Property(x => x.Amount)
                   .HasColumnType("decimal(18,2)");

            builder.HasOne(x => x.Customer)
                   .WithMany(x => x.Deals)
                   .HasForeignKey(x => x.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.AssignedUser)
                   .WithMany(x => x.Deals)
                   .HasForeignKey(x => x.AssignedUserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
