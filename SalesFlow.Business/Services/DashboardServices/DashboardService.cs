using Microsoft.EntityFrameworkCore;
using SalesFlow.Business.Dtos.ActivityLogDtos;
using SalesFlow.Business.Dtos.CustomerDtos;
using SalesFlow.Business.Dtos.DashboardDtos;
using SalesFlow.Business.Dtos.DealDtos;
using SalesFlow.Business.Dtos.LeadDtos;
using SalesFlow.Business.Dtos.MeetingDtos;
using SalesFlow.Business.Dtos.TaskItemDtos;
using SalesFlow.Core.Results;
using SalesFlow.DataAccess.Repositories.ActivityRepositories;
using SalesFlow.DataAccess.Repositories.CustomerRepositories;
using SalesFlow.DataAccess.Repositories.DealRepositories;
using SalesFlow.DataAccess.Repositories.LeadRepositories;
using SalesFlow.DataAccess.Repositories.MeetingRepositories;
using SalesFlow.DataAccess.Repositories.TaskItemRepositories;
using SalesFlow.Entity.Enums;
using TaskStatus = SalesFlow.Entity.Enums.TaskStatus;

namespace SalesFlow.Business.Services.DashboardServices
{
    public class DashboardService : IDashboardService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILeadRepository _leadRepository;
        private readonly IDealRepository _dealRepository;
        private readonly IMeetingRepository _meetingRepository;
        private readonly ITaskItemRepository _taskRepository;
        private readonly IActivityLogRepository _activityLogRepository;
        public DashboardService(ICustomerRepository customerRepository, ILeadRepository leadRepository, IDealRepository dealRepository, IMeetingRepository meetingRepository, ITaskItemRepository taskRepository, IActivityLogRepository activityLogRepository)
        {
            _customerRepository = customerRepository;
            _leadRepository = leadRepository;
            _dealRepository = dealRepository;
            _meetingRepository = meetingRepository;
            _taskRepository = taskRepository;
            _activityLogRepository = activityLogRepository;
        }

        public async Task<Result<DashboardDto>> GetDashboardAsync()
        {
            DashboardDto dashboard = new()
            {
                Summary = await GetSummaryAsync(),

                Sales = await GetSalesStatisticsAsync(),

                Leads = await GetLeadStatisticsAsync(),

                Meetings = await GetMeetingStatisticsAsync(),

                Tasks = await GetTaskStatisticsAsync(),

                Recent = await GetRecentActivitiesAsync(),

                RecentActivities = await GetRecentActivitiesTimelineAsync()
            };

            return Result<DashboardDto>.Success(dashboard);
        }
        private async Task<DashboardSummaryDto> GetSummaryAsync()
        {
            return new DashboardSummaryDto
            {
                TotalCustomers = await _customerRepository.GetAll().CountAsync(),

                TotalLeads = await _leadRepository.GetAll().CountAsync(),

                TotalDeals = await _dealRepository.GetAll().CountAsync(),

                TotalMeetings = await _meetingRepository.GetAll().CountAsync(),

                TotalTasks = await _taskRepository.GetAll().CountAsync()
            };
        }
        private async Task<DashboardSalesDto> GetSalesStatisticsAsync()
        {
            return new DashboardSalesDto
            {
                PipelineAmount = await _dealRepository.GetAll()
                    .Where(x => x.Stage != DealStage.Won &&
                                x.Stage != DealStage.Lost)
                    .SumAsync(x => x.Amount),

                WonAmount = await _dealRepository.GetAll()
                    .Where(x => x.Stage == DealStage.Won)
                    .SumAsync(x => x.Amount),

                WonDeals = await _dealRepository.GetAll()
                    .CountAsync(x => x.Stage == DealStage.Won),

                LostDeals = await _dealRepository.GetAll()
                    .CountAsync(x => x.Stage == DealStage.Lost),

                ActiveDeals = await _dealRepository.GetAll()
                    .CountAsync(x => x.Stage != DealStage.Won &&
                                     x.Stage != DealStage.Lost),
                    MonthlySales = await _dealRepository
    .GetAll()
    .Where(x => x.Stage == DealStage.Won)
    .GroupBy(x => new
    {
        x.CreatedDate.Year,
        x.CreatedDate.Month
    })
    .OrderBy(x => x.Key.Year)
    .ThenBy(x => x.Key.Month)
    .Select(x => new MonthlySalesDto
    {
        Month = new DateTime(
            x.Key.Year,
            x.Key.Month,
            1
        ).ToString("MMM"),

        Amount = x.Sum(y => y.Amount)
    })
    .ToListAsync()
            };
        }
        private async Task<DashboardLeadDto> GetLeadStatisticsAsync()
        {
            return new DashboardLeadDto
            {
                New = await _leadRepository.GetAll()
                    .CountAsync(x => x.Status == LeadStatus.New),

                Contacted = await _leadRepository.GetAll()
                    .CountAsync(x => x.Status == LeadStatus.Contacted),

                Qualified = await _leadRepository.GetAll()
                    .CountAsync(x => x.Status == LeadStatus.Qualified),

                Converted = await _leadRepository.GetAll()
                    .CountAsync(x => x.Status == LeadStatus.Converted),

                Lost = await _leadRepository.GetAll()
                    .CountAsync(x => x.Status == LeadStatus.Lost)
            };
        }
        private async Task<DashboardMeetingDto> GetMeetingStatisticsAsync()
        {
            DateTime today = DateTime.Today;
            DateTime nextWeek = today.AddDays(7);

            IQueryable<Entity.Entities.Meeting> meetings = _meetingRepository.GetAll();

            return new DashboardMeetingDto
            {
                Today = await meetings.CountAsync(x =>
                    x.StartDate.Date == today),

                ThisWeek = await meetings.CountAsync(x =>
                    x.StartDate >= today &&
                    x.StartDate < nextWeek),

                Scheduled = await meetings.CountAsync(x =>
                    x.Status == MeetingStatus.Scheduled),

                Completed = await meetings.CountAsync(x =>
                    x.Status == MeetingStatus.Completed)
            };
        }

        private async Task<DashboardTaskDto> GetTaskStatisticsAsync()
        {
            IQueryable<Entity.Entities.TaskItem> tasks = _taskRepository.GetAll();

            return new DashboardTaskDto
            {
                Pending = await tasks.CountAsync(x =>
                    x.Status == TaskStatus.Pending),

                InProgress = await tasks.CountAsync(x =>
                    x.Status == TaskStatus.InProgress),

                Completed = await tasks.CountAsync(x =>
                    x.Status == TaskStatus.Completed),

                Cancelled = await tasks.CountAsync(x =>
                    x.Status == TaskStatus.Cancelled)
            };
        }
        private async Task<DashboardRecentDto> GetRecentActivitiesAsync()
        {
            List<RecentCustomerDto> customers = await _customerRepository
                .GetAll()
                .OrderByDescending(x => x.CreatedDate)
                .Take(5)
                .Select(x => new RecentCustomerDto
                {
                    Id = x.Id,
                    FullName = $"{x.ContactFirstName} {x.ContactLastName}",
                    CompanyName = x.CompanyName,
                    CreatedDate = x.CreatedDate
                })
                .ToListAsync();

            List<RecentLeadDto> leads = await _leadRepository
                .GetAll()
                .OrderByDescending(x => x.CreatedDate)
                .Take(5)
                .Select(x => new RecentLeadDto
                {
                    Id = x.Id,
                    FullName = $"{x.FirstName} {x.LastName}",
                    CompanyName = x.CompanyName,
                    Status = x.Status,
                    CreatedDate = x.CreatedDate
                })
                .ToListAsync();

            List<RecentDealDto> deals = await _dealRepository
                .GetAll()
                .OrderByDescending(x => x.CreatedDate)
                .Take(5)
                .Select(x => new RecentDealDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Amount = x.Amount,
                    Stage = x.Stage,
                    CreatedDate = x.CreatedDate
                })
                .ToListAsync();

            List<UpcomingMeetingDto> upcomingMeetings = await _meetingRepository
                .GetAll()
                .Where(x => x.StartDate >= DateTime.Now)
                .OrderBy(x => x.StartDate)
                .Take(5)
                .Select(x => new UpcomingMeetingDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    StartDate = x.StartDate,
                    Status = x.Status,
                    CustomerName = x.Customer.ContactFirstName + " " + x.Customer.ContactLastName
                })
                .ToListAsync();

            List<UpcomingTaskDto> upcomingTasks = await _taskRepository
                .GetAll()
                .Where(x => x.Status != TaskStatus.Completed &&
                            x.Status != TaskStatus.Cancelled)
                .OrderBy(x => x.DueDate)
                .Take(5)
                .Select(x => new UpcomingTaskDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    DueDate = x.DueDate,
                    Status = x.Status,
                    CustomerName = x.Customer.ContactFirstName + " " + x.Customer.ContactLastName
                })
                .ToListAsync();

            return new DashboardRecentDto
            {
                Customers = customers,
                Leads = leads,
                Deals = deals,
                UpcomingMeetings = upcomingMeetings,
                UpcomingTasks = upcomingTasks
            };
        }
        private async Task<List<ResultActivityLogDto>> GetRecentActivitiesTimelineAsync()
        {
            return await _activityLogRepository
                .GetAll()
                .OrderByDescending(x => x.CreatedDate)
                .Take(10)
                .Select(x => new ResultActivityLogDto
                {
                    Id = x.Id,
                    Action = x.Action,
                    EntityName = x.EntityName,
                    EntityId = x.EntityId,
                    Description = x.Description,
                    UserId = x.UserId,
                    UserName = x.User == null
                        ? null
                        : $"{x.User.FirstName} {x.User.LastName}",
                    CreatedDate = x.CreatedDate
                })
                .ToListAsync();
        }
    }
}
