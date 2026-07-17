using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SalesFlow.DataAccess.Extensions;
using SalesFlow.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.DataAccess.Context
{
    public class AppDbContext:IdentityDbContext<AppUser,AppRole,int>
    {
        public AppDbContext(DbContextOptions options):base(options)
        {
            
        }
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Lead> Leads { get; set; }

        public DbSet<Deal> Deals { get; set; }

        public DbSet<Meeting> Meetings { get; set; }

        public DbSet<TaskItem> WorkItems { get; set; }

        public DbSet<Note> Notes { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<CustomerTag> CustomerTags { get; set; }

        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            builder.ApplyGlobalQueryFilter();
        }
    }
}
