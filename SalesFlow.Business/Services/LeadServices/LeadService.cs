using FluentValidation;
using Mapster;
using SalesFlow.Business.Dtos.LeadDtos;
using SalesFlow.Business.Services.ActivityLogServices;
using SalesFlow.Business.Services.CustomerServices;
using SalesFlow.Business.Services.ExportServices;
using SalesFlow.Business.Services.UserServices;
using SalesFlow.Core.Paginations;
using SalesFlow.Core.Results;
using SalesFlow.DataAccess.Repositories.CustomerRepositories;
using SalesFlow.DataAccess.Repositories.DealRepositories;
using SalesFlow.DataAccess.Repositories.LeadRepositories;
using SalesFlow.DataAccess.Repositories.MeetingRepositories;
using SalesFlow.DataAccess.Repositories.TaskItemRepositories;
using SalesFlow.DataAccess.Uows;
using SalesFlow.Entity.Entities;
using SalesFlow.Entity.Enums;
using Microsoft.EntityFrameworkCore;

namespace SalesFlow.Business.Services.LeadServices
{
    public class LeadService : ILeadService
    {
        private readonly ILeadRepository _leadRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly LeadBusinessRules _leadBusinessRules;
        private readonly IValidator<CreateLeadDto> _createValidator;
        private readonly IValidator<UpdateLeadDto> _updateValidator;
        private readonly IValidator<ConvertLeadDto> _convertValidator;
        private readonly ICustomerRepository _customerRepository;
        private readonly CustomerBusinessRules _customerBusinessRules;
        private readonly IDealRepository _dealRepository;
        private readonly IMeetingRepository _meetingRepository;
        private readonly ITaskItemRepository _taskRepository;
        private readonly IActivityLogService _activityLogService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IExcelExportService _excelExportService;
        private readonly IPdfExportService  _pdfExportService;
        public LeadService(ILeadRepository leadRepository, IUnitOfWork unitOfWork, LeadBusinessRules leadBusinessRules, IValidator<CreateLeadDto> createValidator, IValidator<UpdateLeadDto> updateValidator, IValidator<ConvertLeadDto> convertValidator, ICustomerRepository customerRepository, CustomerBusinessRules customerBusinessRules, IDealRepository dealRepository, IMeetingRepository meetingRepository, ITaskItemRepository taskRepository, IActivityLogService activityLogService, ICurrentUserService currentUserService, IExcelExportService excelExportService, IPdfExportService pdfExportService)
        {
            _leadRepository = leadRepository;
            _unitOfWork = unitOfWork;
            _leadBusinessRules = leadBusinessRules;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _convertValidator = convertValidator;
            _customerRepository = customerRepository;
            _customerBusinessRules = customerBusinessRules;
            _dealRepository = dealRepository;
            _meetingRepository = meetingRepository;
            _taskRepository = taskRepository;
            _activityLogService = activityLogService;
            _currentUserService = currentUserService;
            _excelExportService = excelExportService;
            _pdfExportService = pdfExportService;
        }

        public async Task<Result> ConvertAsync(int leadId, ConvertLeadDto dto)
        {
            await _convertValidator.ValidateAndThrowAsync(dto);

            Lead lead = await _leadBusinessRules.GetLeadByIdAsync(leadId, true);
            _leadBusinessRules.EnsureUserCanModify(lead);

            _leadBusinessRules.EnsureLeadCanBeConverted(lead);
            await _customerBusinessRules.EnsureEmailIsUniqueAsync(lead.Email);

            Customer customer = CreateCustomerFromLead(lead, dto.CustomerType);

            await _customerRepository.AddAsync(customer);

            if (dto.CreateInitialDeal)
            {
                await CreateInitialDealAsync(customer, lead);
            }

            if (dto.CreateInitialMeeting)
            {
                await CreateInitialMeetingAsync(customer, lead);
            }

            if (dto.CreateInitialTask)
            {
                await CreateInitialTaskAsync(customer, lead);
            }

            lead.Status = LeadStatus.Converted;

            _leadRepository.Update(lead);
          
            await _unitOfWork.SaveChangesAsync();

            return Result.Success("Lead converted successfully.");
        }
        public async Task<Result> CreateAsync(CreateLeadDto dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);
            await _leadBusinessRules.EnsureEmailIsUniqueAsync(dto.Email);
            var lead = dto.Adapt<Lead>();
            lead.Status = LeadStatus.New;
            await _leadRepository.AddAsync(lead);
            await _activityLogService.AddAsync(ActivityAction.Create, nameof(Lead), lead.Id, $"Lead '{lead.FirstName} {lead.LastName}' created.", _currentUserService.UserId);

            await _unitOfWork.SaveChangesAsync();
            return Result.Success("Lead created successfully.");
        }

        public async Task<Result> UpdateAsync(UpdateLeadDto dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);
            var lead = await _leadBusinessRules.GetLeadByIdAsync(dto.Id, true);

            _leadBusinessRules.EnsureUserCanModify(lead);

            await _leadBusinessRules.EnsureEmailIsUniqueForUpdateAsync(dto.Id, dto.Email);
            dto.Adapt(lead);
            _leadRepository.Update(lead);
            await _activityLogService.AddAsync(ActivityAction.Update, nameof(Lead), lead.Id, $"Lead '{lead.FirstName} {lead.LastName}' updated.", _currentUserService.UserId);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success("Lead updated successfully.");
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var lead = await _leadBusinessRules.GetLeadByIdAsync(id, true);
            _leadBusinessRules.EnsureUserCanModify(lead);
            _leadRepository.Delete(lead);
            await _activityLogService.AddAsync(ActivityAction.Delete, nameof(Lead), lead.Id, $"Lead '{lead.FirstName} {lead.LastName}' deleted.", _currentUserService.UserId);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success("Lead deleted successfully.");
        }

        public async Task<Result<PagedResult<ResultLeadDto>>> GetAllAsync(PaginationRequest request)
        {
            IQueryable<Lead> query = _leadRepository.GetAll();

            if (!_currentUserService.IsInRole("Admin") &&
                !_currentUserService.IsInRole("SalesManager"))
            {
                query = query.Where(x =>
                    x.AssignedUserId == _currentUserService.UserId);
            }

            var leads = await query
                .ProjectToType<ResultLeadDto>()
                .ToPagedResultAsync(request);
            return Result<PagedResult<ResultLeadDto>>.Success(leads);
        }

        public async Task<Result<GetByIdLeadDto>> GetByIdAsync(int id)
        {
            var lead = await _leadBusinessRules.GetLeadByIdAsync(id);
            _leadBusinessRules.EnsureUserCanModify(lead, "You are not authorized to access this record.");
            var dto = lead.Adapt<GetByIdLeadDto>();
            return Result<GetByIdLeadDto>.Success(dto);
        }
        private static Customer CreateCustomerFromLead(Lead lead,CustomerType customerType)
        {
            return new Customer
            {
                CustomerType = customerType,
                CompanyName = lead.CompanyName,
                ContactFirstName = lead.FirstName,
                ContactLastName = lead.LastName,
                Email = lead.Email,
                PhoneNumber = lead.PhoneNumber,
                Website = lead.Website,
                Address = lead.Address,
                Description = lead.Description,
                AssignedUserId = lead.AssignedUserId
            };
        }
        private async Task CreateInitialDealAsync(Customer customer, Lead lead)
        {
            Deal deal = new()
            {
                Title = $"{customer.ContactFirstName} {customer.ContactLastName}",

                Description = lead.Description,

                Customer = customer,

                AssignedUserId = lead.AssignedUserId ?? 0,

                Stage = DealStage.New,

                Amount = 0
            };

            await _dealRepository.AddAsync(deal);
        }
        private async Task CreateInitialMeetingAsync(Customer customer, Lead lead)
        {
            Meeting meeting = new()
            {
                Title = $"Initial Meeting - {customer.ContactFirstName}",

                CustomerId = customer.Id,

                AssignedUserId = lead.AssignedUserId,

                StartDate = DateTime.UtcNow.AddDays(1),

                EndDate = DateTime.UtcNow.AddDays(1).AddHours(1),

                Status = MeetingStatus.Scheduled,

                Type = MeetingType.Online
            };

            await _meetingRepository.AddAsync(meeting);
        }
        private async Task CreateInitialTaskAsync(Customer customer, Lead lead)
        {
            TaskItem task = new()
            {
                Title = $"Follow up {customer.ContactFirstName}",

                Customer = customer,

                AssignedUserId = lead.AssignedUserId,

                DueDate = DateTime.UtcNow.AddDays(3),

                Priority = TaskPriority.Medium,

                Status = SalesFlow.Entity.Enums.TaskStatus.Pending
            };

            await _taskRepository.AddAsync(task);
        }
        public async Task<byte[]> ExportAsync()
        {
            IQueryable<Lead> query = _leadRepository.GetAll();

            if (!_currentUserService.IsInRole("Admin") &&
                !_currentUserService.IsInRole("SalesManager"))
            {
                query = query.Where(x =>
                    x.AssignedUserId == _currentUserService.UserId);
            }
            List<Lead> leads = await _leadRepository
                .GetAll()
                .OrderBy(x => x.FirstName)
                .ToListAsync();

            return _excelExportService.ExportLeads(leads);
        }
        public async Task<byte[]> ExportPdfAsync()
        {
            IQueryable<Lead> query = _leadRepository.GetAll();

            if (!_currentUserService.IsInRole("Admin") &&
                !_currentUserService.IsInRole("SalesManager"))
            {
                query = query.Where(x =>
                    x.AssignedUserId == _currentUserService.UserId);
            }
            List<Lead> leads = await _leadRepository
                .GetAll()
                .OrderBy(x => x.FirstName)
                .ToListAsync();

            return _pdfExportService.ExportLeads(leads);
        }
    }
}
