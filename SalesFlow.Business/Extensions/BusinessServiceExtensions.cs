using FluentValidation;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using SalesFlow.Business.Services.AttachmentServices;
using SalesFlow.Business.Services.AuthServices;
using SalesFlow.Business.Services.CustomerServices;
using SalesFlow.Business.Services.DealServices;
using SalesFlow.Business.Services.JwtServices;
using SalesFlow.Business.Services.LeadServices;
using SalesFlow.Business.Services.MeetingServices;
using SalesFlow.Business.Services.NoteServices;
using SalesFlow.Business.Services.TaskItemServices;
using SalesFlow.Business.Validations.CustomerValidators;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Extensions
{
    public static class BusinessServiceExtensions
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {


            services.AddScoped<CustomerBusinessRules>();

            services.AddScoped<ICustomerService, CustomerService>();
            services.AddValidatorsFromAssemblyContaining<CreateCustomerValidator>();
            services.AddScoped<LeadBusinessRules>();
            services.AddScoped<ILeadService, LeadService>();
            services.AddScoped<DealBusinessRules>();
            services.AddScoped<IDealService, DealService>();
            services.AddScoped<MeetingBusinessRules>();
            services.AddScoped<ITaskItemService, TaskItemService>();
            services.AddScoped<TaskItemBusinessRules>();
            services.AddScoped<IMeetingService, MeetingService>();
            services.AddScoped<NoteBusinessRules>();
            services.AddScoped<INoteService, NoteService>();
            services.AddScoped<AttachmentBusinessRules>();
            services.AddScoped<IAttachmentService, AttachmentService>();
            services.AddScoped<AuthBusinessRules>();
            services.AddScoped<IAuthService, AuthService>();

            // JWT
            services.AddScoped<ITokenService, TokenService>();
            return services;
        }
    }
}
