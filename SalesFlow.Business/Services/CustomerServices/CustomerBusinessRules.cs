using SalesFlow.Core.Exceptions;
using SalesFlow.DataAccess.Repositories.CustomerRepositories;
using SalesFlow.Entity.Entities;

namespace SalesFlow.Business.Services.CustomerServices
{
    public class CustomerBusinessRules
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerBusinessRules(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task EnsureEmailIsUniqueAsync(string email)
        {
            bool exists = await _customerRepository.AnyAsync(x => x.Email == email);

            if (exists)
                throw new BusinessException("A customer with this email already exists.");
        }

        public async Task EnsureEmailIsUniqueForUpdateAsync(int customerId, string email)
        {
            bool exists = await _customerRepository.AnyAsync(x => x.Email == email && x.Id != customerId);

            if (exists)
                throw new BusinessException("A customer with this email already exists.");
        }

        public async Task<Customer> GetCustomerByIdAsync(int id, bool tracking = false)
        {
            Customer? customer = await _customerRepository.GetByIdAsync(id, tracking);

            if (customer is null)
                throw new NotFoundException("Customer not found.");

            return customer;
        }
    }
}