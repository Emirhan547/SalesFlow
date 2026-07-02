using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesFlow.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.DataAccess.Configurations
{
    public class WorkItemConfiguration : IEntityTypeConfiguration<WorkItem>
    {
        public void Configure(EntityTypeBuilder<WorkItem> builder)
        {
            builder.Property(x => x.Title)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(x => x.Description)
                   .HasMaxLength(1000);

            builder.HasOne(x => x.Customer)
                   .WithMany()
                   .HasForeignKey(x => x.CustomerId);

            builder.HasOne(x => x.AssignedUser)
                   .WithMany(x => x.WorkItems)
                   .HasForeignKey(x => x.AssignedUserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
