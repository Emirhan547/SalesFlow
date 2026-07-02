using FluentValidation;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using SalesFlow.Business.Services.CustomerServices;
using SalesFlow.Business.Services.LeadServices;
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
            return services;
        }
    }
}
