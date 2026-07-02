using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesFlow.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.DataAccess.Configurations
{
    public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
    {
        public void Configure(EntityTypeBuilder<Attachment> builder)
        {
            builder.Property(x => x.FileName)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(x => x.FilePath)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(x => x.ContentType)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasOne(x => x.Customer)
                   .WithMany(x => x.Attachments)
                   .HasForeignKey(x => x.CustomerId);
        }
    }
}