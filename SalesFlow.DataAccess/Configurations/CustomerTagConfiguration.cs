using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesFlow.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.DataAccess.Configurations
{
    public class CustomerTagConfiguration : IEntityTypeConfiguration<CustomerTag>
    {
        public void Configure(EntityTypeBuilder<CustomerTag> builder)
        {
            builder.HasOne(x => x.Customer)
                   .WithMany(x => x.CustomerTags)
                   .HasForeignKey(x => x.CustomerId);

            builder.HasOne(x => x.Tag)
                   .WithMany(x => x.CustomerTags)
                   .HasForeignKey(x => x.TagId);

            builder.HasIndex(x => new { x.CustomerId, x.TagId })
                   .IsUnique();
        }
    }
}
