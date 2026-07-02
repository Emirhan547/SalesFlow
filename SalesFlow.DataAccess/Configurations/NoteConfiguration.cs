using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesFlow.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.DataAccess.Configurations
{
    public class NoteConfiguration : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.Property(x => x.Content)
                   .IsRequired()
                   .HasMaxLength(2000);

            builder.HasOne(x => x.Customer)
                   .WithMany(x => x.Notes)
                   .HasForeignKey(x => x.CustomerId);

            builder.HasOne(x => x.CreatedBy)
                   .WithMany(x => x.Notes)
                   .HasForeignKey(x => x.CreatedById)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
