using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SalesFlow.Business.Dtos.CustomerDtos;
using SalesFlow.Business.Dtos.TagDtos;
using SalesFlow.Business.Services.ActivityLogServices;
using SalesFlow.Business.Services.AIServices;
using SalesFlow.Business.Services.AIServices.PromptBuilders;
using SalesFlow.Business.Services.AIServices.Prompts;
using SalesFlow.Business.Services.ExportServices;
using SalesFlow.Business.Services.RealtimeServices;
using SalesFlow.Business.Services.UserServices;
using SalesFlow.Core.Exceptions;
using SalesFlow.Core.Paginations;
using SalesFlow.Core.Results;
using SalesFlow.DataAccess.Repositories.CustomerRepositories;
using SalesFlow.DataAccess.Uows;
using SalesFlow.Entity.Entities;
using SalesFlow.Entity.Enums;

namespace SalesFlow.Business.Services.CustomerServices
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly CustomerBusinessRules _customerBusinessRules;
        private readonly IValidator<CreateCustomerDto> _createValidator;
        private readonly IValidator<UpdateCustomerDto> _updateValidator;
        private readonly IActivityLogService _activityLogService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IExcelExportService _excelExportService;
        private readonly IPdfExportService _pdfExportService;
        private readonly IRealtimeService _realtimeService;
        private readonly IOpenAiService _openAiService;

        public CustomerService(ICustomerRepository customerRepository, IUnitOfWork unitOfWork, CustomerBusinessRules customerBusinessRules, IValidator<CreateCustomerDto> createValidator, IValidator<UpdateCustomerDto> updateValidator, IActivityLogService activityLogService, ICurrentUserService currentUserService, IExcelExportService excelExportService, IPdfExportService pdfExportService, IRealtimeService realtimeService, IOpenAiService openAiService)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
            _customerBusinessRules = customerBusinessRules;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _activityLogService = activityLogService;
            _currentUserService = currentUserService;
            _excelExportService = excelExportService;
            _pdfExportService = pdfExportService;
            _realtimeService = realtimeService;
            _openAiService = openAiService;
        }
        public async Task<Result<string>> GenerateFollowUpEmailAsync(
    int customerId,
    GenerateFollowUpEmailDto dto)
        {
            Customer customer =
                await _customerBusinessRules.GetCustomerForAiInsightsAsync(customerId);

            _customerBusinessRules.EnsureUserCanAccess(customer);

            string prompt =
                FollowUpEmailPromptBuilder.Build(customer, dto);

            string email =
                await _openAiService.GenerateAsync(
                    FollowUpEmailPrompt.System,
                    prompt);

            return Result<string>.Success(email);
        }
        public async Task<Result<string>> GenerateInsightsAsync(int customerId)
        {
            Customer customer =
                await _customerBusinessRules.GetCustomerForAiInsightsAsync(customerId);

            _customerBusinessRules.EnsureUserCanAccess(customer);

            string prompt = CustomerPromptBuilder.Build(customer);

            string result = await _openAiService.GenerateAsync(
                CustomerSummaryPrompt.System,
                prompt);

            return Result<string>.Success(result);
        }
        public async Task<Result> CreateAsync(CreateCustomerDto dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);
            await _customerBusinessRules.EnsureEmailIsUniqueAsync(dto.Email);

            var customer = dto.Adapt<Customer>();

            await _customerRepository.AddAsync(customer);
            await _activityLogService.AddAsync(ActivityAction.Create, nameof(Customer),customer.Id,$"Customer '{customer.ContactFirstName} {customer.ContactLastName}' created.",_currentUserService.UserId);


            await _unitOfWork.SaveChangesAsync();
            await _realtimeService.DashboardUpdatedAsync();

            return Result.Success("Customer created successfully.");
        }
        public async Task<Result> AddTagAsync(int customerId, int tagId)
        {
            var customer = await _customerBusinessRules.GetCustomerByIdAsync(customerId, true);
            _customerBusinessRules.EnsureUserCanAccess(customer);

            await _customerBusinessRules.EnsureTagExistsAsync(tagId);

            await _customerBusinessRules.EnsureCustomerTagNotExistsAsync(customerId, tagId);

            customer.CustomerTags.Add(new CustomerTag
            {
                CustomerId = customerId,
                TagId = tagId
            });

            await _unitOfWork.SaveChangesAsync();

            return Result.Success("Tag added successfully.");
        }
        public async Task<Result> RemoveTagAsync(int customerId, int tagId)
        {
            var customer = await _customerRepository
                .GetCustomerWithTagsAsync(customerId, true);

            if (customer is null)
                throw new NotFoundException("Customer not found.");
            _customerBusinessRules.EnsureUserCanAccess(customer);
            var customerTag = customer.CustomerTags
                .FirstOrDefault(x => x.TagId == tagId);

            if (customerTag is null)
                throw new BusinessException("Tag not assigned to this customer.");

            customer.CustomerTags.Remove(customerTag);

            await _unitOfWork.SaveChangesAsync();

            return Result.Success("Tag removed successfully.");
        }
        public async Task<Result<List<ResultTagDto>>> GetTagsAsync(int customerId)
        {
            var customer = await _customerRepository
                .GetCustomerWithTagsAsync(customerId);

            if (customer is null)
                throw new NotFoundException("Customer not found.");
            _customerBusinessRules.EnsureUserCanAccess(customer);
            var tags = customer.CustomerTags
                .Select(x => x.Tag)
                .Adapt<List<ResultTagDto>>();

            return Result<List<ResultTagDto>>.Success(tags);
        }
        public async Task<Result> UpdateAsync(UpdateCustomerDto dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);

            var customer = await _customerBusinessRules.GetCustomerByIdAsync(dto.Id, true);

            _customerBusinessRules.EnsureUserCanAccess(customer);

            await _customerBusinessRules.EnsureEmailIsUniqueForUpdateAsync(dto.Id, dto.Email);
            dto.Adapt(customer);
            _customerRepository.Update(customer);
            await _activityLogService.AddAsync(ActivityAction.Update, nameof(Customer), customer.Id, $"Customer '{customer.ContactFirstName} {customer.ContactLastName}' updated.", _currentUserService.UserId);
            await _unitOfWork.SaveChangesAsync();
            await _realtimeService.DashboardUpdatedAsync();
            return Result.Success("Customer updated successfully.");
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var customer = await _customerBusinessRules.GetCustomerByIdAsync(id, true);
            _customerBusinessRules.EnsureUserCanAccess(customer);
            _customerRepository.Delete(customer);
            await _activityLogService.AddAsync(ActivityAction.Delete, nameof(Customer),customer.Id,$"Customer '{customer.ContactFirstName} {customer.ContactLastName}' deleted.",_currentUserService.UserId);
            await _realtimeService.DashboardUpdatedAsync();
            await _unitOfWork.SaveChangesAsync();
            return Result.Success("Customer deleted successfully.");
        }

        public async Task<Result<PagedResult<ResultCustomerDto>>> GetAllAsync(
    CustomerFilterRequest request)
        {
            IQueryable<Customer> query = _customerRepository.GetAll();
            if (!_currentUserService.IsInRole("Admin") &&
    !_currentUserService.IsInRole("SalesManager"))
            {
                query = query.Where(x =>
                    x.AssignedUserId == _currentUserService.UserId);
            }
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                string search = request.Search.Trim().ToLower();

                query = query.Where(x =>

    (x.CompanyName != null &&
     EF.Functions.Like(x.CompanyName, $"%{request.Search}%"))

    ||

    EF.Functions.Like(x.ContactFirstName, $"%{request.Search}%")

    ||

    EF.Functions.Like(x.ContactLastName, $"%{request.Search}%")

    ||

    EF.Functions.Like(x.Email, $"%{request.Search}%"));
            }

            if (request.CustomerType.HasValue)
            {
                query = query.Where(x =>
                    x.CustomerType == request.CustomerType.Value);
            }

            if (request.AssignedUserId.HasValue)
            {
                query = query.Where(x =>
                    x.AssignedUserId == request.AssignedUserId.Value);
            }

            PagedResult<ResultCustomerDto> customers = await query
                .OrderByDescending(x => x.CreatedDate)
                .ProjectToType<ResultCustomerDto>()
                .ToPagedResultAsync(request);

            return Result<PagedResult<ResultCustomerDto>>
                .Success(customers);
        }

        public async Task<Result<GetByIdCustomerDto>> GetByIdAsync(int id)

        {
            var customer = await _customerBusinessRules.GetCustomerByIdAsync(id);
            _customerBusinessRules.EnsureUserCanAccess(customer, "You are not authorized to access this record.");

            var dto = customer.Adapt<GetByIdCustomerDto>();

            return Result<GetByIdCustomerDto>.Success(dto);
        }
        public async Task<byte[]> ExportAsync()
        {
            IQueryable<Customer> query = _customerRepository.GetAll();

            if (!_currentUserService.IsInRole("Admin") &&
                !_currentUserService.IsInRole("SalesManager"))
            {
                query = query.Where(x =>
                    x.AssignedUserId == _currentUserService.UserId);
            }
            var orderedQuery = query.OrderBy(x => x.ContactFirstName);
            List<Customer> customers = orderedQuery.Provider is IAsyncQueryProvider
                ? await orderedQuery.ToListAsync()
                : orderedQuery.ToList();


            return _excelExportService.ExportCustomers(customers);
        }
        public async Task<byte[]> ExportPdfAsync()
        {
            IQueryable<Customer> query = _customerRepository.GetAll();

            if (!_currentUserService.IsInRole("Admin") &&
                !_currentUserService.IsInRole("SalesManager"))
            {
                query = query.Where(x =>
                    x.AssignedUserId == _currentUserService.UserId);
            }
            var orderedQuery = query.OrderBy(x => x.ContactFirstName);
            List<Customer> customers = orderedQuery.Provider is IAsyncQueryProvider
                ? await orderedQuery.ToListAsync()
                : orderedQuery.ToList();


            return _pdfExportService.ExportCustomers(customers);
        }
    }
}
