using FluentValidation;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using SalesFlow.Business.Services.ActivityLogServices;
using SalesFlow.Business.Services.AttachmentServices;
using SalesFlow.Business.Services.AuthServices;
using SalesFlow.Business.Services.CustomerServices;
using SalesFlow.Business.Services.DashboardServices;
using SalesFlow.Business.Services.DealServices;
using SalesFlow.Business.Services.ExportServices;
using SalesFlow.Business.Services.FileServices;
using SalesFlow.Business.Services.JwtServices;
using SalesFlow.Business.Services.LeadServices;
using SalesFlow.Business.Services.MeetingServices;
using SalesFlow.Business.Services.NoteServices;
using SalesFlow.Business.Services.NotificationServices;
using SalesFlow.Business.Services.ProfileServices;
using SalesFlow.Business.Services.TagServices;
using SalesFlow.Business.Services.TaskItemServices;
using SalesFlow.Business.Services.UserServices;
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
            services.AddScoped<CustomerBusinessRules>();
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
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<TagBusinessRules>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            // JWT
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<ActivityLogBusinessRules>();
            services.AddScoped<IExcelExportService, ExcelExportService>();
            services.AddScoped<IActivityLogService, ActivityLogService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPdfExportService, PdfExportService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<ProfileBusinessRules>();
            services.AddScoped<
    INotificationService,
    NotificationService>();

            services.AddScoped<
                NotificationBusinessRules>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<AuthBusinessRules>();
            return services;
        }
    }
}
