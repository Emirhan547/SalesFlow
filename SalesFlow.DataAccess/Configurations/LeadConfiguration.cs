using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesFlow.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.DataAccess.Configurations
{
    public class LeadConfiguration : IEntityTypeConfiguration<Lead>
    {
        public void Configure(EntityTypeBuilder<Lead> builder)
        {
            builder.Property(x => x.FirstName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.LastName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.CompanyName)
                   .HasMaxLength(150);

            builder.Property(x => x.Email)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasIndex(x => x.Email);

            builder.Property(x => x.PhoneNumber)
                   .HasMaxLength(20);

            builder.Property(x => x.Website)
                   .HasMaxLength(200);

            builder.Property(x => x.Address)
                   .HasMaxLength(300);

            builder.Property(x => x.Description)
                   .HasMaxLength(1000);

            builder.HasOne(x => x.AssignedUser)
                   .WithMany(x => x.AssignedLeads)
                   .HasForeignKey(x => x.AssignedUserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
