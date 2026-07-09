using SalesFlow.Core.Exceptions;
using SalesFlow.DataAccess.Repositories.CustomerRepositories;
using SalesFlow.DataAccess.Repositories.TagRepositories;
using SalesFlow.Entity.Entities;

namespace SalesFlow.Business.Services.CustomerServices
{
    public class CustomerBusinessRules
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ITagRepository _tagRepository;
        public CustomerBusinessRules(ICustomerRepository customerRepository, ITagRepository tagRepository)
        {
            _customerRepository = customerRepository;
            _tagRepository = tagRepository;
        }
        public async Task EnsureTagExistsAsync(int tagId)
        {
            var exists = await _tagRepository.AnyAsync(x => x.Id == tagId);

            if (!exists)
                throw new BusinessException("Tag not found.");
        }
        public async Task<CustomerTag> GetCustomerTagAsync(int customerId, int tagId)
        {
            var customer = await _customerRepository.GetCustomerWithTagsAsync(customerId, true)
            ?? throw new NotFoundException("Customer not found.");

            var customerTag = customer.CustomerTags.FirstOrDefault(x => x.TagId == tagId);

            if (customerTag is null)
                throw new BusinessException("Customer tag not found.");

            return customerTag;
        }
        public async Task EnsureCustomerTagNotExistsAsync(int customerId, int tagId)
        {
            var exists = await _customerRepository.AnyAsync(x =>
                x.Id == customerId &&
                x.CustomerTags.Any(ct => ct.TagId == tagId));

            if (exists)
                throw new BusinessException("This customer already has this tag.");
        }
        public async Task EnsureEmailIsUniqueAsync(string email)
        {
            var exists = await _customerRepository.AnyAsync(x => x.Email == email);

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
            var customer = await _customerRepository.GetByIdAsync(id, tracking);

            if (customer is null)
                throw new NotFoundException("Customer not found.");

            return customer;
        }
        public async Task EnsureCustomerExistsAsync(int customerId)
        {
            bool exists = await _customerRepository.AnyAsync(x => x.Id == customerId);

            if (!exists)
                throw new BusinessException("Customer not found.");
        }
    }
}