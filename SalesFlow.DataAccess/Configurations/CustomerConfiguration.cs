using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesFlow.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.DataAccess.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
           

            builder.Property(x => x.CompanyName)
                   .HasMaxLength(150);

            builder.Property(x => x.ContactFirstName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.ContactLastName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.Email)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.PhoneNumber)
                   .HasMaxLength(20);

            builder.Property(x => x.Website)
                   .HasMaxLength(200);

            builder.Property(x => x.TaxNumber)
                   .HasMaxLength(20);

            builder.Property(x => x.Address)
                   .HasMaxLength(300);

            builder.Property(x => x.Description)
                   .HasMaxLength(1000);

            builder.HasIndex(x => x.Email);
            builder.HasOne(x => x.AssignedUser)
    .WithMany(x => x.Customers)
    .HasForeignKey(x => x.AssignedUserId)
    .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
