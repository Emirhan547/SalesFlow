using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SalesFlow.Business.Dtos.CustomerDtos;
using SalesFlow.Core.Paginations;
using SalesFlow.Core.Results;
using SalesFlow.DataAccess.Repositories.CustomerRepositories;
using SalesFlow.DataAccess.Uows;
using SalesFlow.Entity.Entities;

namespace SalesFlow.Business.Services.CustomerServices
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly CustomerBusinessRules _customerBusinessRules;
        private readonly IValidator<CreateCustomerDto> _createValidator;
        private readonly IValidator<UpdateCustomerDto> _updateValidator;
        public CustomerService(ICustomerRepository customerRepository, IUnitOfWork unitOfWork, CustomerBusinessRules customerBusinessRules, IValidator<CreateCustomerDto> createValidator, IValidator<UpdateCustomerDto> updateValidator)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
            _customerBusinessRules = customerBusinessRules;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<Result> CreateAsync(CreateCustomerDto dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);
            await _customerBusinessRules.EnsureEmailIsUniqueAsync(dto.Email);

            var customer = dto.Adapt<Customer>();

            await _customerRepository.AddAsync(customer);

            await _unitOfWork.SaveChangesAsync();

            return Result.Success("Customer created successfully.");
        }

        public async Task<Result> UpdateAsync(UpdateCustomerDto dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);

            await _customerBusinessRules.EnsureEmailIsUniqueForUpdateAsync(dto.Id, dto.Email);

            var customer = await _customerBusinessRules.GetCustomerByIdAsync(dto.Id, true);
            dto.Adapt(customer);
            _customerRepository.Update(customer);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success("Customer updated successfully.");
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var customer = await _customerBusinessRules.GetCustomerByIdAsync(id, true);
            _customerRepository.Delete(customer);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success("Customer deleted successfully.");
        }

        public async Task<Result<PagedResult<ResultCustomerDto>>> GetAllAsync(PaginationRequest request)
        {
            var customers = await _customerRepository
                .GetAll()
                .ProjectToType<ResultCustomerDto>()
                .ToPagedResultAsync(request);

            return Result<PagedResult<ResultCustomerDto>>.Success(customers);
        }

        public async Task<Result<GetByIdCustomerDto>> GetByIdAsync(int id)
        {
            var customer = await _customerBusinessRules.GetCustomerByIdAsync(id);

            var dto = customer.Adapt<GetByIdCustomerDto>();

            return Result<GetByIdCustomerDto>.Success(dto);
        }
    }
}
