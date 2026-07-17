using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SalesFlow.Business.Dtos.CustomerDtos;
using SalesFlow.Business.Dtos.TagDtos;
using SalesFlow.Business.Services.ActivityLogServices;
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
        public CustomerService(ICustomerRepository customerRepository, IUnitOfWork unitOfWork, CustomerBusinessRules customerBusinessRules, IValidator<CreateCustomerDto> createValidator, IValidator<UpdateCustomerDto> updateValidator, IActivityLogService activityLogService, ICurrentUserService currentUserService, IExcelExportService excelExportService, IPdfExportService pdfExportService, IRealtimeService realtimeService)
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

            var tags = customer.CustomerTags
                .Select(x => x.Tag)
                .Adapt<List<ResultTagDto>>();

            return Result<List<ResultTagDto>>.Success(tags);
        }
        public async Task<Result> UpdateAsync(UpdateCustomerDto dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);

            await _customerBusinessRules.EnsureEmailIsUniqueForUpdateAsync(dto.Id, dto.Email);

            var customer = await _customerBusinessRules.GetCustomerByIdAsync(dto.Id, true);
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

            var dto = customer.Adapt<GetByIdCustomerDto>();

            return Result<GetByIdCustomerDto>.Success(dto);
        }
        public async Task<byte[]> ExportAsync()
        {
            List<Customer> customers = await _customerRepository
                .GetAll()
                .OrderBy(x => x.ContactFirstName)
                .ToListAsync();

            return _excelExportService.ExportCustomers(customers);
        }
        public async Task<byte[]> ExportPdfAsync()
        {
            List<Customer> customers = await _customerRepository
                .GetAll()
                .OrderBy(x => x.ContactFirstName)
                .ToListAsync();

            return _pdfExportService.ExportCustomers(customers);
        }
    }
}
