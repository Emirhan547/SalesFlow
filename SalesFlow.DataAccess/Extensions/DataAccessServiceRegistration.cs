using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SalesFlow.DataAccess.Context;
using SalesFlow.DataAccess.Repositories.AttachmentRepositories;
using SalesFlow.DataAccess.Repositories.CustomerRepositories;
using SalesFlow.DataAccess.Repositories.CustomerTagRepositories;
using SalesFlow.DataAccess.Repositories.DealRepositories;
using SalesFlow.DataAccess.Repositories.LeadRepositories;
using SalesFlow.DataAccess.Repositories.MeetingRepositories;
using SalesFlow.DataAccess.Repositories.NoteRepositories;
using SalesFlow.DataAccess.Repositories.TagRepositories;
using SalesFlow.DataAccess.Repositories.WorkItemRepositories;
using SalesFlow.DataAccess.Uows;
using SalesFlow.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.DataAccess.Extensions
{
    public static class DataAccessServiceRegistration
    {
        public static IServiceCollection AddDataAccessServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SqlServer"));
            });

            services.AddIdentityCore<AppUser>().AddRoles<AppRole>().AddEntityFrameworkStores<AppDbContext>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ILeadRepository, LeadRepository>();
            services.AddScoped<IDealRepository, DealRepository>();
            services.AddScoped<IMeetingRepository, MeetingRepository>();
            services.AddScoped<IWorkItemRepository, WorkItemRepository>();
            services.AddScoped<INoteRepository, NoteRepository>();
            services.AddScoped<IAttachmentRepository, AttachmentRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<ICustomerTagRepository, CustomerTagRepository>();

            return services;
        }
    }
}
